using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;
using ConnectAndSell.DataAccessStandard.Server.Common;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.EFCore1
{
    public static class DapperHelper
    {
        public static ResultSet ExecuteStoredProc(SqlConnection connection, SqlTransaction transaction, MethodInfo mInfo, params object[] arguments)
        {
            var commandInputContext = DBUtility.GetStoredProcedureCommandInputContext(mInfo, arguments);
            var dynamicParameters = GetDynamicParameters(commandInputContext);
            using (var reader = connection.QueryMultiple(sql: commandInputContext.CommandName, param: dynamicParameters, transaction: transaction, commandTimeout: null, commandType: CommandType.StoredProcedure))
            {
                Dictionary<Type, List<object>> recordListDictionaryBasedOnRecordType = new Dictionary<Type, List<object>>();
                foreach (var returnRecordType in commandInputContext.ReturnRecordTypes)
                {
                    var readMethods = typeof(SqlMapper.GridReader).GetMethods().Where(c => c.Name == "Read" && c.IsGenericMethod && c.GetParameters().ToList().All(p => p.ParameterType == typeof(bool))).ToList();
                    var readMethodInfo = readMethods.First().MakeGenericMethod(new Type[] { returnRecordType });
                    var recordListOfReturnRecordType = readMethodInfo.Invoke(reader, new object[] { true });
                    var enumerableObject = recordListOfReturnRecordType as IEnumerable<object>;
                    recordListDictionaryBasedOnRecordType.Add(returnRecordType, enumerableObject != null ? new List<object>(enumerableObject) : null);
                }

                Dictionary<string, object> outputParameterNameValueDictionary = new Dictionary<string, object>();
                if (commandInputContext.OutputArgs.Any())
                {
                    foreach (var dbParameterArgInfo in commandInputContext.OutputArgs)
                    {
                        outputParameterNameValueDictionary.Add(dbParameterArgInfo.ParameterName,
                            dynamicParameters.Get<object>(dbParameterArgInfo.ParameterName));
                    }
                }
                var retVal = new ResultSet(recordListDictionaryBasedOnRecordType, outputParameterNameValueDictionary);
                return retVal;
            }
        }


        private static DynamicParameters GetDynamicParameters(CommandInputContextInfo commandInputContextInfo)
        {
            var dynamicParameters = new DynamicParameters();
            foreach (var dbParameterArgValueInfo in commandInputContextInfo.InputArgumentValue)
            {
                if (dbParameterArgValueInfo.Value is DataTable table)
                {
                    var customQueryParameter = table.AsTableValuedParameter();
                    dynamicParameters.Add(dbParameterArgValueInfo.Key.ParameterName, customQueryParameter, dbParameterArgValueInfo.Key.DbType, dbParameterArgValueInfo.Key.ParameterDirection);
                }
                else
                {
                    dynamicParameters.Add(dbParameterArgValueInfo.Key.ParameterName, dbParameterArgValueInfo.Value,
                        dbParameterArgValueInfo.Key.DbType, dbParameterArgValueInfo.Key.ParameterDirection);
                }
            }

            foreach (var dbParameterArgValueInfo in commandInputContextInfo.OutputArgs)
            {
                dynamicParameters.Add(dbParameterArgValueInfo.ParameterName, dbType: dbParameterArgValueInfo.DbType,
                    size: dbParameterArgValueInfo.Size
                    , direction: dbParameterArgValueInfo.ParameterDirection);
            }

            return dynamicParameters;
        }
    }
}
