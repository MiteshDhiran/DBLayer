

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using System.Xml;
using System.IO;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    /// <summary>
    ///  RepositoryProviderBase Abstract Class
    /// </summary>
    /// <seealso cref="IRepositoryProvider" />
    public abstract class RepositoryProviderBase : IRepositoryProvider
    {
        protected SqlTransaction Transaction { get; set; }

        protected SqlConnection Connection { get; set; }

        protected ORMContext ORMContext { get; }

        protected ORMModelMetaInfo ModelMetaInfo => ORMContext.ORMModelMetaInfo;

        protected TraceInfo CurrentTraceInfo { get; }

        protected bool UsingExistingTransaction { get; }

        protected MDRXTransactionToken GetNewTransactionToken()
        {
            return new MDRXTransactionToken(CurrentTraceInfo,Connection,Transaction);
        }
        
        protected RepositoryProviderBase(RepositoryBaseArg arg)
        {
            CurrentTraceInfo = arg.Trace;
            if (arg.TransactionToken == null)
            {
                Connection = arg.SecuredConnectionString.SqlCredential == null
                    ? new SqlConnection(arg.SecuredConnectionString.ConnectionString)
                    : new SqlConnection(arg.SecuredConnectionString.ConnectionString,
                        arg.SecuredConnectionString.SqlCredential);
                UsingExistingTransaction = false;
            }
            else
            {
                Connection = arg.TransactionToken.Connection;
                Transaction = arg.TransactionToken.Transaction;
                UsingExistingTransaction = true;
            }

            ORMContext = arg.ORMContext;
            if (Connection.State != ConnectionState.Open && Transaction == null)
            {
                arg.SQLRetryAction(() => Connection.Open());
            }
        }

        protected virtual  void OnPreCompile()
        {
            
        }

        /// <summary>
        /// Pre-compile.
        /// </summary>
        public void PreCompile()
        {
            OnPreCompile();
        }
        
        protected void CleanupConnection()
        {
            if (UsingExistingTransaction != false) return;
            try
            {
                Transaction?.Rollback();
            }
            finally
            {
                Transaction = null;
            }
            try
            {
                Connection?.Dispose();
            }
            finally
            {
                Connection = null;
            }
        }

        public abstract TK ExecuteDynamicStoredProcedureWithSqlXmlReader<TK>(string spName, int timeOutInSeconds,
            MethodInfo mInfo,
            Func<XmlReader, TK> funcReader, Func<DbCommand, TK> funcCommand, params object[] arguments);

        public abstract List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(
            Expression<Func<TK, bool>> filterExpression,
            Expression<Func<TK, TO>> projectionExpression, bool includeChildRecords,
            int? maxTop) where TK:class;

        public abstract TK ExecuteStoredProcedureWithSQLReader<TK>(int timeOutInSeconds,MethodInfo mInfo,Func<DbDataReader,TK> func,
            params object[] arguments);

        public abstract TK ExecuteDynamicStoredProcedureWithSQLReader<TK>(string spName,int timeOutInSeconds,MethodInfo mInfo,
            Func<DbDataReader,TK> funcReader,Func<DbCommand,TK> funcCommand,params object[] arguments);

        public abstract Tuple<List<TK>,bool> GetRecordsByExpressionByPage<TK, K>(Expression<Func<TK,bool>> filterExpression,
            Expression<Func<TK,K>> orderColumnFunc,int pageNumber,int pageSize,List<string>? listOfChildTables = null) where TK : class;

        public abstract TK GetLastRecord<TK, MK>(Expression<Func<TK,bool>> expression,
            Expression<Func<TK,MK>> descendingOrderColumnFunc,List<string>? listOfChildTables = null) where TK : class;

        public abstract TK? GetFirstRecord<TK, MK>(Expression<Func<TK, bool>> expression,
            Expression<Func<TK, MK>> ascendingOrderColumnFunc, List<string>? listOfChildTables = null) where TK : class;
         


        public abstract List<TK> GetRecordsByMultipleFieldsUsingXML<TK>(List<FilterFieldInfo<TK>> fieldsInformation) where TK : class;

        public abstract List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyIDYear<TK>(List<Tuple<long, int>> list)
            where TK : class;
        

        public abstract List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyBasicValues<TK, TP>(List<TP> list) where TK : class
            where TP : struct;

        public abstract bool IsRecordExists<TK>(Expression<Func<TK, bool>> exp) where TK : class;

        public abstract List<T> DoExecuteCommand<T>(Expression<Func<T, bool>> exp, List<string> filteredChildTableNames) where T : class;

        public virtual ResultSet ExecuteStoredProcUsingSQLClient(MethodInfo mInfo, params object[] arguments)
        {
            return ExecuteStoredProc(0, mInfo, arguments);
        }

        /// <summary>
        /// Does the execute command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <param name="getChildRecords">if set to <c>true</c> [get child records].</param>
        /// <returns></returns>
        public abstract List<T> DoExecuteCommand<T>(Expression<Func<T, bool>> exp, bool getChildRecords) where T : class;

        /// <summary>
        /// Executes the stored proc.
        /// </summary>
        /// <param name="timeOutInSeconds">The time out in seconds.</param>
        /// <param name="mInfo">The m information.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public virtual ResultSet ExecuteStoredProc(int timeOutInSeconds, MethodInfo mInfo, params object[] arguments)
        {
            return DBUtility.ExecuteStoredProc(Connection,Transaction, timeOutInSeconds, mInfo, arguments);
        }

        /// <summary>
        /// Executes the stored proc.
        /// </summary>
        /// <param name="mInfo">The m information.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public virtual ResultSet ExecuteStoredProc(MethodInfo mInfo, params object[] arguments)
        {
            return ExecuteStoredProc(0, mInfo, arguments);
        }

        /// <summary>
        /// Saves the records.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public abstract bool SaveRecords<T>(List<T> records) where T : IMDRXCoreEntity;

        /// <summary>
        /// Saves the records with change tracking data.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public abstract Dictionary<object, List<ChangeTrackingInfo>> SaveRecordsWithChangeTrackingData<TK>(List<TK> records) where TK : IMDRXCoreEntity;

        protected virtual SqlTransaction GetNewTransaction()
        {
            return Connection.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Does the unit of work.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcUnderUnitOfWork">The function under unit of work.</param>
        /// <returns></returns>
        public T DoUnitOfWork<T>(Func<IRepositoryProvider, T> funcUnderUnitOfWork)
        {
            if (Transaction != null)
            {
                try
                {
                    using (Transaction = GetNewTransaction())
                    {
                        var res = funcUnderUnitOfWork(this);
                        Transaction.Commit();
                        return res;
                    }
                }
                finally
                {
                    Transaction.Dispose();
                    Transaction = null;
                }
            }
            else
            {
                return funcUnderUnitOfWork(this);
            }
        }

        /// <summary>
        /// Does the distributed unit of work.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="funcUnderUnitOfWork">The function under unit of work.</param>
        /// <returns></returns>
        public T DoDistributedUnitOfWork<T>(Func<MDRXTransactionToken, IRepositoryProvider, T> funcUnderUnitOfWork)
        {
            try
            {
                using (this.Transaction = GetNewTransaction())
                {
                    Debug.Assert(Transaction != null);
                    var res = funcUnderUnitOfWork(GetNewTransactionToken(), this);
                    Transaction.Commit();
                    Transaction.Dispose();
                    return res;
                }
            }
            finally
            {
                Transaction = null;
            }
        }

        /// <summary>
        /// Gets the records by TSQL where clause.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="parameterizedWhereClause">The parameterized where clause.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="joinClause">The join clause.</param>
        /// <returns></returns>
        public List<TK> GetRecordsByTSQLWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class
        {
                var tsql = DBXMLUtility.GetTSQLQueryForXmlResultSet(typeof(TK),ModelMetaInfo, parameterizedWhereClause, joinClause);
                return DBUtility.ExecuteForXML<TK>(Connection, Transaction,parameters, tsql);
            
        }

        /// <summary>
        /// Gets the records by tsqljson where clause.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="parameterizedWhereClause">The parameterized where clause.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="joinClause">The join clause.</param>
        /// <returns></returns>
        public List<TK> GetRecordsByTSQLJSONWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK: class
        {
            var tsql = DBXMLUtility.GetTSQLQueryForJSONResultSet(typeof(TK),ModelMetaInfo, parameterizedWhereClause, joinClause);
            return DBUtility.ExecuteForJSON<TK>(Connection, Transaction,
                parameters, tsql);
        }

        /// <summary>
        /// Gets the records by key values.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <typeparam name="TP">The type of the p.</typeparam>
        /// <param name="primaryKeyValues">The primary key values.</param>
        /// <returns></returns>
        public abstract List<TK> GetRecordsByKeyValues<TK, TP>(List<TP> primaryKeyValues);

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public virtual bool BulkInsert<TK>(List<TK> records) where TK: IMDRXCoreEntity 
        {
            var retVal = DBBulkInsertUtility.BulkInsert(CurrentTraceInfo, Connection, Transaction, ModelMetaInfo,false, records);
            return retVal.Item2;
        }

        /// <summary>
        /// Bulks the insert with change tracking.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public virtual Dictionary<object, List<ChangeTrackingInfo>> BulkInsertWithChangeTracking<TK>(List<TK> records) where TK : IMDRXCoreEntity
        {
            var retVal =  DBBulkInsertUtility.BulkInsert(CurrentTraceInfo, Connection, Transaction, ModelMetaInfo,true, records);
            return retVal.Item1;
        }

        /// <summary>
        /// Does the execute command fetch selected columns.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <typeparam name="TO">The type of the o.</typeparam>
        /// <param name="exp">The exp.</param>
        /// <param name="mappingExp">The mapping exp.</param>
        /// <param name="includeChildRecords">if set to <c>true</c> [include child records].</param>
        /// <returns></returns>
        public abstract List<TO> DoExecuteCommandFetchSelectedColumns<TK,TO>(Expression<Func<TK, bool>> exp,
            Expression<Func<TK, TO>> mappingExp, bool includeChildRecords) where TK: class, IMDRXCoreEntity;

        /// <summary>
        /// Deletes the by key values.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <typeparam name="TP">The type of the p.</typeparam>
        /// <param name="primaryKeyValues">The primary key values.</param>
        /// <returns></returns>
        public virtual bool DeleteByKeyValues<TK, TP>(List<TP> primaryKeyValues)
        {
            return DeleteTSQLCommandHelper
                .ExecuteDeleteCommand<TK, TP>(ModelMetaInfo
                    , Connection
                    , Transaction,
                    primaryKeyValues);
        }

        /// <summary>
        /// Gets the records by key values json.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <typeparam name="TP">The type of the p.</typeparam>
        /// <param name="primaryKeyValues">The primary key values.</param>
        /// <returns></returns>
        public List<TK> GetRecordsByKeyValuesJSON<TK, TP>(List<TP> primaryKeyValues)
        {
            var tsqlCommandText = DBXMLUtility.GenerateJSONFilterCriteriaForPKValues<TP>(ModelMetaInfo, typeof(TK), primaryKeyValues);
            return DBUtility.ExecuteForJSON<TK>(Connection, Transaction,
                new List<DbParameter>(), tsqlCommandText);
        }

        /// <summary>
        /// Gets the associated records by tsqljson where clause.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="parameterizedWhereClause">The parameterized where clause.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="joinClause">The join clause.</param>
        /// <returns></returns>
        public virtual List<TK> GetAssociatedRecordsByTSQLJSONWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class
        {
        
            var tsqlCommandText = ModelMetaInfo.GetTSQLQueryForAssociateEntitiesFilteredByWhereClause(typeof(TK),parameterizedWhereClause,joinClause);
            return DBUtility.ExecuteForJSON<TK>(Connection, Transaction,
                parameters, tsqlCommandText);
        }

        /// <summary>
        /// Gets the json data.
        /// </summary>
        /// <param name="commandText">The command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public JsonDocument GetJSONData(string commandText, List<DbParameter> parameters)
        {
            return DBUtility.ExecuteForJSONDocument(Connection, Transaction, parameters, commandText);
        }

        /// <summary>
        /// Gets the resolved object.
        /// </summary>
        /// <typeparam name="TS">The type of the s.</typeparam>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="repositoryBase">The repository base.</param>
        /// <param name="sourceObject">The source object.</param>
        /// <returns></returns>
        public TK GetResolvedObject<TS, TK>(IApplicationRepositoryBase repositoryBase, TS sourceObject) where TK : class
        {
            if (sourceObject == null) return null;
            var retVal = ResolveTypeMapperUtility.GetResolvedObject(repositoryBase,ModelMetaInfo
                                    , sourceObject,
                                    sourceObject.GetType()
                                    , typeof(TK)) as TK;
            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="repositoryBase"></param>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        public List<TK> GetResolvedObjectList<TS, TK>(IApplicationRepositoryBase repositoryBase,List<TS> sourceObject)
            where TK : class
        {
            if (sourceObject == null || (sourceObject.Any() == false)) return default(List<TK>);
            var retVal = ResolveTypeMapperUtility.PopulateResolveObjectList(repositoryBase,ModelMetaInfo
                ,sourceObject.Cast<object>().ToList(),
                typeof(TS)
                ,typeof(TK));
            return retVal?.Cast<TK>()?.ToList();
        }

        /// <summary>
        /// Gets the type of the pk lookup resolved dictionary for single.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <typeparam name="TPK">The type of the pk.</typeparam>
        /// <typeparam name="TLk">The type of the lk.</typeparam>
        /// <param name="repoBase">The repo base.</param>
        /// <param name="pkObjectList">The pk object list.</param>
        /// <returns></returns>
        public Dictionary<TPK, TLk> GetPKLookupResolvedDictionaryForSingleType<TK, TPK, TLk>(
            IApplicationRepositoryBase repoBase, List<TPK> pkObjectList) 
            where TPK : class, IPrimaryKeyBase<TK> 
            where TLk : class, IPrimaryKeyResolveBase<TPK, TK>
        {
            var jsonString = DataContractSerializerHelper.DataContractJSONSerialize(pkObjectList);
            var lookupMainTableType = pkObjectList.First().GetType().GetInterface(typeof(IPrimaryKeyBase<>).FullName).GenericTypeArguments.ToList().First();//table type to which this PK type belongs to
            var lookupMainTableMetaInfoWithType = ModelMetaInfo.TableTypeMetaInfoDic[lookupMainTableType];
            var inputArgName = new ArgumentVariable($"@{lookupMainTableMetaInfoWithType.TableDataContractMetaInfo.TableName}JSONArg");
            var lookupResolveQueryWithoutFilter = lookupMainTableMetaInfoWithType.GetLookupResolveQueryWithoutFilterForType();
            var lookupResolveQueryWithFilter = lookupMainTableMetaInfoWithType.GetSelectQueryBasedOnPrimaryKeyJSONValues(lookupResolveQueryWithoutFilter, inputArgName);
            var doc = GetJSONData(lookupResolveQueryWithFilter, new List<DbParameter>() { new SqlParameter(inputArgName.ArgumentVariableName, SqlDbType.NVarChar, int.MaxValue) { Value = jsonString } });
            var jsonText = doc.RootElement.GetRawText();
            var listOfPrimaryIDLookup = lookupMainTableMetaInfoWithType.DeserializePrimaryIDLookupJSONToListOfPrimaryIDLookupFunc(jsonText) as IList<TLk>;
            if (listOfPrimaryIDLookup != null && listOfPrimaryIDLookup.Any())
            {
                var resolverProvider = new ResolverProvider();
                if (ORMContext.ORMModelMetaInfo.PKLookupClassDependencyDictionaryByTableDataContractMetaInfo
                    .TryGetValue(
                        lookupMainTableMetaInfoWithType,
                        out var resolvePropertyDependenciesOnAnother))
                {
                    foreach (var primaryIDLookupObject in listOfPrimaryIDLookup)
                    {
                        foreach (var resolvePropertyDependencyOnAnotherTableInfoWithType in resolvePropertyDependenciesOnAnother)
                        {
                            var propertiesToBeReplaced = resolvePropertyDependencyOnAnotherTableInfoWithType
                                .ResolvePropertyDependencyOnAnotherTableInfo
                                .ThisTableColumnName
                                .Select(p => typeof(TLk).GetProperty(p)).ToList();
                            object[] propertyValuesToBeReplaced = propertiesToBeReplaced
                                .Select(p => p.GetValue(primaryIDLookupObject)).ToArray(); 
                            resolverProvider.AddPropertiesThatNeedToBeResolved(new PropertiesToBeSetWithResolvedValues(
                                resolvePropertyDependencyOnAnotherTableInfoWithType.PropertyInfoOnLookupResolvedObjectToBeSetWithResolvedValue
                                ,primaryIDLookupObject
                                ,propertyValuesToBeReplaced
                                ,resolvePropertyDependencyOnAnotherTableInfoWithType.AnotherPKTableDataContractMetaInfoWithType
                                ));    
                        }
                    }
                }
                resolverProvider.ResolveAllProviders(repoBase);
            }
            var dic = listOfPrimaryIDLookup?.ToLookup(c => c.PrimaryKeyRecordInfo).ToDictionary(k => k.Key , kv => kv.First() as TLk);
            return dic;
        }

        #region "Selected Columns"

        /// <summary>
        /// Gets the records by TSQL by selected columns.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <returns></returns>
        public XmlDocument GetRecordsByTSQLBySelectedColumns<TK>(SelectedColumnBuilder selectedColumns) where TK : class
        {
            var tsql = DBXMLUtility.GetTSQLSelectedColumnsQueryForXmlResultSet(typeof(TK), ModelMetaInfo, selectedColumns.PropertyInfoList);
            return DBUtility.ExecuteForXmlDocument<TK>(Connection, Transaction, null, tsql);
        }

        /// <summary>
        /// Gets the records by TSQL by selected columns.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public XmlDocument GetRecordsByTSQLBySelectedColumns<TK>(SelectedColumnBuilder selectedColumns, string whereClause, List<DbParameter> parameters) where TK : class
        {
            var commandText = DBXMLUtility.GetTSQLSelectedColumnsQueryForXmlResultSet(typeof(TK), ModelMetaInfo, selectedColumns.PropertyInfoList, whereClause);
            return DBUtility.ExecuteForXmlDocument<TK>(Connection, Transaction, parameters, commandText);
        }

        /// <summary>
        /// Gets all records by json for selected columns.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <returns></returns>
        public JsonDocument GetAllRecordsByJsonForSelectedColumns<TK>(SelectedColumnBuilder selectedColumns) where TK : class
        {
            string commandText = DBXMLUtility.GetTSQLSelectedColumnsQueryForJsonResultSet(typeof(TK), ModelMetaInfo, selectedColumns.PropertyInfoList);
            return DBUtility.ExecuteForJSONDocument(Connection, Transaction, null, commandText);
        }

        /// <summary>
        /// Gets the records by json for selected columns.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public JsonDocument GetRecordsByJsonForSelectedColumns<TK>(SelectedColumnBuilder selectedColumns, string whereClause, List<DbParameter> parameters) where TK : class
        {
            string commandText = DBXMLUtility.GetTSQLSelectedColumnsQueryForJsonResultSet(typeof(TK), ModelMetaInfo, selectedColumns.PropertyInfoList, whereClause);
            return DBUtility.ExecuteForJSONDocument(Connection, Transaction, parameters, commandText);
        }

        #endregion        
        /// <summary>
        /// Gets the pk lookup resolved dictionary.
        /// </summary>
        /// <param name="pkObjectList">The pk object list.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ConcurrentDictionary<Type, ConcurrentDictionary<object, object>> GetPKLookupResolvedDictionary(List<object> pkObjectList)
        {
            throw new NotImplementedException();
        }

        protected virtual void CleanupLocalContext()
        {
            
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            CleanupConnection();
            CleanupLocalContext();
        }
    }
}
/* P r o p r i e t a r y  N o t i c e */
/*
Confidential and proprietary information of Allscripts Healthcare, LLC and/or its affiliates. Authorized users only.
Notice to U.S. Government Users: This software is "Commercial Computer Software." Subject to full notice set
forth herein.
*/
/* P r o p r i e t a r y  N o t i c e */