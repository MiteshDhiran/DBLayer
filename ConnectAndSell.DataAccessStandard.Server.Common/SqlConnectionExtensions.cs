using System;
using System.Data;
using System.Transactions;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
  public static class SqlConnectionExtensions
  {
   
      /*
    public static void OpenWithRetry(this SqlConnection connection)
    {
      connection.OpenWithRetry(RetryPolicyBase.Default);
    }

    public static void OpenWithRetry(this SqlConnection connection, RetryPolicyBase retryPolicy)
    {
      (retryPolicy == null || !(Transaction.Current == (Transaction) null) ? RetryPolicyBase.NoRetry : retryPolicy).ExecuteAction((Action) (() =>
      {
        if (connection == null || connection.State == ConnectionState.Open)
          return;
        connection.Open();
      }));
    }
    */

    /*public static Guid GetSessionTracingId(
      this SqlConnection connection,
      RetryPolicyBase connectionRetryPolicy,
      RetryPolicyBase commandRetryPolicy)
    {
      try
      {
        connection.OpenWithRetry(connectionRetryPolicy);
        using (SqlCommand command = new SqlCommand())
        {
          command.Connection = connection;
          command.CommandType = CommandType.Text;
          command.CommandText = Resources.QueryGetContextInfo;
          object obj = command.ExecuteScalarWithRetry(commandRetryPolicy);
          if (obj == null)
            return Guid.Empty;
          Guid result;
          Guid.TryParse(obj.ToString(), out result);
          return result;
        }
      }
      catch
      {
        return Guid.Empty;
      }
      finally
      {
        if (connection != null && connection.State != ConnectionState.Closed)
          connection.Close();
      }
    }

    public static void SetContextInfo(
      this SqlConnection connection,
      byte[] contextInfoData,
      RetryPolicyBase connectionRetryPolicy,
      RetryPolicyBase commandRetryPolicy)
    {
      try
      {
        connection.OpenWithRetry(connectionRetryPolicy);
        using (SqlCommand command = new SqlCommand())
        {
          command.Connection = connection;
          command.CommandType = CommandType.Text;
          command.CommandText = Resources.SetContextInfo;
          SqlParameterCollection parameters = command.Parameters;
          SqlParameter sqlParameter = new SqlParameter();
          sqlParameter.DbType = DbType.Binary;
          sqlParameter.Size = 128;
          sqlParameter.ParameterName = "@contextInfoData";
          sqlParameter.Value = (object) contextInfoData;
          parameters.Add(sqlParameter);
          command.ExecuteNonQueryWithRetry(commandRetryPolicy);
        }
      }
      finally
      {
        if (connection != null && connection.State != ConnectionState.Closed)
          connection.Close();
      }
    }*/
  }
}
