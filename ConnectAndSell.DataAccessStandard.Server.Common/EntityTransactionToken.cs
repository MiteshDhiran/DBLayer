using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class EntityTransactionToken
    {
        public TraceInfo PreviousTraceInfo { get; }
        internal SqlConnection Connection { get; }
        internal SqlTransaction Transaction { get; }
        internal EntityTransactionToken(TraceInfo previousTraceInfo, SqlConnection connection, SqlTransaction transaction)
        {
            this.PreviousTraceInfo = previousTraceInfo ?? throw new ArgumentNullException($"{nameof(transaction)}");
            this.Connection = connection ?? throw new ArgumentNullException($"{nameof(connection)}");
            this.Transaction = transaction ?? throw new ArgumentNullException($"{nameof(transaction)}");
        }
    }
}