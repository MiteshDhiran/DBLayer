using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class SqlExecutionContext
    {
        public SqlExecutionContext(SqlConnection connection, SqlTransaction transaction, ORMModelMetaInfo modelMetaInfo,
            TraceInfo currentTraceInfo)
        {
            Connection = connection ?? throw new ArgumentNullException(nameof(connection));
            Transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
            ModelMetaInfo = modelMetaInfo ?? throw new ArgumentNullException(nameof(modelMetaInfo));
            CurrentTraceInfo = currentTraceInfo ?? throw new ArgumentNullException(nameof(currentTraceInfo));
        }

        public SqlConnection Connection { get; }
        public SqlTransaction Transaction { get; }
        public ORMModelMetaInfo ModelMetaInfo { get; }
        public TraceInfo CurrentTraceInfo { get; }
        
    }
}
