using System;
using System.Data;
using System.Transactions;
using System.Xml;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public static class SqlCommandExtensions
  {
    public static int ExecuteNonQueryWithRetry(this SqlCommand command)
    {
      return command.ExecuteNonQueryWithRetry(RetryPolicyBase.Default);
    }

    public static int ExecuteNonQueryWithRetry(this SqlCommand command, RetryPolicyBase retryPolicy)
    {
      return command.ExecuteNonQueryWithRetry(retryPolicy, RetryPolicyBase.NoRetry);
    }

    public static int ExecuteNonQueryWithRetry(
      this SqlCommand command,
      RetryPolicyBase cmdRetryPolicy,
      RetryPolicyBase conRetryPolicy)
    {
      RetryPolicyBase retryPolicyBase = cmdRetryPolicy;
      return (cmdRetryPolicy == null || command.Transaction != null ? RetryPolicyBase.NoRetry : retryPolicyBase).ExecuteAction<int>((Func<int>) (() =>
      {
        SqlCommandExtensions.EnsureSQLServerTest(command, conRetryPolicy);
        return command.ExecuteNonQuery();
      }));
    }

    public static SqlDataReader ExecuteReaderWithRetry(this SqlCommand command)
    {
      return command.ExecuteReaderWithRetry(RetryPolicyBase.Default);
    }

    public static SqlDataReader ExecuteReaderWithRetry(
      this SqlCommand command,
      RetryPolicyBase retryPolicy)
    {
      return command.ExecuteReaderWithRetry(retryPolicy, RetryPolicyBase.NoRetry);
    }

    public static SqlDataReader ExecuteReaderWithRetry(
      this SqlCommand command,
      RetryPolicyBase cmdRetryPolicy,
      RetryPolicyBase conRetryPolicy)
    {
      return (cmdRetryPolicy == null || command.Transaction != null ? RetryPolicyBase.NoRetry : cmdRetryPolicy).ExecuteAction<SqlDataReader>((Func<SqlDataReader>) (() =>
      {
        SqlCommandExtensions.EnsureSQLServerTest(command, conRetryPolicy);
        return command.ExecuteReader();
      }));
    }

    public static SqlDataReader ExecuteReaderWithRetry(
      this SqlCommand command,
      CommandBehavior behavior)
    {
      return command.ExecuteReaderWithRetry(behavior, RetryPolicyBase.Default);
    }

    public static SqlDataReader ExecuteReaderWithRetry(
      this SqlCommand command,
      CommandBehavior behavior,
      RetryPolicyBase retryPolicy)
    {
      return command.ExecuteReaderWithRetry(behavior, retryPolicy, RetryPolicyBase.NoRetry);
    }

    public static SqlDataReader ExecuteReaderWithRetry(
      this SqlCommand command,
      CommandBehavior behavior,
      RetryPolicyBase cmdRetryPolicy,
      RetryPolicyBase conRetryPolicy)
    {
      return (cmdRetryPolicy == null || command.Transaction != null ? RetryPolicyBase.NoRetry : cmdRetryPolicy).ExecuteAction<SqlDataReader>((Func<SqlDataReader>) (() =>
      {
        SqlCommandExtensions.EnsureSQLServerTest(command, conRetryPolicy);
        return command.ExecuteReader(behavior);
      }));
    }

    public static object ExecuteScalarWithRetry(this SqlCommand command)
    {
      return command.ExecuteScalarWithRetry(RetryPolicyBase.Default);
    }

    public static object ExecuteScalarWithRetry(
      this SqlCommand command,
      RetryPolicyBase retryPolicy)
    {
      return command.ExecuteScalarWithRetry(retryPolicy, RetryPolicyBase.NoRetry);
    }

    public static object ExecuteScalarWithRetry(
      this SqlCommand command,
      RetryPolicyBase cmdRetryPolicy,
      RetryPolicyBase conRetryPolicy)
    {
      return (cmdRetryPolicy == null || command.Transaction != null ? RetryPolicyBase.NoRetry : cmdRetryPolicy).ExecuteAction<object>((Func<object>) (() =>
      {
        SqlCommandExtensions.EnsureSQLServerTest(command, conRetryPolicy);
        return command.ExecuteScalar();
      }));
    }

    public static XmlReader ExecuteXmlReaderWithRetry(this SqlCommand command)
    {
      return command.ExecuteXmlReaderWithRetry(RetryPolicyBase.Default);
    }

    public static XmlReader ExecuteXmlReaderWithRetry(
      this SqlCommand command,
      RetryPolicyBase retryPolicy)
    {
      return command.ExecuteXmlReaderWithRetry(retryPolicy, RetryPolicyBase.NoRetry);
    }

    public static XmlReader ExecuteXmlReaderWithRetry(
      this SqlCommand command,
      RetryPolicyBase cmdRetryPolicy,
      RetryPolicyBase conRetryPolicy)
    {
      return (cmdRetryPolicy == null || command.Transaction != null ? RetryPolicyBase.NoRetry : cmdRetryPolicy).ExecuteAction<XmlReader>((Func<XmlReader>) (() =>
      {
        SqlCommandExtensions.EnsureSQLServerTest(command, conRetryPolicy);
        return command.ExecuteXmlReader();
      }));
    }

    public static DataSet ExecuteDatasetWithRetry(
      this SqlCommand command,
      DataSet? dataset = null,
      SqlDataAdapter? da = null)
    {
      return command.ExecuteDatasetWithRetry(RetryPolicyBase.Default, dataset, da);
    }

    public static DataSet ExecuteDatasetWithRetry(
      this SqlCommand command,
      RetryPolicyBase retryPolicy,
      DataSet? dataset = null,
      SqlDataAdapter? da = null)
    {
      return command.ExecuteDatasetWithRetry(retryPolicy, RetryPolicyBase.NoRetry, dataset, da);
    }

    public static DataSet ExecuteDatasetWithRetry(
      this SqlCommand command,
      RetryPolicyBase? cmdRetryPolicy,
      RetryPolicyBase conRetryPolicy,
      DataSet? dataset = null,
      SqlDataAdapter? da = null)
    {
      return (cmdRetryPolicy == null || command.Transaction != null ? RetryPolicyBase.NoRetry : cmdRetryPolicy).ExecuteAction<DataSet>((Func<DataSet>) (() =>
      {
        DataSet dataSet = dataset == null ? new DataSet() : dataset;
        SqlCommandExtensions.EnsureSQLServerTest(command, conRetryPolicy);
        if (da != null)
          da.Fill(dataSet);
        else
          new SqlDataAdapter(command).Fill(dataSet);
        return dataSet;
      }));
    }

    public static object ExecuteScalarWithRetry(
      this SqlCommand command,
      string extractFromColumnName)
    {
      return command.ExecuteScalarWithRetry(RetryPolicyBase.Default, extractFromColumnName);
    }

    public static object ExecuteScalarWithRetry(
      this SqlCommand command,
      RetryPolicyBase retryPolicy,
      string extractFromColumnName)
    {
      return command.ExecuteScalarWithRetry(retryPolicy, RetryPolicyBase.NoRetry, extractFromColumnName);
    }

    public static object ExecuteScalarWithRetry(
      this SqlCommand command,
      RetryPolicyBase cmdRetryPolicy,
      RetryPolicyBase conRetryPolicy,
      string extractFromColumnName)
    {
      return (cmdRetryPolicy == null || command.Transaction != null ? RetryPolicyBase.NoRetry : cmdRetryPolicy).ExecuteAction<object>((Func<object>) (() =>
      {
        SqlCommandExtensions.EnsureSQLServerTest(command, conRetryPolicy);
        if (string.IsNullOrWhiteSpace(extractFromColumnName))
          return command.ExecuteScalar();
        SqlDataReader sqlDataReader = command.ExecuteReader();
        if (!sqlDataReader.HasRows)
          return (object) null;
        sqlDataReader.Read();
        return sqlDataReader[extractFromColumnName];
      }));
    }

    private static void EnsureSQLServerTest(SqlCommand command, RetryPolicyBase retryPolicy)
    {
      //if (command == null || command.Connection != null && command.Connection.State == ConnectionState.Open)
      //  return;
      //command.Connection.OpenWithRetry(retryPolicy);
    }
  }
}
