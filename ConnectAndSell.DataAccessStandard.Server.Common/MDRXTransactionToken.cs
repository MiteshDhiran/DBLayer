using System;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public class MDRXTransactionToken
    {
        public TraceInfo PreviousTraceInfo { get; }
        internal SqlConnection Connection { get; }
        internal SqlTransaction Transaction { get; }
        internal MDRXTransactionToken(TraceInfo previousTraceInfo, SqlConnection connection, SqlTransaction transaction)
        {
            this.PreviousTraceInfo = previousTraceInfo ?? throw new ArgumentNullException($"{nameof(transaction)}");
            this.Connection = connection ?? throw new ArgumentNullException($"{nameof(connection)}");
            this.Transaction = transaction ?? throw new ArgumentNullException($"{nameof(transaction)}");
        }
    }
}