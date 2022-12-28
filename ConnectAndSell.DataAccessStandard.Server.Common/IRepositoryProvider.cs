using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Xml;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public interface IRepositoryProvider : IDisposable
    {

        TK ExecuteDynamicStoredProcedureWithSqlXmlReader<TK>(string spName, int timeOutInSeconds, MethodInfo mInfo,
            Func<XmlReader, TK> funcReader, Func<DbCommand, TK> funcCommand, params object[] arguments);

        List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(Expression<Func<TK, bool>> filterExpression,
            Expression<Func<TK, TO>> projectionExpression, bool includeChildRecords,
            int? maxTop ) where TK: class;

        TK ExecuteStoredProcedureWithSQLReader<TK>(int timeOutInSeconds, MethodInfo mInfo, Func<DbDataReader, TK> func,
            params object[] arguments);

        TK ExecuteDynamicStoredProcedureWithSQLReader<TK>(string spName,int timeOutInSeconds,MethodInfo mInfo,
            Func<DbDataReader,TK> funcReader,Func<DbCommand,TK> funcCommand,params object[] arguments);

        Tuple<List<TK>,bool> GetRecordsByExpressionByPage<TK, K>(Expression<Func<TK, bool>> filterExpression,
            Expression<Func<TK, K>> orderColumnFunc, int pageNumber, int pageSize, List<string>? listOfChildTables = null) where TK : class;

        TK GetLastRecord<TK, K>(Expression<Func<TK, bool>> expression,
            Expression<Func<TK, K>> descendingOrderColumnFunc, List<string>? listOfChildTables = null) where TK : class;

        TK? GetFirstRecord<TK, K>(Expression<Func<TK, bool>> expression,
            Expression<Func<TK, K>> ascendingOrderColumnFunc, List<string>? listOfChildTables = null) where TK : class; 
        

        List<TK> GetRecordsByMultipleFieldsUsingXML<TK>(List<FilterFieldInfo<TK>> fieldsInformation) where TK : class;

        List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyIDYear<TK>(List<Tuple<long, int>> list) where TK : class;

        List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyBasicValues<TK, TP>(List<TP> list) where TK : class
            where TP : struct;

        bool IsRecordExists<TK>(Expression<Func<TK, bool>> exp) where TK : class;

        List<T> DoExecuteCommand<T>(Expression<Func<T, bool>> exp, List<string> filteredChildTableNames) where T : class;

        ResultSet ExecuteStoredProcUsingSQLClient(MethodInfo mInfo, params object[] arguments);

        List<T> DoExecuteCommand<T>(Expression<Func<T, bool>> exp, bool getChildRecords) where T: class ;

        ResultSet ExecuteStoredProc(int timeOutInSeconds, MethodInfo mInfo, params object[] arguments);
        
        ResultSet ExecuteStoredProc(MethodInfo mInfo, params object[] arguments);

        bool SaveRecords<T>(List<T> records) where T : IMDRXCoreEntity;

        Dictionary<object, List<ChangeTrackingInfo>> SaveRecordsWithChangeTrackingData<TK>(List<TK> records)
            where TK : IMDRXCoreEntity;

        T DoUnitOfWork<T>(Func<IRepositoryProvider,T> funcUnderUnitOfWork);

        T DoDistributedUnitOfWork<T>(Func<MDRXTransactionToken,IRepositoryProvider, T> funcUnderUnitOfWork);

        List<TK> GetRecordsByTSQLWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK: class;

        List<TK> GetRecordsByTSQLJSONWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class;

        List<TK> GetRecordsByKeyValues<TK, TP>(List<TP> primaryKeyValues);
        
        bool BulkInsert<T>(List<T> records) where T : IMDRXCoreEntity;

        Dictionary<object, List<ChangeTrackingInfo>> BulkInsertWithChangeTracking<TK>(List<TK> records)
            where TK : IMDRXCoreEntity;

        List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(Expression<Func<TK, bool>> exp,
            Expression<Func<TK, TO>> mappingExp, bool includeChildRecords) where TK: class, IMDRXCoreEntity;

        bool DeleteByKeyValues<TK, TP>(List<TP> primaryKeyValues);

        List<TK> GetRecordsByKeyValuesJSON<TK, TP>(List<TP> primaryKeyValues);

        List<TK> GetAssociatedRecordsByTSQLJSONWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class;

        JsonDocument GetJSONData(string commandText, List<DbParameter> parameters);

        TK GetResolvedObject<TS, TK>(IApplicationRepositoryBase repositoryBase, TS sourceObject) where TK : class;

        List<TK> GetResolvedObjectList<TS, TK>(IApplicationRepositoryBase repositoryBase,List<TS> sourceObject) where TK : class;

        Dictionary<TPK, TLk>
            GetPKLookupResolvedDictionaryForSingleType<TK,TPK,TLk>(IApplicationRepositoryBase repoBase, List<TPK> pkObjectList) where TPK :class,IPrimaryKeyBase<TK> where TLk :class ,IPrimaryKeyResolveBase<TPK, TK> ;

        ConcurrentDictionary<Type, ConcurrentDictionary<object, object>> GetPKLookupResolvedDictionary(
            List<object> pkObjectList);


        #region "Selected Columns"

        XmlDocument GetRecordsByTSQLBySelectedColumns<TK>(SelectedColumnBuilder selectedColumns) where TK : class;

        JsonDocument GetAllRecordsByJsonForSelectedColumns<TK>(SelectedColumnBuilder selectedColumns) where TK : class;

        JsonDocument GetRecordsByJsonForSelectedColumns<TK>(SelectedColumnBuilder selectedColumns, String whereClause, List<DbParameter> parameters) where TK : class;

        XmlDocument GetRecordsByTSQLBySelectedColumns<TK>(SelectedColumnBuilder selectedColumns, String whereClause, List<DbParameter> parameters) where TK : class;

        #endregion
    }
}
