using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class RetryPolicyBase
  {
    private static readonly TimeSpan RETRYINTERVAL = TimeSpan.FromSeconds(5.0);
    //private static string checkFailOverSqlQuery = "SELECT R.REPLICA_SERVER_NAME AS REPLICA_SERVER_NAME,G.NAME AS AVAILABILITYGROUPNAME,RS.ROLE_DESC AS ROLE_DESC,RS.OPERATIONAL_STATE_DESC AS OPERATIONAL_STATE,RS.CONNECTED_STATE_DESC AS CONNECTED_STATE,RS.RECOVERY_HEALTH_DESC AS RECOVERY_HEALTH,RS.SYNCHRONIZATION_HEALTH_DESC AS SYNCHRONIZATION_HEALTH, * FROM SYS.AVAILABILITY_REPLICAS R INNER JOIN SYS.AVAILABILITY_GROUPS G ON R.GROUP_ID = G.GROUP_ID INNER JOIN SYS.DM_HADR_AVAILABILITY_REPLICA_STATES RS ON R.REPLICA_ID = RS.REPLICA_ID INNER JOIN sys.availability_group_listeners L ON  L.Group_id = G.Group_id where RS.CONNECTED_STATE = 1 AND SYNCHRONIZATION_HEALTH_DESC = 'Healthy' AND replica_server_name = @@servername AND L.DNS_Name = 'datasource'";
    private const int MAXRETRYCOUNT = 12;
    private readonly ShouldRetry shouldRetry;
    private RetryCallback retryCallback;

    public int MaximumRetryCount { get; protected set; }

    public static int DefaultMaximumRetryCount { get; protected set; }

    public static TimeSpan DefaultRetryInterval { get; protected set; }

    public static Func<Exception, bool> ExtendedTransientErrorProcessor { get; set; }

    static RetryPolicyBase()
    {
      DefaultMaximumRetryCount = 12;
      DefaultRetryInterval = RETRYINTERVAL;
    }

    public static RetryPolicyBase NoRetry => new RetryPolicyBase(0);

    public static RetryPolicyBase Default => new RetryPolicyBase(DefaultMaximumRetryCount, DefaultRetryInterval);

    public static RetryPolicyBase DefaultIncremental => new RetryPolicyBase(DefaultMaximumRetryCount, DefaultRetryInterval, DefaultRetryInterval);

    public RetryPolicyBase(int retryCount)
      : this(retryCount, DefaultRetryInterval)
    {
    }

    public RetryPolicyBase(int retryCount, TimeSpan intervalBetweenRetries)
    {
      MaximumRetryCount = retryCount;
      if (retryCount == 0)
        shouldRetry = (int currentRetryCount, Exception lastException, out TimeSpan retryInterval) =>
        {
            retryInterval = TimeSpan.Zero;
            return false;
        };
      else
        shouldRetry = (int currentRetryCount, Exception lastException, out TimeSpan retryInterval) =>
        {
            if (IsTransientError(lastException))
            {
                retryInterval = intervalBetweenRetries;
                return currentRetryCount < retryCount;
            }
            retryInterval = TimeSpan.Zero;
            return false;
        };
    }

    public RetryPolicyBase(int retryCount, TimeSpan initialInterval, TimeSpan increment)
    {
      MaximumRetryCount = retryCount;
      shouldRetry = (int currentRetryCount, Exception lastException, out TimeSpan retryInterval) =>
      {
          if (IsTransientError(lastException) && currentRetryCount < retryCount)
          {
              retryInterval = TimeSpan.FromMilliseconds(initialInterval.TotalMilliseconds + increment.TotalMilliseconds * currentRetryCount);
              return true;
          }
          retryInterval = TimeSpan.Zero;
          return false;
      };
    }

    public RetryCallback RetryCallback
    {
      get => retryCallback;
      set => retryCallback = value;
    }

    public void ExecuteAction(Action action)
    {
      ExecuteAction(action, null);
    }

    public T ExecuteAction<T>(Func<T> func)
    {
      return ExecuteAction(func, null);
    }

    internal void ExecuteAction(Action action, RetryCallback customRetryCallback)
    {
      int currentRetryCount = 0;
      TimeSpan delay = TimeSpan.Zero;
      while (true)
      {
        Exception lastException;
        try
        {
          action();
          break;
        }
        catch (RetryLimitExceededException ex)
        {
          if (ex.InnerException == null)
            break;
          throw ex.InnerException;
        }
        catch (Exception ex)
        {
          lastException = ex;
          if (!shouldRetry(currentRetryCount++, lastException, out delay))
          {
            SqlException(ex);
            throw;
          }
        }
        if (customRetryCallback != null)
          customRetryCallback(currentRetryCount, lastException, delay);
        else
        {
            retryCallback?.Invoke(currentRetryCount, lastException, delay);
        }

        Thread.Sleep(delay);
      }
    }

    internal T ExecuteAction<T>(Func<T> func, RetryCallback customRetryCallback)
    {
      int currentRetryCount = 0;
      TimeSpan delay = TimeSpan.Zero;
      while (true)
      {
        Exception lastException;
        try
        {
          return func();
        }
        catch (RetryLimitExceededException ex)
        {
          if (ex.InnerException != null)
            throw ex.InnerException;
          return default(T);
        }
        catch (Exception ex)
        {
          lastException = ex;
          if (!shouldRetry(currentRetryCount++, lastException, out delay))
          {
            SqlException(ex);
            throw;
          }
        }
        if (customRetryCallback != null)
          customRetryCallback(currentRetryCount, lastException, delay);
        else if (retryCallback != null)
          retryCallback(currentRetryCount, lastException, delay);
        Thread.Sleep(delay);
      }
    }

    /*
    private static void DetectFailOver(Exception ex)
    {
      string exceptionMessage = ex.Message;
      if (!GetSqlErrorNumbers(ex).Any(sqlErrorNumber => sqlErrorNumber.Equals(596)))
        return;
      SqlException sqlException = ex as SqlException;
      if (sqlException == null)
        throw new Exception($"{"No sql exception found, No re-try would happen."}{exceptionMessage}");
      string server = sqlException.Server;
      if (string.IsNullOrWhiteSpace(server))
        throw new Exception($"{"No server name found, No re-try would happen."}{exceptionMessage}");
      string connectionString = "Data Source=" + server + ";Initial Catalog=master;Integrated Security=SSPI";
      checkFailOverSqlQuery = checkFailOverSqlQuery.Replace("datasource", server);
      using (TransactionRetryScope transactionRetryScope = new TransactionRetryScope(TransactionScopeOption.Suppress, TimeSpan.FromSeconds(5.0), Default, () =>
      {
          using (SqlConnection sqlConnection = new SqlConnection(connectionString))
          {
              DataSet dataSet = new SqlCommand(checkFailOverSqlQuery)
              {
                  Connection = sqlConnection,
                  CommandType = CommandType.Text
              }.ExecuteDatasetWithRetry();
              if (dataSet.Tables.Count <= 0 || dataSet.Tables[0].Rows.Count <= 0)
                  throw new Exception(
                      $"{"No data returned from check fail over query, No re-try would happen."}{exceptionMessage}");
              if (!dataSet.Tables[0].AsEnumerable().Where(row =>
              {
                  if (row.Field<string>("ROLE_DESC") == "SECONDARY" || row.Field<string>("ROLE_DESC") == "PRIMARY")
                      return row.Field<string>("OPERATIONAL_STATE") == "ONLINE";
                  return false;
              }).Any())
                  throw new Exception(
                      $"{"there is no row that has ROLE_DESC as SECONDARY or PRIMARY and OPERATIONAL_STATE as ONLINE."}{exceptionMessage}");
          }
      }))
      {
        transactionRetryScope.InvokeUnitOfWork();
        transactionRetryScope.Complete();
      }
    }
    */
    
    private static void SqlException(Exception ex)
    {
      SqlException sqlException = ex as SqlException;
      if (sqlException == null)
        return;
      foreach (SqlError error in sqlException.Errors)
      {
        if (error.Number == 17892)
          throw new Exception(string.Format("{0}{1}System may be unavailable due to UPGRADE. Please try again later, or contact your Support Desk", ex.Message, Environment.NewLine));
      }
    }

    private static IEnumerable<int> GetSqlErrorNumbers(Exception ex)
    {
      SqlException sqlException = ex as SqlException;
      if (sqlException != null)
      {
        foreach (SqlError error in sqlException.Errors)
          yield return error.Number;
      }
    }

    public static bool IsTransientError(Exception ex)
    {
      bool flag1 = false;
      if (ex != null)
      {
        if (ex is SqlException sqlException)
        {
          bool flag2 = true;
          bool flag3 = false;
          foreach (SqlError error in sqlException.Errors)
          {
            flag2 &= IsTransientErrorCollectionItem(error);
            flag3 = IsTranciencyOverrider(error);
            if (flag3)
              break;
          }
          flag1 = flag3 | flag2;
        }
        else if (ex is TimeoutException)
          flag1 = true;
        else if (ex is InvalidOperationException)
        {
          if (ex.Message.Contains("The timeout period elapsed prior to obtaining a connection from the pool"))
            flag1 = true;
        }
        else if (ex.InnerException != null)
          flag1 = IsTransientError(ex.InnerException);
      }
      if (!flag1 && ExtendedTransientErrorProcessor != null)
        flag1 = flag1 || ExtendedTransientErrorProcessor(ex);
      return flag1;
    }

    private static bool IsTranciencyOverrider(SqlError sqlError)
    {
      return sqlError.Number == 6005;
    }

    public static bool IsTransientErrorCollectionItem(SqlError sqlError)
    {
      bool flag;
      switch (sqlError.Number)
      {
        case -2:
        case -1:
        case 0:
        case 2:
        case 53:
        case 64:
        case 121:
        case 233:
        case 315:
        case 539:
        case 596:
        case 615:
        case 652:
        case 667:
        case 670:
        case 671:
        case 679:
        case 983:
        case 988:
        case 1204:
        case 1205:
        case 1206:
        case 1222:
        case 2801:
        case 3303:
        case 3621:
        case 3906:
        case 3960:
        case 4060:
        case 4083:
        case 6005:
        case 10054:
        case 10060:
        case 10061:
        case 21386:
          flag = true;
          break;
        default:
          flag = false;
          break;
      }
      return flag;
    }

    private delegate bool ShouldRetry(int retryCount, Exception lastException, out TimeSpan delay);
  }
}
