using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using ConnectAndSell.DataAccessStandard.Server.Common.Polly;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public interface IApplicationRepositoryBase
    {

        TK ExecuteDynamicStoredProcedureWithSqlXmlReader<TK>(string spName, int timeOutInSeconds, MethodInfo mInfo,
            Func<XmlReader, TK> funcReader, Func<DbCommand, TK> funcCommand, params object[] arguments);

        List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(Expression<Func<TK, bool>> filterExpression,
            Expression<Func<TK, TO>> projectionExpression, bool includeChildRecords,
            int? maxTop ) where TK : class;

        TK ExecuteStoredProcedureWithSQLReader<TK>(int timeOutInSeconds,MethodInfo mInfo,Func<DbDataReader,TK> func,
            params object[] arguments);

        TK ExecuteDynamicStoredProcedureWithSQLReader<TK>(string SPName, int timeOutInSeconds, MethodInfo mInfo,
            Func<DbDataReader, TK> funcReader, Func<DbCommand, TK> funcCommand, params object[] arguments);

        Tuple<List<TK>, bool> GetRecordsByExpressionByPage<TK, K>(Expression<Func<TK, bool>> expression,
            Expression<Func<TK, K>> orderColumnFunc, int pageNumber, int pageSize,
            List<string>? listOfChildTables = null) where TK : class;

        TK GetLastRecord<TK, K>(Expression<Func<TK,bool>> expression,
            Expression<Func<TK,K>> descendingOrderColumnFunc,List<string>? listOfChildTables = null) where TK : class;

        TK? GetFirstRecord<TK, K>(Expression<Func<TK,bool>> expression,
            Expression<Func<TK,K>> ascendingOrderColumnFunc,List<string>? listOfChildTables = null) where TK : class;


        List<TK> GetRecordsByMultipleFieldsUsingXML<TK>(List<FilterFieldInfo<TK>> fieldsInformation) where TK : class;

        List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyIDYear<TK>(List<Tuple<long, int>> list) where TK : class;
        
        List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyBasicValues<TK, TP>(List<TP> list) where TK : class
            where TP : struct;

        bool IsRecordExists<TK>(Expression<Func<TK, bool>> exp) where TK : class;

        List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(Expression<Func<TK, bool>> exp,
            Expression<Func<TK, TO>> mappingExp, bool includeChildRecords) where TK : class, ICoreEntity;

        Task<List<TO>> DoExecuteCommandFetchSelectedColumnsAsync<TK, TO>(Expression<Func<TK, bool>> exp,
            Expression<Func<TK, TO>> mappingExp, bool includeChildRecords) where TK : class, ICoreEntity;

        List<TK> DoExecuteCommand<TK>(Expression<Func<TK, bool>> exp, bool getChildRecords) where TK : class;

        List<TK> DoExecuteCommand<TK>(Expression<Func<TK, bool>> exp, List<string> childTableNames) where TK : class;

        Task<List<TK>> DoExecuteCommandAsync<TK>(Expression<Func<TK, bool>> exp, bool getChildRecords)
            where TK : class;

        Task<List<TK>> DoExecuteCommandAsync<TK>(Expression<Func<TK, bool>> exp, List<string> childTableNames)
            where TK : class;

        ResultSet ExecuteStoredProcUsingSQLClient(MethodInfo mInfo, params object[] arguments);

        ResultSet ExecuteStoredProc(MethodInfo mInfo, params object[] arguments);
        Task<ResultSet> ExecuteStoredProcAsync(MethodInfo mInfo, params object[] arguments);
        bool BulkInsert<TK>(List<TK> records) where TK : ICoreEntity;
        Task<bool> BulkInsertAsync<TK>(List<TK> records) where TK : ICoreEntity;

        Dictionary<object, List<ChangeTrackingInfo>> BulkInsertWithChangeTrackingData<TK>(
            List<TK> records) where TK : ICoreEntity;

        Task<Dictionary<object, List<ChangeTrackingInfo>>>
            BulkInsertWithChangeTrackingDataAsync<TK>(List<TK> records) where TK : ICoreEntity;

        bool SaveRecords<TK>(List<TK> records) where TK : ICoreEntity;
        Task<bool> SaveRecordsAsync<TK>(List<TK> records) where TK : ICoreEntity;

        Dictionary<object, List<ChangeTrackingInfo>>
            SaveRecordsWithChangeTrackingData<TK>(List<TK> records) where TK : ICoreEntity;

        Task<Dictionary<object, List<ChangeTrackingInfo>>>
            SaveRecordsWithChangeTrackingDataAsync<TK>(List<TK> records) where TK : ICoreEntity;

        TK DoUnitOfWork<TK>(Func<TK> funcUnderUnitOfWork);
        Task<TK> DoUnitOfWorkAsync<TK>(Func<TK> funcUnderUnitOfWork);

        List<TK> GetRecordsByTSQLWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class;

        Task<List<TK>> GetRecordsByTSQLWhereClauseAsync<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class;

        List<TK> GetRecordsByTSQLJSONWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class;

        Task<List<TK>> GetRecordsByTSQLJSONWhereClauseAsync<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class;

        List<TK> GetRecordsByKeyValues<TK, TP>(List<TP> primaryKeyValues);
        Task<List<TK>> GetRecordsByKeyValuesAsync<TK, TP>(List<TP> primaryKeyValues);
        bool DeleteRecordsByPKValues<TK, TP>(List<TP> primaryKeyValues);
        Task<bool> DeleteRecordsByPKValuesAsync<TK, TP>(List<TP> primaryKeyValues);
        List<TK> GetRecordsByKeyValuesJSON<TK, TP>(List<TP> primaryKeyValues);
        Task<List<TK>> GetRecordsByKeyValuesJSONAsync<TK, TP>(List<TP> primaryKeyValues);

        List<TK> GetAssociatedRecordsByTSQLJSONWhereClause<TK>(string parameterizedWhereClause, List<DbParameter> parameters,
            string joinClause = "") where TK : class;

        Task<List<TK>> GetAssociatedRecordsByTSQLJSONWhereClauseAsync<TK>(string parameterizedWhereClause, List<DbParameter> parameters,
            string joinClause = "") where TK : class;

        TK GetResolvedObject<TS, TK>(TS sourceObject) where TK : class;
        Task<TK> GetResolvedObjectAsync<TS, TK>(TS sourceObject) where TK : class;
        List<TK> GetResolvedObject<TS, TK>(List<TS> sourceObject) where TK : class;
        Task<JsonDocument> GetJsonDocumentAsync(string commandText, List<DbParameter> parameters);
        JsonDocument GetJsonDocument(string commandText, List<DbParameter> parameters);
        Dictionary<TPK, TLk> GetPKLookupResolvedDictionaryForSingleType<TK,TPK,TLk>(List<TPK> pkObjectList) where TPK :class,IPrimaryKeyBase<TK> where TLk :class ,IPrimaryKeyResolveBase<TPK, TK>;

        #region "Selected Columns"

        //XmlDocument DoExecuteCommandFetchSelectedColumnsByTSQL<TK>(SelectedColumnBuilder selectedColumns) where TK : class;

        //JsonDocument DoExecuteCommandFetchSelectedColumnsByJSON<TK>(SelectedColumnBuilder selectedColumns) where TK : class;

        JsonDocument DoExecuteCommandFetchSelectedColumnsByWhereClause<TK>(SelectedColumnBuilder selectedColumns, string whereClause, List<DbParameter> parameters) where TK : class;

        //XmlDocument DoExecuteCommandFetchSelectedColumnsByTSQL<TK>(SelectedColumnBuilder selectedColumns, String whereClause, List<DbParameter> parameters) where TK : class;

        #endregion
    }

    public abstract class ApplicationRepositoryBase : IApplicationRepositoryBase
    {
        [ThreadStatic] private static IApplicationRepositoryBase CurrentApplicationRepositoryThatStartedUOW;

        [ThreadStatic]
        private static ConcurrentDictionary<IApplicationRepositoryBase, IRepositoryProvider>
            CurrentRepositoryDictionary;

        [ThreadStatic] private static EntityTransactionToken CurrentTransactionToken;
        
        private readonly Action<Action> SQLRetryAction;

        private SecureConnectionString SecuredConnectionString { get; }

        private Func<string> GetCurrentUserFunc { get; }

        private readonly ORMContext _ormContext;

        private readonly ConstructorInfo _constructorInfo;
        
        private Type ProviderType { get; }

        private readonly Func<object> CompiledModelFunc;

        protected ApplicationRepositoryBase(Func<string> getCurrentUserFunc,
            SecureConnectionString securedConnectionString
            , ORMContext ormContext
            , Type providerType
            ,Func<object> compiledModelFunc
            )
        {
            GetCurrentUserFunc = getCurrentUserFunc;
            SecuredConnectionString = securedConnectionString;
            _ormContext = ormContext;
            ProviderType = providerType;
            _constructorInfo = ProviderType.GetConstructor(new[] {typeof(RepositoryBaseArg)});
            CompiledModelFunc = compiledModelFunc;
            SQLRetryAction = (action) =>
            {
                var retryPolicy = new SqlPolicyBuilder().UseSyncExecutor().WithDefaultPolicies().Build();
                retryPolicy.Execute(action);
            };

        }

        protected TraceInfo GetNewTraceInfo()
        {
            return new TraceInfo(this.GetCurrentUserFunc(), DateTimeOffset.UtcNow);
        }

        protected TraceInfo ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo()
        {
            if (CurrentTransactionToken != null)
            {
                throw new InvalidOperationException(
                    $"Cannot perform async operation within unit of work operation. There is already UOW started - Reference TraceID:{CurrentTransactionToken.PreviousTraceInfo.TraceID} started at: {CurrentTransactionToken.PreviousTraceInfo.CurrentTime} Trace ThreadID: {CurrentTransactionToken.PreviousTraceInfo.TraceID}");
            }

            return new TraceInfo(this.GetCurrentUserFunc(), DateTimeOffset.UtcNow);
        }

        /// <summary>
        /// Get the tuple - repositoryProvider and whether it can be disposed immediately after the operation
        /// </summary>
        /// <returns></returns>
        protected RepositoryWithDisposableFlag GetRepository(TraceInfo traceInfo, bool getDisposable = true)
        {
            if (CurrentRepositoryDictionary != null && CurrentRepositoryDictionary.ContainsKey(this))
            {
                return new RepositoryWithDisposableFlag(CurrentRepositoryDictionary[this], false);
            }

            var repoArg = CurrentRepositoryDictionary != null && CurrentTransactionToken != null
                ? new RepositoryBaseArg(CurrentTransactionToken.PreviousTraceInfo, _ormContext, (s) => WrapActionWithRetry(s)(),
                    CurrentTransactionToken,CompiledModelFunc)
                : new RepositoryBaseArg(traceInfo, _ormContext, (s) => WrapActionWithRetry(s)(), SecuredConnectionString,CompiledModelFunc);
            var repo = _constructorInfo.Invoke(new object[]{repoArg}) as IRepositoryProvider;
            if (CurrentRepositoryDictionary != null && CurrentTransactionToken != null)
            {
                CurrentRepositoryDictionary.TryAdd(this, repo ?? throw new InvalidOperationException());
                return new RepositoryWithDisposableFlag(repo, false);
            }
            return new RepositoryWithDisposableFlag(repo ?? throw new InvalidOperationException(), getDisposable);
        }

        public TK ExecuteDynamicStoredProcedureWithSqlXmlReader<TK>(string spName, int timeOutInSeconds,
            MethodInfo mInfo,
            Func<XmlReader, TK> funcReader, Func<DbCommand, TK> funcCommand, params object[] arguments)
            => ExecuteDynamicStoredProcedureWithSqlXmlReaderInternal<TK>(GetNewTraceInfo(), spName, timeOutInSeconds,
                mInfo, funcReader, funcCommand, arguments);

        private TK ExecuteDynamicStoredProcedureWithSqlXmlReaderInternal<TK>(TraceInfo traceInfo, string spName, int timeOutInSeconds,
            MethodInfo mInfo,
            Func<XmlReader, TK> funcReader, Func<DbCommand, TK> funcCommand, params object[] arguments)
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.ExecuteDynamicStoredProcedureWithSqlXmlReader<TK>(spName,timeOutInSeconds,mInfo,funcReader,funcCommand,arguments);
                }
            });
        }

        public List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(Expression<Func<TK, bool>> filterExpression,
            Expression<Func<TK, TO>> projectionExpression, bool includeChildRecords,
            int? maxTop ) where TK:class
            => DoExecuteCommandFetchSelectedColumnsInternal<TK, TO>(GetNewTraceInfo(), filterExpression,
                projectionExpression, includeChildRecords, maxTop);

        private List<TO> DoExecuteCommandFetchSelectedColumnsInternal<TK, TO>(TraceInfo traceInfo,
            Expression<Func<TK, bool>> filterExpression,
            Expression<Func<TK, TO>> projectionExpression, bool includeChildRecords,
            int? maxTop ) where TK: class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.DoExecuteCommandFetchSelectedColumns<TK,TO>(filterExpression,projectionExpression,includeChildRecords,maxTop);
                }
            });
        }

        public TK ExecuteStoredProcedureWithSQLReader<TK>(int timeOutInSeconds, MethodInfo mInfo,
            Func<DbDataReader, TK> func,
            params object[] arguments)
            => ExecuteStoredProcedureWithSQLReaderInternal<TK>(GetNewTraceInfo(), timeOutInSeconds, mInfo, func,
                arguments);

        private TK ExecuteStoredProcedureWithSQLReaderInternal<TK>(TraceInfo traceInfo, int timeOutInSeconds, MethodInfo mInfo,
            Func<DbDataReader, TK> func,
            params object[] arguments)
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.ExecuteStoredProcedureWithSQLReader<TK>(timeOutInSeconds,mInfo,func,arguments);
                }
            });
        }

        public TK ExecuteDynamicStoredProcedureWithSQLReader<TK>(string spName,int timeOutInSeconds,MethodInfo mInfo,
            Func<DbDataReader,TK> funcReader,Func<DbCommand,TK> funcCommand,params object[] arguments)
        =>    ExecuteDynamicStoredProcedureWithSqlReaderInternal(GetNewTraceInfo(), spName, timeOutInSeconds, mInfo,
                funcReader, funcCommand, arguments);
        

        private TK ExecuteDynamicStoredProcedureWithSqlReaderInternal<TK>(TraceInfo traceInfo,string spName,int timeOutInSeconds,MethodInfo mInfo,
            Func<DbDataReader,TK> funcReader,Func<DbCommand,TK> funcCommand,params object[] arguments)
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.ExecuteDynamicStoredProcedureWithSQLReader<TK>(spName,timeOutInSeconds,mInfo,funcReader,funcCommand,arguments);
                }
            });
        }

        public Tuple<List<TK>, bool> GetRecordsByExpressionByPage<TK, K>(Expression<Func<TK, bool>> expression,
            Expression<Func<TK, K>> orderColumnFunc, int pageNumber, int pageSize,
            List<string>? listOfChildTables = null) where TK : class
            => GetRecordsByExpressionByPageInternal<TK, K>(GetNewTraceInfo(), expression, orderColumnFunc, pageNumber,
                pageSize, listOfChildTables);


        private Tuple<List<TK>,bool> GetRecordsByExpressionByPageInternal<TK, K>(TraceInfo traceInfo,Expression<Func<TK,bool>> expression,
            Expression<Func<TK,K>> orderColumnFunc,int pageNumber,int pageSize,List<string>? listOfChildTables = null) where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetRecordsByExpressionByPage<TK,K>(expression,orderColumnFunc,pageNumber,pageSize,listOfChildTables);
                }
            });
        }

        public TK? GetFirstRecord<TK, K>(Expression<Func<TK,bool>> expression,Expression<Func<TK,K>> ascendingOrderColumnFunc,List<string>? listOfChildTables = null) where TK:class
        => GetFirstRecordInternal<TK, K>(GetNewTraceInfo(), expression, ascendingOrderColumnFunc,listOfChildTables);
        

        public TK GetLastRecord<TK, K>(Expression<Func<TK,bool>> expression,Expression<Func<TK,K>> descendingOrderColumnFunc,List<string>? listOfChildTables = null) where TK:class
        => GetLastRecordInternal<TK,K>(GetNewTraceInfo(),expression,descendingOrderColumnFunc,listOfChildTables);
        

        private TK? GetFirstRecordInternal<TK, K>(TraceInfo traceInfo,Expression<Func<TK, bool>> expression,
            Expression<Func<TK, K>> ascendingOrderColumnFunc, List<string>? listOfChildTables = null) where TK: class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetFirstRecord<TK,K>(expression,ascendingOrderColumnFunc,listOfChildTables);
                }
            });
        }

        private TK GetLastRecordInternal<TK, K>(TraceInfo traceInfo,Expression<Func<TK,bool>> expression,Expression<Func<TK,K>> descendingOrderColumnFunc,List<string>? listOfChildTables = null) where TK: class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetLastRecord<TK,K>(expression,descendingOrderColumnFunc,listOfChildTables);
                }
            });
        }
        public List<TK> GetRecordsByMultipleFieldsUsingXML<TK>(List<FilterFieldInfo<TK>> fieldsInformation) where TK : class
        => GetRecordsByMultipleFieldsUsingXmlInternal<TK>(GetNewTraceInfo(), fieldsInformation);

        
        public List<TK> GetRecordsByMultipleFieldsUsingXmlInternal<TK>(TraceInfo traceInfo,List<FilterFieldInfo<TK>> fieldsInformation) where TK: class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetRecordsByMultipleFieldsUsingXML<TK>(fieldsInformation);
                }
            });
        }

        public List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(Expression<Func<TK, bool>> exp,
            Expression<Func<TK, TO>> mappingExp, bool includeChildRecords) where TK : class, ICoreEntity
            => DoExecuteCommandFetchSelectedColumnsInternal(GetNewTraceInfo(), exp, mappingExp, includeChildRecords);

        public async Task<List<TO>> DoExecuteCommandFetchSelectedColumnsAsync<TK, TO>(Expression<Func<TK, bool>> exp,
            Expression<Func<TK, TO>> mappingExp, bool includeChildRecords) where TK : class, ICoreEntity
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() =>
                DoExecuteCommandFetchSelectedColumnsInternal(traceInfo, exp, mappingExp, includeChildRecords));
        }

        public List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyIDYear<TK>(List<Tuple<long, int>> list) where TK : class
            => GetRecordsUsingXMLBasedOnPrimaryKeyIDYearInternal<TK>(GetNewTraceInfo(), list);
        

        private List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyIDYearInternal<TK>(TraceInfo traceInfo,List<Tuple<long, int>> list) where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetRecordsUsingXMLBasedOnPrimaryKeyIDYear<TK>(list);
                }
            });
        }

        

        private static Action WrapActionWithRetry(Action executeActionWithRetry) =>
            CurrentTransactionToken == null 
                ? () =>
                {
                    var retryPolicy = new SqlPolicyBuilder().UseSyncExecutor().WithDefaultPolicies().Build();
                    retryPolicy.Execute(executeActionWithRetry);
                }
                : executeActionWithRetry;
        

        private static Func<TR> WrapFuncWithRetry<TR>(Func<TR> executeFunWithRetry) =>
            CurrentTransactionToken == null 
                ? () =>
                {
                    var retryPolicy = new SqlPolicyBuilder().UseSyncExecutor().WithDefaultPolicies().Build();
                    return retryPolicy.Execute(executeFunWithRetry);
                }
                : executeFunWithRetry;

        private static TR ExecuteFuncWithRetry<TR>(Func<TR> executeFunWithRetry) => 
            CurrentTransactionToken != null ? executeFunWithRetry() : WrapFuncWithRetry(executeFunWithRetry)();

        private static TR ExecuteInTransactionWithRetry<TR>(Func<TR> executeUOWWithRetry)
        {
            var transactionRetryPolicy = new SqlPolicyBuilder().UseSyncExecutor()
                               .WithOverallTimeout(TimeSpan.FromSeconds(30)).WithTransaction().WithCircuitBreaker()
                               .Build();

            return transactionRetryPolicy.Execute(executeUOWWithRetry);

        }
        
        private List<TO> DoExecuteCommandFetchSelectedColumnsInternal<TK, TO>(TraceInfo traceInfo,
            Expression<Func<TK, bool>> exp, Expression<Func<TK, TO>> mappingExp, bool includeChildRecords)
            where TK : class, ICoreEntity
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.DoExecuteCommandFetchSelectedColumns<TK, TO>(exp, mappingExp,
                        includeChildRecords);
                }
            });
        }

        public List<TK> DoExecuteCommand<TK>(Expression<Func<TK, bool>> exp, bool getChildRecords) where TK : class =>
            DoExecuteCommandInternal<TK>(GetNewTraceInfo(), exp, getChildRecords);


        public async Task<List<TK>> DoExecuteCommandAsync<TK>(Expression<Func<TK, bool>> exp, bool getChildRecords)
            where TK : class
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => DoExecuteCommandInternal<TK>(traceInfo, exp, getChildRecords));
        }

        
        public List<TK> DoExecuteCommand<TK>(Expression<Func<TK, bool>> exp, List<string> childTableNames) where TK : class =>
            DoExecuteCommandInternal<TK>(GetNewTraceInfo(), exp, childTableNames);


        public async Task<List<TK>> DoExecuteCommandAsync<TK>(Expression<Func<TK, bool>> exp, List<string> childTableNames)
            where TK : class
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => DoExecuteCommandInternal<TK>(traceInfo, exp, childTableNames));
        }

        
        private List<TK> DoExecuteCommandInternal<TK>(TraceInfo traceInfo, Expression<Func<TK, bool>> exp,
            bool getChildRecords) where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repositoryWithDisposableFlag = GetRepository(traceInfo))
                {
                    return repositoryWithDisposableFlag.RepositoryProvider.DoExecuteCommand<TK>(exp, getChildRecords);
                }
            });
        }

        private List<TK> DoExecuteCommandInternal<TK>(TraceInfo traceInfo, Expression<Func<TK, bool>> exp,
            List<string> childTableNames) where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repositoryWithDisposableFlag = GetRepository(traceInfo))
                {
                    return repositoryWithDisposableFlag.RepositoryProvider.DoExecuteCommand<TK>(exp, childTableNames);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mInfo"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public ResultSet ExecuteStoredProcUsingSQLClient(MethodInfo mInfo, params object[] arguments)
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repositoryWithDisposableFlag = GetRepository(GetNewTraceInfo()))
                {
                    return repositoryWithDisposableFlag.RepositoryProvider.ExecuteStoredProcUsingSQLClient(mInfo, arguments);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyBasicValues<TK, TP>(List<TP> list) where TK : class
            where TP : struct
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(GetNewTraceInfo()))
                {
                    return repo.RepositoryProvider.GetRecordsUsingXMLBasedOnPrimaryKeyBasicValues<TK,TP>(list);
                }
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool IsRecordExists<TK>(Expression<Func<TK, bool>> exp) where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(GetNewTraceInfo()))
                {
                    return repo.RepositoryProvider.IsRecordExists<TK>(exp);
                }
            });
        }

        public ResultSet ExecuteStoredProc(int timeOutInSeconds, MethodInfo mInfo, params object[] arguments)
            => GetAppExecuteStoredProcedure(GetNewTraceInfo(), mInfo, arguments);

        public ResultSet ExecuteStoredProc(MethodInfo mInfo, params object[] arguments)
            => GetAppExecuteStoredProcedure(GetNewTraceInfo(), mInfo, arguments);

        public async Task<ResultSet> ExecuteStoredProcAsync(MethodInfo mInfo, params object[] arguments)
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => GetAppExecuteStoredProcedure(traceInfo, mInfo, arguments));
        }

        private ResultSet GetAppExecuteStoredProcedure(TraceInfo traceInfo, MethodInfo mInfo, object[] paramsObjects)
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repositoryWithDisposableFlag = GetRepository(traceInfo))
                {
                    return repositoryWithDisposableFlag.RepositoryProvider.ExecuteStoredProc(mInfo, paramsObjects);
                }
            });
        }

        public bool BulkInsert<TK>(List<TK> records) where TK : ICoreEntity =>
            BulkInsertInternal(GetNewTraceInfo(), records);


        public async Task<bool> BulkInsertAsync<TK>(List<TK> records) where TK : ICoreEntity
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => BulkInsertInternal(traceInfo, records));
        }

        private bool BulkInsertInternal<TK>(TraceInfo traceInfo, List<TK> records) where TK : ICoreEntity =>
            DoUnitOfWork(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.BulkInsert<TK>(records);
                }
            });


        public Dictionary<object, List<ChangeTrackingInfo>> BulkInsertWithChangeTrackingData<TK>(
            List<TK> records) where TK : ICoreEntity =>
            BulkInsertWithChangeTrackingDataInternal(GetNewTraceInfo(), records);

        public async Task<Dictionary<object, List<ChangeTrackingInfo>>>
            BulkInsertWithChangeTrackingDataAsync<TK>(List<TK> records) where TK : ICoreEntity
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => BulkInsertWithChangeTrackingDataInternal(traceInfo, records));
        }

        private Dictionary<object, List<ChangeTrackingInfo>> BulkInsertWithChangeTrackingDataInternal<TK>(
            TraceInfo traceInfo, List<TK> records)
            where TK : ICoreEntity =>
            DoUnitOfWork(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.BulkInsertWithChangeTracking<TK>(records);
                }
            });


        public bool SaveRecords<TK>(List<TK> records) where TK : ICoreEntity =>
            SaveRecordsInternal(GetNewTraceInfo(), records);

        public async Task<bool> SaveRecordsAsync<TK>(List<TK> records) where TK : ICoreEntity
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => SaveRecordsInternal(traceInfo, records));
        }

        private bool SaveRecordsInternal<TK>(TraceInfo traceInfo, List<TK> records) where TK : ICoreEntity =>
            this.DoUnitOfWork(() =>
                {
                    using (var repo = GetRepository(traceInfo))
                    {
                        return repo.RepositoryProvider.SaveRecords(records);
                    }
                }
            );


        public Dictionary<object, List<ChangeTrackingInfo>>
            SaveRecordsWithChangeTrackingData<TK>(List<TK> records) where TK : ICoreEntity =>
            SaveRecordsWithChangeTrackingDataInternal(GetNewTraceInfo(), records);

        public async Task<Dictionary<object, List<ChangeTrackingInfo>>>
            SaveRecordsWithChangeTrackingDataAsync<TK>(List<TK> records) where TK : ICoreEntity
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => SaveRecordsWithChangeTrackingDataInternal(traceInfo, records));
            ;
        }

        private Dictionary<object, List<ChangeTrackingInfo>>
            SaveRecordsWithChangeTrackingDataInternal<TK>(TraceInfo traceInfo, List<TK> records)
            where TK : ICoreEntity =>
            this.DoUnitOfWork(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.SaveRecordsWithChangeTrackingData(records);
                }
            });


        public TK DoUnitOfWork<TK>(Func<TK> funcUnderUnitOfWork) =>
            DoUnitOfWorkInternal(GetNewTraceInfo(), funcUnderUnitOfWork);

        public async Task<TK> DoUnitOfWorkAsync<TK>(Func<TK> funcUnderUnitOfWork)
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => DoUnitOfWorkInternal(traceInfo, funcUnderUnitOfWork));
        }


        private TK DoUnitOfWorkInternal<TK>(TraceInfo traceInfo, Func<TK> funcUnderUnitOfWork)
        {
            if (CurrentTransactionToken != null)
            {
                return funcUnderUnitOfWork();
            }
            else
            {
                TK retVal;
                RepositoryWithDisposableFlag repositoryWithDisposableFlag = null;
                try
                {
                    repositoryWithDisposableFlag = GetRepository(traceInfo, false);
                    {
                        retVal = repositoryWithDisposableFlag.RepositoryProvider.DoDistributedUnitOfWork((token, rep) =>
                        {
                            CurrentTransactionToken = token;
                            CurrentRepositoryDictionary =
                                new ConcurrentDictionary<IApplicationRepositoryBase, IRepositoryProvider>();
                            CurrentRepositoryDictionary.TryAdd(this, rep);
                            CurrentApplicationRepositoryThatStartedUOW = this;
                            return funcUnderUnitOfWork();
                        });
                    }
                }
                finally
                {
                    CurrentApplicationRepositoryThatStartedUOW = null;
                    CurrentTransactionToken = null;
                    var repositories = CurrentRepositoryDictionary?.Select(kv => kv.Value).ToList();
                    CurrentRepositoryDictionary?.Clear();
                    CurrentRepositoryDictionary = null;
                    repositories?.ForEach(r => r?.Dispose());
                    repositoryWithDisposableFlag?.DisposeExplicitly();
                }

                return retVal;
                
            }
        }


        public List<TK> GetRecordsByTSQLWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class
            => GetRecordsByTSQLWhereClauseInternal<TK>(GetNewTraceInfo(), parameterizedWhereClause, parameters,
                joinClause);

        public Task<List<TK>> GetRecordsByTSQLWhereClauseAsync<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return Task.Run(() =>
                GetRecordsByTSQLWhereClauseInternal<TK>(traceInfo, parameterizedWhereClause, parameters, joinClause));
        }

        private List<TK> GetRecordsByTSQLWhereClauseInternal<TK>(TraceInfo traceInfo, string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetRecordsByTSQLWhereClause<TK>(parameterizedWhereClause, parameters,
                        joinClause);
                }
            });
        }

        public List<TK> GetRecordsByTSQLJSONWhereClause<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class
            => GetRecordsByTSQLJSONWhereClauseInternal<TK>(GetNewTraceInfo(), parameterizedWhereClause, parameters,
                joinClause);

        public async Task<List<TK>> GetRecordsByTSQLJSONWhereClauseAsync<TK>(string parameterizedWhereClause,
            List<DbParameter> parameters, string joinClause = "") where TK : class
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() =>
                GetRecordsByTSQLJSONWhereClauseInternal<TK>(traceInfo, parameterizedWhereClause, parameters, joinClause)
            );
        }

        private List<TK> GetRecordsByTSQLJSONWhereClauseInternal<TK>(TraceInfo traceInfo,
            string parameterizedWhereClause, List<DbParameter> parameters, string joinClause = "") where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetRecordsByTSQLJSONWhereClause<TK>(parameterizedWhereClause, parameters,
                        joinClause);
                }
            });
        }

        public List<TK> GetRecordsByKeyValues<TK, TP>(List<TP> primaryKeyValues)
            => GetRecordsByKeyValuesInternal<TK, TP>(GetNewTraceInfo(), primaryKeyValues);

        public async Task<List<TK>> GetRecordsByKeyValuesAsync<TK, TP>(List<TP> primaryKeyValues)
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => GetRecordsByKeyValuesInternal<TK, TP>(traceInfo, primaryKeyValues));
        }

        private List<TK> GetRecordsByKeyValuesInternal<TK, TP>(TraceInfo traceInfo, List<TP> primaryKeyValues)
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetRecordsByKeyValues<TK, TP>(primaryKeyValues);
                }
            });
        }


        public bool DeleteRecordsByPKValues<TK, TP>(List<TP> primaryKeyValues)
            => DeleteRecordsByPKValues<TK, TP>(GetNewTraceInfo(), primaryKeyValues);

        public async Task<bool> DeleteRecordsByPKValuesAsync<TK, TP>(List<TP> primaryKeyValues)
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => DeleteRecordsByPKValues<TK, TP>(traceInfo, primaryKeyValues));
        }

        private bool DeleteRecordsByPKValues<TK, TP>(TraceInfo traceInfo, List<TP> primaryKeyValues)
        {
            return this.DoUnitOfWork(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.DeleteByKeyValues<TK, TP>(primaryKeyValues);
                }
            });
        }

        public List<TK> GetRecordsByKeyValuesJSON<TK, TP>(List<TP> primaryKeyValues)
            => GetRecordsByKeyValuesJSONInternal<TK, TP>(GetNewTraceInfo(), primaryKeyValues);

        public async Task<List<TK>> GetRecordsByKeyValuesJSONAsync<TK, TP>(List<TP> primaryKeyValues)
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => GetRecordsByKeyValuesJSONInternal<TK, TP>(traceInfo, primaryKeyValues));
        }

        private List<TK> GetRecordsByKeyValuesJSONInternal<TK, TP>(TraceInfo traceInfo, List<TP> primaryKeyValues)
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetRecordsByKeyValuesJSON<TK, TP>(primaryKeyValues);
                }
            });
        }

        public List<TK> GetAssociatedRecordsByTSQLJSONWhereClause<TK>(string parameterizedWhereClause, List<DbParameter> parameters,
            string joinClause = "") where TK : class => GetAssociatedRecordsByTSQLJSONWhereClauseInternal<TK>(GetNewTraceInfo(),
            parameterizedWhereClause, parameters, joinClause);

        public async Task<List<TK>> GetAssociatedRecordsByTSQLJSONWhereClauseAsync<TK>(string parameterizedWhereClause, List<DbParameter> parameters,
            string joinClause = "") where TK : class
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => GetAssociatedRecordsByTSQLJSONWhereClauseInternal<TK>(traceInfo,
                parameterizedWhereClause, parameters, joinClause));
        }
        
        private List<TK> GetAssociatedRecordsByTSQLJSONWhereClauseInternal<TK>(TraceInfo traceInfo,string parameterizedWhereClause, List<DbParameter> parameters,
            string joinClause = "") where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetAssociatedRecordsByTSQLJSONWhereClause<TK>(parameterizedWhereClause,
                        parameters, joinClause);
                }
            });
            
        }

        public TK GetResolvedObject<TS, TK>(TS sourceObject) where TK : class
        {
            return GetResolvedObjectInternal<TS, TK>(GetNewTraceInfo(), sourceObject);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TS"></typeparam>
        /// <typeparam name="TK"></typeparam>
        /// <param name="sourceObject"></param>
        /// <returns></returns>
        private List<TK> GetResolvedObjectList<TS, TK>(List<TS> sourceObject) where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(GetNewTraceInfo()))
                {
                    return repo.RepositoryProvider.GetResolvedObjectList<TS,TK>(this,sourceObject);
                }
            });
        }

        public async Task<TK> GetResolvedObjectAsync<TS, TK>(TS sourceObject) where TK : class
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => GetResolvedObjectInternal<TS,TK>(traceInfo, sourceObject));
        }
        
        private TK GetResolvedObjectInternal<TS,TK>(TraceInfo traceInfo, TS sourceObject) where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetResolvedObject<TS, TK>(this,sourceObject);
                }
            });
            
        }

        public List<TK> GetResolvedObject<TS, TK>(List<TS> sourceObject) where TK : class =>
            GetResolvedObjectList<TS,TK>(sourceObject);
            //sourceObject?.Select(GetResolvedObject<TS, TK>).ToList();

        public async Task<JsonDocument> GetJsonDocumentAsync(string commandText, List<DbParameter> parameters)
        {
            var traceInfo = ValidateIfAsyncActionCanBePerformedAndGetNewTraceInfo();
            return await Task.Run(() => GetJsonDocumentInternal(traceInfo, commandText, parameters));
        }
        public JsonDocument GetJsonDocument(string commandText, List<DbParameter> parameters)
        {
            return GetJsonDocumentInternal(GetNewTraceInfo(), commandText, parameters);
        }
        
        internal JsonDocument GetJsonDocumentInternal(TraceInfo traceInfo, string commandText, List<DbParameter> parameters)
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(traceInfo))
                {
                    return repo.RepositoryProvider.GetJSONData(commandText, parameters);
                }
            });
        }

        public Dictionary<TPK, TLk> GetPKLookupResolvedDictionaryForSingleType<TK,TPK,TLk>(List<TPK> pkObjectList) where TPK :class,IPrimaryKeyBase<TK> where TLk :class ,IPrimaryKeyResolveBase<TPK, TK>
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(GetNewTraceInfo()))
                {
                    return repo.RepositoryProvider.GetPKLookupResolvedDictionaryForSingleType<TK,TPK,TLk>(this, pkObjectList);
                }
            });
        }

        #region "Selected Columns"
        //public XmlDocument DoExecuteCommandFetchSelectedColumnsByTSQL<TK>(SelectedColumnBuilder selectedColumns) where TK : class
        //             => GetRecordsByTSQLSelectedColumns<TK>(GetNewTraceInfo(), selectedColumns);

        //public JsonDocument DoExecuteCommandFetchSelectedColumnsByJSON<TK>(SelectedColumnBuilder selectedColumns) where TK : class
        //                 => GetRecordsByJsonForSelectedColumns<TK>(GetNewTraceInfo(), selectedColumns);

        //public XmlDocument GetRecordsByTSQLSelectedColumns<TK>(TraceInfo traceInfo, SelectedColumnBuilder selectedColumns) where TK : class
        //{
        //    using (var repo = GetRepository(traceInfo))
        //    {
        //        return repo.RepositoryProvider.GetRecordsByTSQLBySelectedColumns<TK>(selectedColumns);
        //    }
        //}

        //public XmlDocument DoExecuteCommandFetchSelectedColumnsByTSQL<TK>(SelectedColumnBuilder selectedColumns, string whereClause, List<DbParameter> parameters) where TK : class
        //{
        //    using (var repo = GetRepository(GetNewTraceInfo()))
        //    {
        //        return repo.RepositoryProvider.GetRecordsByTSQLBySelectedColumns<TK>(selectedColumns, whereClause, parameters);
        //    }
        //}

        //public JsonDocument GetRecordsByJsonForSelectedColumns<TK>(TraceInfo traceInfo, SelectedColumnBuilder selectedColumns) where TK : class
        //{
        //    using (var repo = GetRepository(traceInfo))
        //    {
        //        return repo.RepositoryProvider.GetRecordsByJsonForSelectedColumns<TK>(selectedColumns);
        //    }
        //}

        public JsonDocument DoExecuteCommandFetchSelectedColumnsByWhereClause<TK>(SelectedColumnBuilder selectedColumns, string whereClause, List<DbParameter> parameters) where TK : class
        {
            return ExecuteFuncWithRetry(() =>
            {
                using (var repo = GetRepository(GetNewTraceInfo()))
                {
                    return repo.RepositoryProvider.GetRecordsByJsonForSelectedColumns<TK>(selectedColumns, whereClause, parameters);
                }
            });
        }

        #endregion
    }
}