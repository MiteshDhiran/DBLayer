using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public static class DeleteTSQLCommandHelper
    {
        public static bool ExecuteDeleteCommand<TK, TP>(ORMModelMetaInfo modelMetaInfo,SqlConnection connection,SqlTransaction transaction, List<TP> primaryKeyValues)
        {
            var xmlSerializedPKValues = DataContractSerializerHelper.DataContractSerialize(primaryKeyValues);
            var deleteTSQL = modelMetaInfo.GetDeleteTSQLForType(typeof(TK));
            
            /*
             var declareAndSetInputArg = $@"
                                            DECLARE @inputArg XML;
                                            SET @inputArg = '{xmlSerializedPKValues}';
                                          ";

            var sql = $@"
                            SET NOCOUNT ON;
                            {declareAndSetInputArg}
                            {deleteTSQL}
                        ";
            */
            
            var sqlExpectingParameter = $@"
                            SET NOCOUNT ON;
                            {deleteTSQL}
                        ";

            var sqlParameter = new SqlParameter("@inputArg", SqlDbType.Xml,int.MaxValue) {Value = xmlSerializedPKValues};
            var parameters = new List<DbParameter>() {sqlParameter};
            var allRecordsDeletedIncludingChild =DBUtility.ExecuteNonQueryCommand(connection, transaction, parameters, sqlExpectingParameter);
            return true;
        }
    }
}
