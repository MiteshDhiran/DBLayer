

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Xml;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using ConnectAndSell.DataAccessStandard.Server.Common;
using ConnectAndSell.EFCore1;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;

namespace ConnectAndSell.DataAccess.Repository.EFCore1
{
    /// <summary>
    /// Core1DynamicRepository
    /// </summary>
    /// <seealso cref="ConnectAndSell.DataAccessStandard.Server.Common.RepositoryProviderBase" />
    public class Core1DynamicRepository : RepositoryProviderBase
    {
        
        private DbContext DbContext { get; }

        
        private IModel EFModel => Core1ModelBuilder.GetEFModel(ORMContext);

        /// <summary>
        /// Initializes a new instance of the <see cref="Core1DynamicRepository"/> class.
        /// </summary>
        /// <param name="arg">The argument.</param>
        public Core1DynamicRepository(RepositoryBaseArg arg) : base(arg)
        {
            DbContextOptionsBuilder<GenericDbContext> optionsBuilder = new DbContextOptionsBuilder<GenericDbContext>();
            optionsBuilder.UseSqlServer(Connection);
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.EnableSensitiveDataLogging(true);
            DbContext = new GenericDbContext(ORMContext,optionsBuilder);
            DbContext.ChangeTracker.LazyLoadingEnabled = false;
            DbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            DbContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            if (arg.TransactionToken != null)
            {
                DbContext.Database.UseTransaction(Transaction);
            }
        }

        protected override void CleanupLocalContext()
        {
            try
            {
                DbContext?.Dispose();
            }
            finally
            {
            }
        }
        
        protected override SqlTransaction GetNewTransaction()
        {
            return  ConvertContextTransactionToSqlTransaction(
                this.DbContext.Database.BeginTransaction(IsolationLevel.ReadCommitted));
        }
        
        private SqlTransaction ConvertContextTransactionToSqlTransaction(IDbContextTransaction transaction)
        {
            RelationalTransaction relationalTransaction = transaction as RelationalTransaction;
            if (relationalTransaction == null) throw new ArgumentException($"nameof(transaction) cannot be converted to RelationalTransaction");
            DbTransaction dbTransaction = relationalTransaction.GetDbTransaction();
            if (dbTransaction == null) throw new ArgumentException($"nameof(transaction) cannot be converted to dbTransaction");
            SqlTransaction sqlTransaction = dbTransaction as SqlTransaction;
            if (sqlTransaction == null) throw new ArgumentException($"nameof(transaction) cannot be converted to SqlTransaction");
            return sqlTransaction;
        }

        public override TK ExecuteDynamicStoredProcedureWithSqlXmlReader<TK>(string spName, int timeOutInSeconds, MethodInfo mInfo,
            Func<XmlReader, TK> funcReader, Func<DbCommand, TK> funcCommand, params object[] arguments)
        {
            using (var command = CreateCommandWithDynamicSP(spName,mInfo,arguments))
            {
                command.Connection = Connection;
                command.CommandTimeout = timeOutInSeconds;
                command.Transaction = Transaction;
                using (var executeReader = command.ExecuteXmlReader())
                {
                    var result = funcReader(executeReader);
                    executeReader.Close();
                    funcCommand?.Invoke(command);
                    return result;
                }
            }
        }

        public override List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(Expression<Func<TK, bool>> filterExpression, Expression<Func<TK, TO>> projectionExpression,
            bool includeChildRecords, int? maxTop) 
        {
            Func<TK,TO> compiledMappingFunc = projectionExpression.Compile();
            var query = includeChildRecords ? DbContext.Set<TK>().IncludeChildRecords(ModelMetaInfo) : DbContext.Set<TK>();
            return query.Where((Expression<Func<TK,bool>>)filterExpression).Select(compiledMappingFunc).ToList();
        }

        public override TK ExecuteStoredProcedureWithSQLReader<TK>(int timeOutInSeconds, MethodInfo mInfo,
            Func<DbDataReader, TK> func,
            params object[] arguments)
        {
            return ExecuteDynamicStoredProcedureWithSQLReader<TK>(DBUtility.GetSPName(mInfo), timeOutInSeconds, mInfo, func,
                null, arguments);
        }

        public override TK ExecuteDynamicStoredProcedureWithSQLReader<TK>(string spName, int timeOutInSeconds, MethodInfo mInfo,
            Func<DbDataReader, TK> funcReader, Func<DbCommand, TK> funcCommand, params object[] arguments)
        {
            //List<Type> types = Enumerable.Select<ResultTypeAttribute,Type>((IEnumerable<ResultTypeAttribute>)Attribute.GetCustomAttributes((MemberInfo)mInfo,typeof(ResultTypeAttribute)),(Func<ResultTypeAttribute,Type>)(d => d.Type)).ToList<Type>();
            using (SqlCommand command = CreateCommandWithDynamicSP(spName,mInfo,arguments))
            {
                command.Connection = this.Connection;
                command.CommandTimeout = timeOutInSeconds;
                if (Transaction != null)
                    command.Transaction = Transaction;
                using (var executeReader = command.ExecuteReader())
                {
                    var result = funcReader(executeReader);
                    executeReader.Close();
                    funcCommand?.Invoke(command);
                    return result;
                }
            }
        }

        private static SqlCommand CreateCommandWithDynamicSP(string spName,MethodInfo mInfo,params object[] arguments)
        {
            SProcMethodInfo spMethodInfo = SProcMethodInfo.GetSPMethodInfo(mInfo,spName);
            SqlCommand cmd = new SqlCommand() {
                CommandType = CommandType.StoredProcedure,
                CommandText = spName
            };
            SetCommandParameters(arguments,spMethodInfo,cmd);
            return cmd;
        }

        private static void SetCommandParameters(object[] arguments,SProcMethodInfo spMethodInfo,SqlCommand cmd)
        {
            SqlParameter sqlParameter = new SqlParameter
            {
                ParameterName = "returnValuePara",
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(sqlParameter);
            var parameterInfoArray = spMethodInfo.AllParameterInfoArray;
            ParameterAttribute[] parameterAttributeArray = spMethodInfo.spParamters.paraAttrs;
            for (int index = 0; index < parameterAttributeArray.Length; ++index)
            {
                ParameterAttribute parameterAttribute = parameterAttributeArray[index];

                if (parameterAttribute != null)
                {
                    SqlParameter p = new SqlParameter { ParameterName = parameterAttribute.Name };
                    if (!parameterInfoArray[index].IsIn && parameterAttribute.DbType.IndexOf("(",StringComparison.Ordinal) > 0)
                    {
                        if (parameterAttribute.DbType.IndexOf(",",StringComparison.Ordinal) == -1)
                        {
                            p.Size = Convert.ToInt32(parameterAttribute.DbType.Substring(parameterAttribute.DbType.IndexOf("(",StringComparison.Ordinal) + 1,parameterAttribute.DbType.IndexOf(")") - 1 - parameterAttribute.DbType.IndexOf("(")));
                        }
                        else
                        {
                            p.Precision = Convert.ToByte(parameterAttribute.DbType.Substring(parameterAttribute.DbType.IndexOf("(",StringComparison.Ordinal) + 1,parameterAttribute.DbType.IndexOf(",",StringComparison.Ordinal) - 1 - parameterAttribute.DbType.IndexOf("(")));
                            p.Scale = Convert.ToByte(parameterAttribute.DbType.Substring(parameterAttribute.DbType.IndexOf(",",StringComparison.Ordinal) + 1,parameterAttribute.DbType.IndexOf(")",StringComparison.Ordinal) - 1 - parameterAttribute.DbType.IndexOf(",")));
                        }
                    }
                    DBUtility.GetParaSqlType(p,parameterAttribute.DbType);
                    if (parameterInfoArray[index].IsOut)
                        p.Direction = ParameterDirection.Output;
                    else if (parameterInfoArray[index].IsRef)
                        p.Direction = ParameterDirection.InputOutput;
                    if (arguments[index] == null)
                    {
                        p.Value = (object)DBNull.Value;
                        p.IsNullable = true;
                    }
                    else
                        p.Value = arguments[index];
                    cmd.Parameters.Add(p);
                }
            }
        }

        public override Tuple<List<TK>,bool> GetRecordsByExpressionByPage<TK, K>(Expression<Func<TK,bool>> filterExpression,
            Expression<Func<TK,K>> orderColumnFunc,int pageNumber,int pageSize,List<string> listOfChildTables = null) 

        {
            var count = (pageNumber > 0) ? pageNumber * pageSize : 0;
            var query = DbContext.Set<TK>().Where((Expression<Func<TK,bool>>)filterExpression).OrderBy(orderColumnFunc).Skip(count).Take(pageSize);
            var list = GetRecordsBasedOnQuery<TK>(query,true);
            var isLastPage = list.Count < pageSize;
            return new Tuple<List<TK>,bool>(list,isLastPage);
        }

        private  List<TK> GetRecordsBasedOnQuery<TK>(IQueryable<TK> queryable,bool populateChild) where TK : class
        {
            if (populateChild)
            {
                queryable = queryable.IncludeChildRecords(ModelMetaInfo);
            }
            return queryable.ToList();
        }

        public override TK GetFirstRecord<TK, MK>(Expression<Func<TK, bool>> expression, Expression<Func<TK, MK>> ascendingOrderColumnFunc, List<string> listOfChildTables = null)
        {
            return DbContext.Set<TK>().Where((Expression<Func<TK,bool>>)expression).OrderBy(ascendingOrderColumnFunc).FirstOrDefault();
        }

        public override TK GetLastRecord<TK, MK>(Expression<Func<TK, bool>> expression, Expression<Func<TK, MK>> descendingOrderColumnFunc, List<string> listOfChildTables = null)
        {
            return DbContext.Set<TK>().Where((Expression<Func<TK,bool>>)expression).OrderByDescending(descendingOrderColumnFunc).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <param name="fieldsInformation"></param>
        /// <returns></returns>
        public override List<TK> GetRecordsByMultipleFieldsUsingXML<TK>(List<FilterFieldInfo<TK>> fieldsInformation)
        {
            var detailFieldInfo = fieldsInformation.Select(c => new FilterFieldDetailInfo(this.ORMContext.ORMModelMetaInfo, c)).ToList();
            var parameterWhereClauseAndConditionTextTuple = detailFieldInfo.Aggregate(
                new Tuple<StringBuilder, int, List<SqlParameter>>(new StringBuilder(), 0, new List<SqlParameter>()),
                (sbWithIndexTuple, s) =>
                {
                    var parameterName = $"{s.ColumnName}";
                    var sqlParameter = s.StringMaxLength.HasValue ? new SqlParameter(parameterName,s.ColumnSqlDbType,s.StringMaxLength.Value) : new SqlParameter(parameterName,s.ColumnSqlDbType);
                    sqlParameter.Value = s.ColumnValue;
                    sbWithIndexTuple.Item3.Add(sqlParameter);
                    if (sbWithIndexTuple.Item2 > 0)
                    {
                        sbWithIndexTuple.Item1.Append(" " + s.ConditionType.ToString() + " ");
                    }

                    sbWithIndexTuple.Item1.Append(s.ColumnValue == null
                        ? $"{s.ColumnName} IS NULL"
                        : $"{s.ColumnName} =  @{parameterName}");
                    return new Tuple<StringBuilder, int, List<SqlParameter>>(sbWithIndexTuple.Item1,
                        sbWithIndexTuple.Item2 + 1, sbWithIndexTuple.Item3);
                });
            var dbParametrs = parameterWhereClauseAndConditionTextTuple.Item3.Cast<DbParameter>().ToList();
            var parameterizedForXmlQuery = DBXMLUtility.GetTSQLQueryForXmlResultSet(typeof(TK), this.ModelMetaInfo,parameterWhereClauseAndConditionTextTuple.Item1.ToString(), null);
            return DBUtility.ExecuteForXML<TK>(DbContext.Database.GetDbConnection() as SqlConnection,Transaction,
                dbParametrs,parameterizedForXmlQuery);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public override List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyIDYear<TK>(List<Tuple<long, int>> list) 
        {
            var commandText = DBXMLUtility.GetTSQLForXMLBasedOnPrimaryKeyWithPartitionColumn<TK>(ModelMetaInfo,  list);
            return DBUtility.ExecuteForXML<TK>(DbContext.Database.GetDbConnection() as SqlConnection, Transaction,
                new List<DbParameter>(), commandText);
        }

        
        

        public override List<TK> DoExecuteCommand<TK>(Expression<Func<TK, bool>> exp, List<string> childTableNames)
        {
            var res = DbContext.Set<TK>().IncludeSelectedChildRecords(ModelMetaInfo,childTableNames).Where(exp).ToList();
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <returns></returns>
        public override bool IsRecordExists<TK>(Expression<Func<TK, bool>> exp) 
        {
            return DbContext.Set<TK>().Where((Expression<Func<TK, bool>>)exp).Take(1).Any();
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TP"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public override List<TK> GetRecordsUsingXMLBasedOnPrimaryKeyBasicValues<TK,TP>(List<TP> list) 
        {
            var tsqlCommandText = DBXMLUtility.GetTSQLForXMLBasedOnPrimaryKeyBasicValues<TP>(ModelMetaInfo, typeof(TK), list);
            return DBUtility.ExecuteForXML<TK>(DbContext.Database.GetDbConnection() as SqlConnection, Transaction,
                new List<DbParameter>(), tsqlCommandText);

        }

        /// <summary>
        /// Gets the records by key values.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <typeparam name="TP">The type of the p.</typeparam>
        /// <param name="primaryKeyValues">The primary key values.</param>
        /// <returns></returns>
        public override  List<TK> GetRecordsByKeyValues<TK, TP>(List<TP> primaryKeyValues)
        {
            var tsqlCommandText = DBXMLUtility.GenerateXMLFilterCriteriaForPKValues<TP>(ModelMetaInfo, typeof(TK), primaryKeyValues);
            return DBUtility.ExecuteForXML<TK>(DbContext.Database.GetDbConnection() as SqlConnection, Transaction,
                new List<DbParameter>(), tsqlCommandText);
        }

        /// <summary>
        /// Does the execute command.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exp">The exp.</param>
        /// <param name="getChildRecords">if set to <c>true</c> [get child records].</param>
        /// <returns></returns>
        public override List<T> DoExecuteCommand<T>(Expression<Func<T, bool>> exp, bool getChildRecords) 
        {
            if (getChildRecords)
            {
                var res = DbContext.Set<T>().IncludeChildRecords(ModelMetaInfo).Where(exp).ToList();
                return res;
            }
            else
            {
                return DbContext.Set<T>().Where(exp).ToList();
            }
        }

        /// <summary>
        /// Saves the records.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public override bool SaveRecords<TK>(List<TK> records) 
        {
            var retValTuple = SaveRecordsInternal(DbContext, ModelMetaInfo, false, CurrentTraceInfo.UserName, DateTimeOffset.UtcNow, records);
            return retValTuple.Item2;
        }

        /// <summary>
        /// Saves the records with change tracking data.
        /// </summary>
        /// <typeparam name="TK">The type of the k.</typeparam>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        public override Dictionary<object, List<ChangeTrackingInfo>> SaveRecordsWithChangeTrackingData<TK>(List<TK> records) 
        {
            var retValTuple = SaveRecordsInternal(DbContext, ModelMetaInfo, true, CurrentTraceInfo.UserName, DateTimeOffset.UtcNow, records);
            return retValTuple.Item1;
        }

        private static Tuple<Dictionary<object, List<ChangeTrackingInfo>>, bool> SaveRecordsInternal<TK>(DbContext dbContext, ORMModelMetaInfo model, bool isAuditingRequired, string currentUser, DateTimeOffset currentDateTimeOffset, List<TK> recordList) where TK: ICoreEntity 
        {
            var trackingDictionary = new Dictionary<object, List<ChangeTrackingInfo>>();
            if (recordList == null || recordList.Any() == false) return new Tuple<Dictionary<object, List<ChangeTrackingInfo>>, bool>(trackingDictionary, true);
            var tuple =
                new Tuple<
                    Dictionary<Type, List<object>>
                    , Dictionary<Type, List<object>>
                    , Dictionary<Type, Stack<object>>
                    , Dictionary<Type, List<object>>
                >
                (
                    new Dictionary<Type, List<object>>()
                    , new Dictionary<Type, List<object>>()
                    , new Dictionary<Type, Stack<object>>()
                    , new Dictionary<Type, List<object>>()
                );
            var newEntities = recordList.Where(t => t.DataEntityState == DomainEntityState.New).ToList();
            foreach (var entity in newEntities)
            {
                DBUtility.MarkChildAsNewIfRecordIsNew(entity, model);
            }

            DBUtility.SegregateRecords(tuple, recordList.Cast<object>().ToList(), model);


            if (tuple.Item3.Any()) //delete
            {
                foreach (var rec in tuple.Item3.Keys.SelectMany(tableType => tuple.Item3[tableType]))
                {
                    dbContext.Entry(rec).State = EntityState.Deleted;
                }
            }

            if (tuple.Item1.Any()) //new
            {
                foreach (var tableType in tuple.Item1.Keys) //added
                {
                    DBUtility.SetSystemColumnValuesForInsert(tuple.Item1[tableType], model, currentUser,
                        currentDateTimeOffset);
                    foreach (var rec in tuple.Item1[tableType])
                    {
                        dbContext.Entry(rec).State = EntityState.Added;
                    }
                }
            }

            if (tuple.Item2.Any()) //update
            {
                foreach (var tableType in tuple.Item2.Keys) //update
                {
                    DBUtility.SetSystemColumnValuesForUpdate(tuple.Item2[tableType], model, currentUser,
                        currentDateTimeOffset);
                    foreach (var rec in tuple.Item2[tableType])
                    {
                        dbContext.Entry(rec).State = EntityState.Modified;
                    }
                }
            }

            if (tuple.Item4.Any()) //unchanged
            {
                foreach (var rec in tuple.Item4.Keys.SelectMany(tableType => tuple.Item4[tableType]))
                {
                    dbContext.Entry(rec).State = EntityState.Unchanged;
                }
            }


            if (isAuditingRequired)
            {
                PerformChangeTracking(dbContext, trackingDictionary); //tracks old value new value
            }

            dbContext.SaveChanges(true); //Sets the primary key value from database

            /*
            if (isAuditingRequired)
            {
                var auditEventWithDetailsList = DBUtility.FillAuditData(trackingDictionary, auditArgInfo);
                auditArgInfo.PersistAuditFunc(auditEventWithDetailsList);
            }
            */

            //mark all records as unchanged

            var listToBeMarkedUnchanged = tuple.Item1.SelectMany(o => o.Value.Select(z => (ICoreEntity)z))
                .Union(tuple.Item2.SelectMany(o => o.Value.Select(z => (ICoreEntity)z)))
                .Union(tuple.Item3.SelectMany(o => o.Value.Select(z => (ICoreEntity)z)))
                .Union(tuple.Item4.SelectMany(o => o.Value.Select(z => (ICoreEntity)z))).ToList();

            foreach (var item in listToBeMarkedUnchanged)
            {
                item.DataEntityState = DomainEntityState.Unchanged;
            }

            return new Tuple<Dictionary<object, List<ChangeTrackingInfo>>, bool>(trackingDictionary, true);
        }

        private static void PerformChangeTracking(
            DbContext dbContext, Dictionary<object, List<ChangeTrackingInfo>> agg)
        {
            foreach (var entityEntry in dbContext.ChangeTracker.Entries())
            {
                var isModified = entityEntry.State == EntityState.Modified;
                var currentEntityState = DomainEntityState.Unchanged;
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        currentEntityState = DomainEntityState.New;
                        break;
                    case EntityState.Deleted:
                        currentEntityState = DomainEntityState.Deleted;
                        break;
                    case EntityState.Modified:
                        currentEntityState = DomainEntityState.Modified;
                        break;
                    case EntityState.Unchanged:
                        currentEntityState = DomainEntityState.Unchanged;
                        break;
                    case EntityState.Detached:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                var currentTableName =
                    dbContext.Model.FindEntityType(entityEntry.Entity.GetType()).GetTableName();
                var allParentPath = GetParentPath(dbContext, entityEntry);
                var originalValues = isModified ? entityEntry.GetDatabaseValues() : entityEntry.CurrentValues;
                var primaryPropertyInfoList = entityEntry.Metadata.FindPrimaryKey().Properties.Select(t => new ColumnNetInfo(t.PropertyInfo)).Cast<IColumnNetInfo>().ToList();
                var allProperties = entityEntry.CurrentValues.Properties.Select(p => p.Name).ToList();
                var rootRecordInfo = allParentPath.Count == 0
                    ? new RootRecordInfo(entityEntry.Entity.GetType(), currentTableName,
                        primaryPropertyInfoList, entityEntry.Entity)
                    : new RootRecordInfo(allParentPath.Last().ParentClassType,
                        allParentPath.Last().ParentTableName, allParentPath.Last().PrimaryKeyPropertyInfos, allParentPath.Last().ParentObject);
                var entityPropertiesOldValueNewValue = allProperties.Aggregate(new List<ChangeTrackingInfo>(), (list, propertyName) =>
                {
                    list.Add(new ChangeTrackingInfo(rootRecordInfo, allParentPath, entityEntry.Entity, primaryPropertyInfoList, currentEntityState, entityEntry.Entity.GetType(), currentTableName, propertyName, originalValues[propertyName], entityEntry.CurrentValues[propertyName]));
                    return list;
                });
                if (agg.ContainsKey(entityEntry.Entity) == false)
                {
                    agg.Add(entityEntry.Entity, entityPropertiesOldValueNewValue);
                }
            }
        }

        private static Queue<ParentRecordInfo> GetParentPath(DbContext dbContext, EntityEntry entry)
        {
            Queue<ParentRecordInfo> GetParentPathInternal(DbContext dbContextInternal
                , EntityEntry entryInternal, Queue<ParentRecordInfo> rootObjectPath)
            {

                var navigationEntry = entryInternal.Navigations.FirstOrDefault(n => n.Metadata.IsDependentToPrincipal());
                if (navigationEntry == null) return rootObjectPath;
                var parentPropertyNamePointingToChild = navigationEntry.Metadata.FindInverse().PropertyInfo.Name;
                var parentClassName = navigationEntry.Metadata.FindInverse().DeclaringEntityType.Name;
                var parentClassType = navigationEntry.Metadata.FindInverse().DeclaringEntityType.ClrType;
                var parentTableName = dbContextInternal.Model
                    .FindEntityType(navigationEntry.Metadata.FindInverse().DeclaringEntityType.ClrType).GetTableName();
                var firstReferenceEntry =
                    navigationEntry.EntityEntry.Navigations.First(nn =>
                        nn is ReferenceEntry) as ReferenceEntry;
                Debug.Assert(firstReferenceEntry != null, nameof(firstReferenceEntry) + " != null");
                var parentEntityEntry = firstReferenceEntry.TargetEntry;
                var primaryPropertyInfoList = parentEntityEntry.Metadata.FindPrimaryKey().Properties.Select(s => new ColumnNetInfo(s.PropertyInfo) ).Cast<IColumnNetInfo>().ToList();
                var parentEntity = parentEntityEntry.Entity;
                var parentRecordInfo = new ParentRecordInfo(parentClassType, parentClassName, parentTableName, parentPropertyNamePointingToChild, primaryPropertyInfoList, parentEntity);
                rootObjectPath.Enqueue(parentRecordInfo);
                return GetParentPathInternal(dbContextInternal, parentEntityEntry, rootObjectPath);
            }
            return GetParentPathInternal(dbContext, entry, new Queue<ParentRecordInfo>());
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
        public override List<TO> DoExecuteCommandFetchSelectedColumns<TK, TO>(Expression<Func<TK, bool>> exp, Expression<Func<TK, TO>> mappingExp, bool includeChildRecords) 
        {
            var query = includeChildRecords ? DbContext.Set<TK>().IncludeChildRecords(ModelMetaInfo) : DbContext.Set<TK>();
            return query.Where((Expression<Func<TK, bool>>)exp).Select(mappingExp).ToList();
        }


        protected override void OnPreCompile()
        {
            Core1ModelBuilder.GetEFModel(ORMContext);
        }

        /// <summary>
        /// Executes the stored proc.
        /// </summary>
        /// <param name="mInfo">The m information.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public override ResultSet ExecuteStoredProc(MethodInfo mInfo, params object[] arguments)
        {
            return DapperHelper.ExecuteStoredProc(Connection, Transaction, mInfo, arguments);
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