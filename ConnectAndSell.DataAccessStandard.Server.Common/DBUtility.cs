


using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Transactions;
using System.Xml;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    /// <summary>
    /// DBUtility class
    /// </summary>
    public static class DBUtility
    {

        private static readonly ConcurrentDictionary<string, SProcMethodInfo> ProcMethodInfoCacheDic =
            new ConcurrentDictionary<string, SProcMethodInfo>();

        /// <summary>
        /// Segregates the records.
        /// </summary>
        /// <param name="tuple">The tuple.</param>
        /// <param name="recordList">The record list.</param>
        /// <param name="model">The model.</param>
        public static void SegregateRecords(
            Tuple<Dictionary<Type, List<object>>, Dictionary<Type, List<object>>, Dictionary<Type, Stack<object>>,
                Dictionary<Type, List<object>>> tuple, List<Object> recordList,
            ORMModelMetaInfo model)
        {
            if (recordList == null || recordList.Any() == false) return;
            var type = recordList.First().GetType();
            var dicBasedOnStatus = recordList.ToLookup(p => ((IMDRXCoreEntity) p).DataEntityState)
                .ToDictionary(kv => kv.Key, kvg => kvg.ToList());
            if (dicBasedOnStatus.ContainsKey(DomainEntityState.New))
            {
                if (tuple.Item1.ContainsKey(type) == false)
                {
                    tuple.Item1.Add(type, new List<object>());
                }

                tuple.Item1[type].AddRange(dicBasedOnStatus[DomainEntityState.New]);
            }

            if (dicBasedOnStatus.ContainsKey(DomainEntityState.Modified))
            {
                if (tuple.Item2.ContainsKey(type) == false)
                {
                    tuple.Item2.Add(type, new List<object>());
                }

                tuple.Item2[type].AddRange(dicBasedOnStatus[DomainEntityState.Modified]);
            }

            if (dicBasedOnStatus.ContainsKey(DomainEntityState.Deleted))
            {
                if (tuple.Item3.ContainsKey(type) == false)
                {
                    tuple.Item3.Add(type, new Stack<object>());
                }

                dicBasedOnStatus[DomainEntityState.Deleted].ForEach(p =>
                {
                    var delRecord = p as IMDRXCoreEntity;
                    if (delRecord != null)
                    {
                        DeleteWithChild(delRecord, model);
                    }

                    tuple.Item3[type].Push(p);
                });
            }

            if (dicBasedOnStatus.ContainsKey(DomainEntityState.Unchanged))
            {
                if (tuple.Item4.ContainsKey(type) == false)
                {
                    tuple.Item4.Add(type, new List<object>());
                }

                dicBasedOnStatus[DomainEntityState.Unchanged].ForEach(p => tuple.Item4[type].Add(p));
            }

            //For each entity set property info get the list of objects and call the method recursively
            var proprtyInfos = GetChildEntitySetProperties(type, model);
            foreach (var entitySetPropertyInfo in proprtyInfos)
            {
                var listOfObjects = recordList.Select(r => entitySetPropertyInfo.GetValue(r, null) as IEnumerable)
                    .Where(list => list != null).ToList();
                if (listOfObjects.Any())
                {
                    List<object> flatList = new List<object>();
                    foreach (var lo in listOfObjects)
                    {
                        flatList.AddRange(lo.Cast<object>());
                    }

                    if (flatList.Any())
                    {
                        SegregateRecords(tuple, flatList, model);
                    }
                }
            }
        }

        private static void DeleteWithChild(IMDRXCoreEntity obj, ORMModelMetaInfo model)
        {
            obj.Delete();
            List<object> childs = obj.GetChildEntitySet(model);
            foreach (IList items in childs)
            {
                var newItems = new List<IMDRXCoreEntity>();
                foreach (IMDRXCoreEntity item in items)
                {
                    //items which are new cannot be deleted from DB and hence need to be removed from in-memory child collection of parent class.
                    if (item.DataEntityState == DomainEntityState.New)
                    {
                        newItems.Add(item);
                    }
                    else
                    {
                        DeleteWithChild(item, model);
                    }
                }

                if (newItems.Any())
                {
                    newItems.ForEach(n => items.Remove(n));
                }
            }
        }

        private static void Delete(this IMDRXCoreEntity entity)
        {
            if (entity.DataEntityState == DomainEntityState.New)
            {
                throw new InvalidOperationException(
                    "You cannot delete a record that is not yet inserted. Remove it from containing collections, instead");
            }

            entity.DataEntityState = DomainEntityState.Deleted;
        }

        /// <summary>
        /// Gets the child entity set properties.
        /// </summary>
        /// <param name="entityType">Type of the entity.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <returns></returns>
        public static ConcurrentBag<PropertyInfo> GetChildEntitySetProperties(this Type entityType,
            ORMModelMetaInfo modelMetaInfo)
        {
            PropertyInfo[] propertyInfo = entityType.GetProperties();
            var childPropertyNames = modelMetaInfo.GetImmediateChildPropertyNames(entityType);
            return new ConcurrentBag<PropertyInfo>(propertyInfo.ToList()
                .Where(p => childPropertyNames.Contains(p.Name, StringComparer.OrdinalIgnoreCase)
                ).ToList());
        }

        /// <summary>
        /// Gets the child entity set.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <returns></returns>
        public static List<object> GetChildEntitySet(this IMDRXCoreEntity entity, ORMModelMetaInfo modelMetaInfo)
        {
            var propertyInfoList = GetChildEntitySetProperties(entity.GetType(), modelMetaInfo);
            var retVal = propertyInfoList.Aggregate(new List<Object>(), (agg, property) =>
            {
                agg.Add(property.GetValue(entity));
                return agg;
            });
            return retVal;
        }

        /// <summary>
        /// Marks the child as new if record is new.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <param name="model">The model.</param>
        public static void MarkChildAsNewIfRecordIsNew(IMDRXCoreEntity record, ORMModelMetaInfo model)
        {
            var heliosRecord = record;
            if (heliosRecord == null || heliosRecord.DataEntityState != DomainEntityState.New) return;
            var propertyInfos = GetChildEntitySetProperties(record.GetType(), model);
            foreach (var propertyInfo in propertyInfos)
            {
                var entitySetValue = propertyInfo.GetValue(record, null) as IEnumerable;
                if (entitySetValue != null)
                {
                    foreach (var childRecord in entitySetValue)
                    {
                        var objectStatusRecord = childRecord as IMDRXCoreEntity;
                        if (objectStatusRecord != null && objectStatusRecord.DataEntityState != DomainEntityState.New)
                        {
                            objectStatusRecord.DataEntityState = DomainEntityState.New;
                            MarkChildAsNewIfRecordIsNew(objectStatusRecord, model);
                        }
                        else
                        {
                            break; //if one of the child record is new then others will be new
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the <see ref="records"/> with system populated columns for update
        /// </summary>
        /// <param name="records"></param>
        /// <param name="userName"></param>
        /// <param name="dateTime"></param>
        public static void SetSystemColumnValuesForUpdate(List<object> records,
            ORMModelMetaInfo model, string userName, DateTimeOffset dateTime)
        {
            if (records == null || records.Any() == false) return;
            SetSystemDateInSystemDefinedColumn(
                new List<MDRXSystemDefinedColumn>() {MDRXSystemDefinedColumn.TouchedWhenUTC}, model,
                records, dateTime);
            SetUserInSystemDefinedColumn(new List<MDRXSystemDefinedColumn>() {MDRXSystemDefinedColumn.TouchedBy},
                model, records, userName);
        }

        /// <summary>
        /// Updates the <see ref="records"/> with system populated columns for insert
        /// </summary>
        /// <param name="records"></param>
        /// <param name="userName"></param>
        /// <param name="dateTime"></param>
        public static void SetSystemColumnValuesForInsert(List<object> records,
            ORMModelMetaInfo model, string userName, DateTimeOffset dateTime)
        {
            if (records == null || records.Any() == false) return;
            SetSystemDateInSystemDefinedColumn(
                new List<MDRXSystemDefinedColumn>()
                    {MDRXSystemDefinedColumn.CreatedWhenUTC, MDRXSystemDefinedColumn.TouchedWhenUTC},
                model, records, dateTime);
            SetUserInSystemDefinedColumn(
                new List<MDRXSystemDefinedColumn>() {MDRXSystemDefinedColumn.CreatedBy, MDRXSystemDefinedColumn.TouchedBy},
                model, records, userName);
        }

        /// <summary>
        /// Updates the  <see ref="records"/> system populated username properties  with <see ref="userName"/>
        /// </summary>
        /// <param name="systemDefinedColumns"></param>
        /// <param name="records"></param>
        /// <param name="userName"></param>
        private static void SetUserInSystemDefinedColumn(List<MDRXSystemDefinedColumn> systemDefinedColumns,
            ORMModelMetaInfo model, List<object> records, string userName)
        {
            if (systemDefinedColumns == null || records == null || records.Any() == false) return;
            var propertyInfos =
                GetAutogeneratedPropertyInfoDic(
                        new Tuple<ORMModelMetaInfo, Type>(model, records.First().GetType()))
                    .Join(systemDefinedColumns, os => os.Key, iis => iis, (o, i) => o.Value);
            foreach (var r in records)
            {
                foreach (var pi in propertyInfos)
                {
                    pi.SetValue(r, userName);
                }
            }
        }

        /// <summary>
        /// Updates the <see ref="records"/> system populated datetime properties with <see ref="dateTime"/>
        /// </summary>
        /// <param name="systemDefinedColumns"></param>
        /// <param name="records"></param>
        /// <param name="dateTime"></param>
        private static void SetSystemDateInSystemDefinedColumn(List<MDRXSystemDefinedColumn> systemDefinedColumns,
            ORMModelMetaInfo modelMetaInfo, List<object> records, DateTimeOffset dateTime)
        {
            if (systemDefinedColumns == null || records == null || records.Any() == false) return;
            var propertyInfos =
                GetAutogeneratedPropertyInfoDic(
                        new Tuple<ORMModelMetaInfo, Type>(modelMetaInfo, records.First().GetType()))
                    .Join(systemDefinedColumns, os => os.Key, iis => iis, (o, i) => o.Value);
            foreach (var r in records)
            {
                foreach (var pi in propertyInfos)
                {
                    pi.SetValue(r, dateTime);
                }
            }
        }

        /// <summary>
        /// Function to get system defined property infos
        /// </summary>
        private static readonly
            Func<Tuple<ORMModelMetaInfo, Type>, Dictionary<MDRXSystemDefinedColumn, PropertyInfo>>
            GetAutogeneratedPropertyInfoDic =
                Memonize<Tuple<ORMModelMetaInfo, Type>, Dictionary<MDRXSystemDefinedColumn, PropertyInfo>>(
                    (func, tuple) =>
                    {
                        var propertiesAndColumnAttributesDic = tuple.Item2
                            .GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                            .ToLookup(k => k.Name).ToDictionary(kv => kv.Key, kvg => kvg.First(),
                                StringComparer.InvariantCultureIgnoreCase);

                        return tuple.Item1.GetSystemDefinedColumnTypeNameDictionary().Select(p =>
                            {
                                PropertyInfo ret;
                                propertiesAndColumnAttributesDic.TryGetValue(p.Value, out ret);
                                return new Tuple<MDRXSystemDefinedColumn, PropertyInfo>(p.Key, ret);
                            }).Where(t => t.Item2 != null).ToLookup(kv => kv.Item1)
                            .ToDictionary(kv => kv.Key, kvg => kvg.First().Item2);
                    });

        public static Func<T, TK> Memonize<T, TK>(this Func<Func<T, TK>, T, TK> funToMemonize)
        {
            var m = new ConcurrentDictionary<T, TK>();
            var lockDic =
                new ConcurrentDictionary<T, object>(); //added locking mechanism so that only one function call is made in case of concurrently function is called for same input parameter
            Func<T, TK> retVal = null;
            retVal = x =>
            {
                if (m.ContainsKey(x))
                {
                    return m[x];
                }
                else
                {
                    object lockObj = new object();
                    lockDic.TryAdd(x, lockObj);
                    object objFromDic;
                    lockObj = lockDic.TryGetValue(x, out objFromDic)
                        ? objFromDic
                        : lockObj; //if lockDic.TryRemove(x, out obj); is called in another thread then value will not be in dic - so taking extra caution
                    TK result;
                    lock (lockObj)
                    {
                        if (m.ContainsKey(x) == false) // double check if the object is already fetched
                        {
                            result = funToMemonize(retVal, x);
                            m.TryAdd(x, result);
                        }
                        else
                        {
                            m.TryGetValue(x, out result);
                        }
                    }

                    object obj;
                    lockDic.TryRemove(x, out obj);
                    return result;
                }
            };
            return retVal;
        }

        
        public static string GetSPName(MethodInfo mInfo)
        {
            var functionAttribute = (FunctionAttribute) Attribute.GetCustomAttribute(mInfo, typeof(FunctionAttribute));
            return functionAttribute.Name;
        }

        private static SqlCommand CreateCommand(MethodInfo mInfo, params object[] arguments)
        {
            string name = GetSPName(mInfo);
            SProcMethodInfo spMethodInfo2 = GetSPMethodInfo(mInfo, name);
            SqlCommand cmd = new SqlCommand()
            {
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandText = name
            };
            SetCommandParameters(arguments, spMethodInfo2, cmd);
            return cmd;
        }

        private static void SetCommandParameters(object[] arguments, SProcMethodInfo spMethodInfo, SqlCommand cmd)
        {
            SqlParameter sqlParameter = new SqlParameter
            {
                ParameterName = "returnValuePara",
                Direction = ParameterDirection.ReturnValue
            };
            cmd.Parameters.Add(sqlParameter);
            ParameterInfo[] parameterInfoArray = spMethodInfo.spParamters.paraInfo
                .Where(c => c.CustomAttributes.Any(t => t.AttributeType == typeof(ParameterAttribute))).ToArray();
            ParameterAttribute[] parameterAttributeArray = spMethodInfo.spParamters.paraAttrs;
            for (int index = 0; index < parameterAttributeArray.Length; ++index)
            {
                ParameterAttribute parameterAttribute = parameterAttributeArray[index];

                if (parameterAttribute != null)
                {
                    SqlParameter p = new SqlParameter {ParameterName = parameterAttribute.Name};
                    if (!parameterInfoArray[index].IsIn &&
                        parameterAttribute.DbType.IndexOf("(", StringComparison.Ordinal) > 0)
                    {
                        if (parameterAttribute.DbType.IndexOf(",", StringComparison.Ordinal) == -1)
                        {
                            p.Size = Convert.ToInt32(parameterAttribute.DbType.Substring(
                                parameterAttribute.DbType.IndexOf("(", StringComparison.Ordinal) + 1,
                                parameterAttribute.DbType.IndexOf(")") - 1 - parameterAttribute.DbType.IndexOf("(")));
                        }
                        else
                        {
                            p.Precision = Convert.ToByte(parameterAttribute.DbType.Substring(
                                parameterAttribute.DbType.IndexOf("(", StringComparison.Ordinal) + 1,
                                parameterAttribute.DbType.IndexOf(",", StringComparison.Ordinal) - 1 -
                                parameterAttribute.DbType.IndexOf("(")));
                            p.Scale = Convert.ToByte(parameterAttribute.DbType.Substring(
                                parameterAttribute.DbType.IndexOf(",", StringComparison.Ordinal) + 1,
                                parameterAttribute.DbType.IndexOf(")", StringComparison.Ordinal) - 1 -
                                parameterAttribute.DbType.IndexOf(",")));
                        }
                    }

                    GetParaSqlType(p, parameterAttribute.DbType);
                    if (parameterInfoArray[index].IsOut)
                        p.Direction = ParameterDirection.Output;
                    else if (parameterInfoArray[index].ParameterType.IsByRef)
                        p.Direction = ParameterDirection.InputOutput;
                    if (arguments[index] == null)
                    {
                        p.Value = (object) DBNull.Value;
                        p.IsNullable = true;
                    }
                    else
                        p.Value = arguments[index];

                    cmd.Parameters.Add(p);
                }
            }
        }

        /// <summary>
        /// Gets the stored procedure command input context.
        /// </summary>
        /// <param name="mInfo">The m information.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public static CommandInputContextInfo GetStoredProcedureCommandInputContext(MethodInfo mInfo, params object[] arguments)
        {
            var dynamicSPParameterObject = new System.Dynamic.ExpandoObject() as IDictionary<string, Object>;
            var spName = GetSPName(mInfo);
            var spMethodInfo = GetSPMethodInfo(mInfo, spName);
            Dictionary<DBParameterArgInfo,object> inputParameterNameDictionaryValue = new Dictionary<DBParameterArgInfo, object>();
            foreach (var inputArg in spMethodInfo.InputArgs)
            {
                inputParameterNameDictionaryValue.Add(inputArg,arguments[inputArg.ArgIndex]);
                dynamicSPParameterObject.Add(new KeyValuePair<string, object>(inputArg.ParameterNameWithoutAtSign, arguments[inputArg.ArgIndex]));
            }

            var retVal = new CommandInputContextInfo(spName,dynamicSPParameterObject,spMethodInfo.ReturnRecordTypeList,spMethodInfo.InputArgs,spMethodInfo.OutputArgs,inputParameterNameDictionaryValue);
            return retVal;
        }

        public static CommandInputContextInfo GetStoredProcedureCommandInputContextWithSpecificSPName(string spName, MethodInfo mInfo,params object[] arguments)
        {
            var dynamicSPParameterObject = new System.Dynamic.ExpandoObject() as IDictionary<string,Object>;
            var spMethodInfo = GetSPMethodInfo(mInfo,spName);
            Dictionary<DBParameterArgInfo,object> inputParameterNameDictionaryValue = new Dictionary<DBParameterArgInfo,object>();
            foreach (var inputArg in spMethodInfo.InputArgs)
            {
                inputParameterNameDictionaryValue.Add(inputArg,arguments[inputArg.ArgIndex]);
                dynamicSPParameterObject.Add(new KeyValuePair<string,object>(inputArg.ParameterNameWithoutAtSign,arguments[inputArg.ArgIndex]));
            }

            var retVal = new CommandInputContextInfo(spName,dynamicSPParameterObject,spMethodInfo.ReturnRecordTypeList,spMethodInfo.InputArgs,spMethodInfo.OutputArgs,inputParameterNameDictionaryValue);
            return retVal;
        }

        public static void GetParaSqlType(SqlParameter p, string paraType)
        {
            string str2 = paraType.IndexOf("(", StringComparison.Ordinal) <= 0
                ? paraType.Trim()
                : paraType.Substring(0, paraType.IndexOf("(", StringComparison.InvariantCultureIgnoreCase)).Trim();
            if (!SqlTypemapper.ContainsKey(str2.ToLower()))
                return;
            p.SqlDbType = SqlTypemapper[str2.ToLower()];
        }

        /// <summary>
        /// The SQL typemapper
        /// </summary>
        internal static readonly ConcurrentDictionary<string, SqlDbType> SqlTypemapper =
            new ConcurrentDictionary<string, SqlDbType>(
                new Dictionary<string, SqlDbType>()
                {
                    {"bigint", SqlDbType.BigInt},
                    {"long", SqlDbType.BigInt}, {"binary", SqlDbType.Binary}, {"bit", SqlDbType.Bit},
                    {"char", SqlDbType.Char}, {"datetime", SqlDbType.DateTime}, {"date", SqlDbType.Date},
                    {"time", SqlDbType.Time}, {"datetime2", SqlDbType.DateTime2},
                    {"datetimeoffset", SqlDbType.DateTimeOffset}, {"smalldatetime", SqlDbType.SmallDateTime},
                    {"decimal", SqlDbType.Decimal}, {"image", SqlDbType.Image}, {"int", SqlDbType.Int},
                    {"nchar", SqlDbType.NChar}, {"ntext", SqlDbType.NText}, {"nvarchar", SqlDbType.NVarChar},
                    {"real", SqlDbType.Real}, {"smallmoney", SqlDbType.SmallMoney}, {"smallint", SqlDbType.SmallInt},
                    {"text", SqlDbType.Text}, {"timestamp", SqlDbType.Timestamp}, {"tinyint", SqlDbType.TinyInt},
                    {"uniqueidentifier", SqlDbType.UniqueIdentifier}, {"varbinary", SqlDbType.VarBinary},
                    {"varchar", SqlDbType.VarChar}, {"variant", SqlDbType.Variant}, {"sql_variant", SqlDbType.Variant},
                    {"xml", SqlDbType.Xml}, {"numeric", SqlDbType.Decimal}
                }
            );

        public static SqlDbType? GetSqlParameterDbType(string paraType)
        {
            string str2 = paraType.IndexOf("(", StringComparison.Ordinal) <= 0
                ? paraType.Trim()
                : paraType.Substring(0, paraType.IndexOf("(", StringComparison.InvariantCultureIgnoreCase)).Trim();
            if (!DBUtility.SqlTypemapper.ContainsKey(str2.ToLower()))
                return null;
            str2 = str2.EndsWith(" NULL", StringComparison.InvariantCultureIgnoreCase)
                ? str2.Substring(0, str2.Length - 4).Trim() : str2;
            str2 = str2.EndsWith(" NOT NULL", StringComparison.InvariantCultureIgnoreCase)
                ? str2.Substring(0, str2.Length - " NOT NULL".Length).Trim() : str2;    
            return DBUtility.SqlTypemapper[str2.ToLower()];
        }

        private static SProcMethodInfo GetSPMethodInfo(MethodInfo mInfo, string spName)
        {
            SProcMethodInfo spMethodInfo;
            if (ProcMethodInfoCacheDic.TryGetValue(spName, out spMethodInfo) == false)
            {
                spMethodInfo = new SProcMethodInfo(mInfo, spName);
                ProcMethodInfoCacheDic.TryAdd(spName, spMethodInfo);
            }

            return spMethodInfo;
        }

        /// <summary>
        /// Executes for XML.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <returns></returns>
        public static List<T> ExecuteForXML<T>(SqlConnection connection, SqlTransaction transaction,List<DbParameter> parameters, string sqlCommandText)
        {
            using (var cmd = new SqlCommand(sqlCommandText))
            {
                parameters?.ForEach(p => cmd.Parameters.Add(new SqlParameter(p.ParameterName,DbTypeToSqlDbType[p.DbType], p.Size){Value = p.Value}));
                cmd.Connection = connection;
                cmd.Transaction = transaction;
                using (var reader = cmd.ExecuteXmlReader())
                {
                    var retVal = reader.Read() ? reader.DeserializeDataContractObject<List<T>>() : new List<T>();
                    //SetObjectStatusToUnModified(retVal);
                    return retVal;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlDbType"></param>
        /// <returns></returns>
        public static bool IsQuoteRequiredForSqlDbType(SqlDbType sqlDbType)
        {
            var isQuoteRequired = sqlDbType == SqlDbType.Char ||
                                  sqlDbType == SqlDbType.VarChar ||
                                  sqlDbType == SqlDbType.NVarChar ||
                                  sqlDbType == SqlDbType.Text || 
                                  sqlDbType == SqlDbType.NText;
            return isQuoteRequired;
        }

        /// <summary>
        /// Executes for XML document.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <returns></returns>
        public static XmlDocument ExecuteForXmlDocument<T>(SqlConnection connection, SqlTransaction transaction, List<DbParameter> parameters, string sqlCommandText)
        {
            using (var cmd = new SqlCommand(sqlCommandText))
            {
                parameters?.ForEach(p => cmd.Parameters.Add(new SqlParameter(p.ParameterName, DbTypeToSqlDbType[p.DbType], p.Size) { Value = p.Value }));
                cmd.Connection = connection;
                cmd.Transaction = transaction;
                var xmlDoc = new XmlDocument();
                using (var reader = cmd.ExecuteXmlReader())
                {
                    xmlDoc.Load(reader);
                    return xmlDoc;
                }
            }
        }

        /// <summary>
        /// Executes for json document.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <returns></returns>
        public static JsonDocument ExecuteForJSONDocument(SqlConnection connection, SqlTransaction transaction,
            List<DbParameter> parameters, string sqlCommandText)
        {
            using (var cmd = new SqlCommand(sqlCommandText))
            {
                parameters?.ForEach(p => cmd.Parameters.Add(new SqlParameter(p.ParameterName,DbTypeToSqlDbType[p.DbType], p.Size){Value = p.Value}));
                cmd.Connection = connection;
                cmd.Transaction = transaction;
                using (var reader = cmd.ExecuteReader())
                {
                    var jsonResult = new StringBuilder();
                    if (!reader.HasRows)
                    {
                        jsonResult.Append("[]");
                        return JsonDocument.Parse(jsonResult.ToString());
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            jsonResult.Append(reader.GetValue(0).ToString());
                        }
                        var jsonText = jsonResult.ToString();
                        return JsonDocument.Parse(jsonText);
                    }
                }
            }
        }

        /// <summary>
        /// Executes for json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <returns></returns>
        public static List<T> ExecuteForJSON<T>(SqlConnection connection, SqlTransaction transaction,List<DbParameter> parameters, string sqlCommandText)
        {
            using (var cmd = new SqlCommand(sqlCommandText))
            {
                parameters?.ForEach(p => cmd.Parameters.Add(new SqlParameter(p.ParameterName,DbTypeToSqlDbType[p.DbType], p.Size){Value = p.Value}));
                cmd.Connection = connection;
                cmd.Transaction = transaction;
                using (var reader = cmd.ExecuteReader())
                {
                    var jsonResult = new StringBuilder();
                    if (!reader.HasRows)
                    {
                        jsonResult.Append("[]");
                        return new List<T>();
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            jsonResult.Append(reader.GetValue(0).ToString());
                        }

                        var retVal = DataContractSerializerHelper.DataContractJSONDeserializer<List<T>>(jsonResult.ToString());
                        return retVal;
                    }
                }
            }
        }



        //https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-data-type-mappings
        //https://gist.github.com/Romoku/5444566        

        /// <summary>
        /// The database type to SQL database type
        /// </summary>
        public static readonly ReadOnlyDictionary<DbType, SqlDbType> DbTypeToSqlDbType = 
            new ReadOnlyDictionary<DbType, SqlDbType>(
            new Dictionary<DbType, SqlDbType>()
        {
                {DbType.AnsiString, SqlDbType.VarChar}
            , {DbType.Binary, SqlDbType.Binary}
            ,{DbType.Byte, SqlDbType.TinyInt}
            ,{DbType.Boolean, SqlDbType.Bit}
            
            ,{DbType.Currency , SqlDbType.Money}
            ,{DbType.Date  , SqlDbType.Date}
            ,{DbType.DateTime   , SqlDbType.DateTime}
            ,{DbType.Decimal    , SqlDbType.Decimal}
            ,{DbType.Double     , SqlDbType.Float}
            ,{DbType.Guid      , SqlDbType.UniqueIdentifier}
            
            ,{DbType.Int16    , SqlDbType.SmallInt}
            ,{DbType.Int32     , SqlDbType.Int}
            ,{DbType.Int64       , SqlDbType.BigInt}
            
            ,{DbType.Object     , SqlDbType.Variant}
            ,{DbType.SByte      , SqlDbType.VarBinary}
            ,{DbType.Single      , SqlDbType.Real}
            ,{DbType.String        , SqlDbType.NVarChar}
            
            ,{DbType.Time      , SqlDbType.Time}
            ,{DbType.UInt16       , SqlDbType.SmallInt}
            ,{DbType.UInt32       , SqlDbType.Int}
            ,{DbType.UInt64         , SqlDbType.BigInt}
            
            ,{DbType.AnsiStringFixedLength       , SqlDbType.Char}
            ,{DbType.StringFixedLength        , SqlDbType.NChar}
            ,{DbType.Xml          , SqlDbType.Xml}
            
            ,{DbType.DateTime2         , SqlDbType.DateTime2}
            ,{DbType.DateTimeOffset           , SqlDbType.DateTimeOffset}
            
        });

        /// <summary>
        /// The SQL database type to database type
        /// </summary>
        public static readonly ReadOnlyDictionary<SqlDbType, DbType> SqlDbTypeToDbType =
            new ReadOnlyDictionary<SqlDbType, DbType>(
                DBUtility.DbTypeToSqlDbType.ToLookup(c => c.Value)
                    .ToDictionary(k => k.Key, kv => kv.First().Key)
            );

        /// <summary>
        /// Executes the non query command.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <returns></returns>
        public static int ExecuteNonQueryCommand(SqlConnection connection, SqlTransaction transaction,List<DbParameter> parameters, string sqlCommandText)
        {
            using (var cmd = new SqlCommand(sqlCommandText))
            {
                parameters?.ForEach(p => cmd.Parameters.Add(new SqlParameter(p.ParameterName,DbTypeToSqlDbType[p.DbType], p.Size){Value = p.Value}));
                cmd.Connection = connection;
                cmd.Transaction = transaction;
                var recordsAffected = cmd.ExecuteNonQuery();
                return recordsAffected;
            }
        }

        /// <summary>
        /// Deserializes the data contract object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        public static T DeserializeDataContractObject<T>(this XmlReader reader)
        {
            var serializer = new DataContractSerializer(typeof(T));
            return (T)serializer.ReadObject(reader);
        }

        /// <summary>
        /// Executes the stored proc.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="timeOutInSeconds">The time out in seconds.</param>
        /// <param name="mInfo">The m information.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">argument {nameof(connection)} is null</exception>
        public static ResultSet ExecuteStoredProc(SqlConnection connection, SqlTransaction transaction, int timeOutInSeconds, MethodInfo mInfo, params object[] arguments)
        {
            if(connection == null) throw new ArgumentException($"argument {nameof(connection)} is null ");
            List<Type> types = Enumerable.ToList<Type>(Enumerable.Select<ResultTypeAttribute, Type>(
                (IEnumerable<ResultTypeAttribute>) Attribute.GetCustomAttributes((MemberInfo) mInfo,
                    typeof(ResultTypeAttribute)), (Func<ResultTypeAttribute, Type>) (d => d.Type)));
            using (SqlCommand command = CreateCommand(mInfo, arguments))
            {
                command.Connection = connection;
                command.CommandTimeout = timeOutInSeconds;
                ResultSet resultSet;
                if (types.Count == 0)
                {
                    if (transaction != null)
                    {
                        command.Transaction = transaction;
                    }

                    object obj = command.Transaction != null 
                        ? command.ExecuteScalar()
                        : command.ExecuteScalar();
                    resultSet = new ResultSet((List<object>) null, command);
                    if (resultSet.ReturnValueObject == null)
                        resultSet.ReturnValueObject = obj;
                }
                else
                {
                    if (transaction != null)
                        command.Transaction = transaction;
                    resultSet = ExecuteSPCommand(command, types);
                }

                return resultSet;
            }
        }

        private static ResultSet ExecuteSPCommand(SqlCommand cmd, List<Type> types)
        {
            DataSet dataSet;

            if (cmd.Transaction == null)
            {
                dataSet = SqlCommandExtensions.ExecuteDatasetWithRetry(cmd, (DataSet) null, (SqlDataAdapter) null);
            }
            else
            {
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
                dataSet = new DataSet();
                ((DataAdapter) sqlDataAdapter).Fill(dataSet);
            }

            if (dataSet.Tables.Count == 0)
                return new ResultSet((List<object>) null, cmd);
            List<object> objectList = new List<object>();
            for (int tableIndex = 0; tableIndex < dataSet.Tables.Count; ++tableIndex)
            {
                Tuple<int, PropertyInfo, MDRXColumnAttribute>[] propAttrInfo = null;
                Type recordType;
                if (types.Count == 1)
                {
                    recordType = types[0];
                    propAttrInfo = GetAllPropertiesAndAttributes(types[0]);
                }
                else
                {
                    var typePopertyTypeTuple = GetTypeForTableData(types, dataSet, tableIndex);
                    if (typePopertyTypeTuple != null)
                    {
                        recordType = typePopertyTypeTuple.Item1;
                        propAttrInfo = typePopertyTypeTuple.Item2;
                    }
                    else
                    {
                        recordType = null;
                        propAttrInfo = null;
                    }
                }

                if (!(recordType == (Type) null) && propAttrInfo != null)
                    objectList.AddRange((IEnumerable<object>) CreateObjectList(propAttrInfo, recordType,
                        dataSet.Tables[tableIndex], cmd.CommandText));
            }

            return new ResultSet(objectList, cmd);
        }

        private static readonly Func<Type, Tuple<int, PropertyInfo, MDRXColumnAttribute>[]> GetAllPropertiesAndAttributes
            = Memonize<Type, Tuple<int, PropertyInfo, MDRXColumnAttribute>[]>(
                (func, type) =>
                {
                    PropertyInfo[] propertyInfos =
                        type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    List<Tuple<int, PropertyInfo, MDRXColumnAttribute>> list =
                        new List<Tuple<int, PropertyInfo, MDRXColumnAttribute>>();
                    int index = 0;
                    foreach (PropertyInfo propertyInfo in propertyInfos)
                    {
                        MDRXColumnAttribute customAttribute =
                            (MDRXColumnAttribute) Attribute.GetCustomAttribute((MemberInfo) propertyInfo,
                                typeof(MDRXColumnAttribute), false);
                        if (customAttribute != null)
                        {
                            list.Add(new Tuple<int, PropertyInfo, MDRXColumnAttribute>(index, propertyInfo,
                                customAttribute));
                            index = index + 1;
                        }
                    }

                    return list.OrderBy(i => i.Item1).ToArray();
                });

        private static Tuple<Type, Tuple<int, PropertyInfo, MDRXColumnAttribute>[]> GetTypeForTableData(List<Type> types,
            DataSet ds, int tableIndex)
        {
            Tuple<Type, Tuple<int, PropertyInfo, MDRXColumnAttribute>[]> retVal = null;
            if (ds.Tables[tableIndex].Columns.Count > 0)
            {
                List<string> list = new List<string>();
                for (int index = 0; index < ds.Tables[tableIndex].Columns.Count; ++index)
                    list.Add(ds.Tables[tableIndex].Columns[index].ColumnName);
                for (int index = 0; index < types.Count; ++index)
                {
                    if (GetAllPropertiesAndAttributes(types[index])
                        .Select(t => t.Item3).Where<MDRXColumnAttribute>((c => c != null))
                        .Select((Func<MDRXColumnAttribute, string>) (c => c.Name))
                        .Intersect<string>((IEnumerable<string>) list).Count() == list.Count)
                    {
                        var type = types[index];
                        var propAttrInfo = GetAllPropertiesAndAttributes(types[index]);
                        retVal = new Tuple<Type, Tuple<int, PropertyInfo, MDRXColumnAttribute>[]>(type, propAttrInfo);
                        break;
                    }
                }
            }

            return retVal;
        }

        private static List<object> CreateObjectList(Tuple<int, PropertyInfo, MDRXColumnAttribute>[] props,
            Type recordType, DataTable dataTable, string storedProcName)
        {
            List<object> list1 = new List<object>();
            if (dataTable.Rows.Count == 0)
                return list1;
            ConstructorInfo constructorInfo = recordType.GetConstructors().First();
            //DynamicMethodHelper.CreateObject objectFactory = DynamicMethodHelper.CreateObjectFactory(recordType);
            storedProcName = storedProcName + recordType.Name;
            List<PropertyInfo> list2 = MappedResultSetForDataSet(storedProcName, dataTable, props);
            int count = dataTable.Columns.Count;
            //DynamicMethodHelper.PropertySetHandler[] propertySetHandlerArray =
            //    new DynamicMethodHelper.PropertySetHandler[count];
            Type[] typeArray = new Type[count];
            bool[] flagArray1 = new bool[count];
            bool[] flagArray2 = new bool[count];
            Type type1 = typeof(Nullable<>);
            for (int index = 0; index < count; ++index)
            {
                if (!(list2[index] == (PropertyInfo) null))
                {
                    //propertySetHandlerArray[index] = DynamicMethodHelper.GetPropertySetter(list2[index]);
                    typeArray[index] = list2[index].PropertyType;
                    if (dataTable.Columns[index].DataType != typeArray[index] && typeArray[index].IsGenericType)
                    {
                        Type type2 = typeArray[index].GetGenericArguments()[0];
                        Type type3 = type1.MakeGenericType(new Type[1]
                        {
                            type2
                        });
                        if (typeArray[index] == type3)
                        {
                            typeArray[index] = type2;
                            flagArray1[index] = true;
                        }
                    }

                    if (!flagArray1[index])
                        flagArray1[index] = !typeArray[index].IsValueType;
                    flagArray2[index] = typeArray[index].IsEnum;
                }
            }

              
            

            foreach (DataRow dataRow in (InternalDataCollectionBase) dataTable.Rows)
            {
                object target = constructorInfo?.Invoke(null);// objectFactory();
                if (target is IMDRXCoreEntity)
                    ((IMDRXCoreEntity) target).Delete();
                for (int index = 0; index < count; ++index)
                {
                    if (!(list2[index] == (PropertyInfo) null))
                    {
                        
                        //DynamicMethodHelper.PropertySetHandler propertySetHandler = propertySetHandlerArray[index];
                        object parameter1 = dataRow[index];
                        if (parameter1 == DBNull.Value)
                        {
                            //if (propertySetHandler == null)
                            {
                                list2[index].SetValue(target, (object) null, (object[]) null);
                            }
                            //else
                            //{
                            //    object parameter2 = flagArray1[index]
                            //        ? (object) null
                            //        : Activator.CreateInstance(typeArray[index]);
                            //    propertySetHandler(target, parameter2);
                            //}
                        }
                        else if (typeArray[index] == parameter1.GetType())
                        {
                            //if (propertySetHandler == null)
                                list2[index].SetValue(target, parameter1, (object[]) null);
                            //else
                            //    propertySetHandler(target, parameter1);
                        }
                        else
                        {
                            try
                            {
                                if (!flagArray2[index])
                                {
                                    parameter1 = Convert.ChangeType(parameter1, typeArray[index],
                                        (IFormatProvider) CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    switch (Type.GetTypeCode(parameter1.GetType()))
                                    {
                                        case TypeCode.Int16:
                                        case TypeCode.Int32:
                                        case TypeCode.Int64:
                                            break;
                                        default:
                                            parameter1 = Enum.Parse(typeArray[index], parameter1.ToString());
                                            break;
                                    }
                                }
                            }
                            catch (InvalidCastException ex1)
                            {
                                Debug.WriteLine(ex1.Message);
                                try
                                {
                                    MethodInfo method = typeArray[index].GetMethod("Parse",
                                        BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                                        (Binder) null, new Type[1]
                                        {
                                            typeof(string)
                                        }, (ParameterModifier[]) null);
                                    if (method != (MethodInfo) null)
                                        parameter1 = method.Invoke((object) null, new object[1]
                                        {
                                            (object) parameter1.ToString()
                                        });
                                }
                                catch (TargetInvocationException ex2)
                                {
                                    throw new PropertyMismatchException(
                                        string.Format("Property :{0} not found or mismatch in Entity : {1}.",
                                            (object) parameter1.GetType().ToString(),
                                            (object) recordType.GetType().ToString()), ex2.InnerException);
                                }
                            }

                            //if (propertySetHandler == null)
                                list2[index].SetValue(target, parameter1, (object[]) null);
                            //else
                                //propertySetHandler(target, parameter1);
                        }
                    }
                }

                if (target is IMDRXCoreEntity)
                    ((IMDRXCoreEntity) target).DataEntityState = DomainEntityState.Unchanged;
                list1.Add(target);
            }

            return list1;
        }

        private static readonly ConcurrentDictionary<string, List<PropertyInfo>> CachePropertySetForResultSet =
            new ConcurrentDictionary<string, List<PropertyInfo>>();

        private static object syncLock = new object();

        private static List<PropertyInfo> MappedResultSetForDataSet(string storedProcName, DataTable dataTable,
            Tuple<int, PropertyInfo, MDRXColumnAttribute>[] props)
        {
            List<PropertyInfo> pInfo = (List<PropertyInfo>) null;

            CachePropertySetForResultSet.TryGetValue(storedProcName, out pInfo);
            if (pInfo != null && !IsColumnPropertyMatch(pInfo, dataTable))
            {
                List<PropertyInfo> list;
                CachePropertySetForResultSet.TryRemove(storedProcName, out list);
                pInfo = (List<PropertyInfo>) null;
            }

            if (pInfo == null)
            {
                lock (syncLock)
                {
                    if (CachePropertySetForResultSet.ContainsKey(storedProcName))
                        CachePropertySetForResultSet.TryGetValue(storedProcName, out pInfo);
                    if (pInfo == null)
                        pInfo = new List<PropertyInfo>();
                    List<string> matchingPropertyNameColumnList = new List<string>();
                    foreach (DataColumn column in (InternalDataCollectionBase) dataTable.Columns)
                    {
                        bool isPropertyNameColumnNameMatchFound = false;
                        string columnNameInUpperCase = column.ColumnName.ToUpper();
                        for (int index = 0; index < props.GetLength(0); ++index)
                        {
                            var propertyInfo =  props[index].Item2;
                            string propertyNameInUpperCase = propertyInfo.Name.ToUpper();
                            if (!matchingPropertyNameColumnList.Contains(propertyNameInUpperCase))
                            {
                                if (columnNameInUpperCase.Equals(propertyNameInUpperCase))
                                {
                                    pInfo.Add(propertyInfo);
                                    matchingPropertyNameColumnList.Add(propertyNameInUpperCase);
                                    isPropertyNameColumnNameMatchFound = true;
                                    break;
                                }
                                else
                                {
                                    MDRXColumnAttribute columnAttribute =  props[index].Item3;
                                    if (columnAttribute != null && !string.IsNullOrEmpty(columnAttribute.Name) &&
                                        columnNameInUpperCase.Equals(columnAttribute.Name.ToUpper()))
                                    {
                                        pInfo.Add(propertyInfo);
                                        matchingPropertyNameColumnList.Add(propertyNameInUpperCase);
                                        isPropertyNameColumnNameMatchFound = true;
                                        break;
                                    }
                                }
                            }
                        }

                        if (!isPropertyNameColumnNameMatchFound)
                            pInfo.Add((PropertyInfo) null);
                    }

                    matchingPropertyNameColumnList.Clear();
                    if (CachePropertySetForResultSet != null)
                    {
                        if (!CachePropertySetForResultSet.ContainsKey(storedProcName))
                            CachePropertySetForResultSet.TryAdd(storedProcName, pInfo);
                    }
                }
            }

            return pInfo;
        }
         
        private static bool IsColumnPropertyMatch(List<PropertyInfo> pInfo, DataTable table)
        {
            bool flag = true;
            if (pInfo.Count < table.Columns.Count)
            {
                flag = false;
            }
            else
            {
                for (int index = 0; index < table.Columns.Count; ++index)
                {
                    if (pInfo[index] != (PropertyInfo) null &&
                        string.Compare(table.Columns[index].ColumnName, pInfo[index].Name, true) != 0)
                    {
                        flag = false;
                        break;
                    }
                }
            }

            return flag;
        }

        /// <summary>
        /// Gets the context attached property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="record">The record.</param>
        /// <param name="propertyInfo">The property information.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public static string GetContextAttachedPropertyValue<T>(T record,PropertyInfo[] propertyInfo , string propertyName) where T : IMDRXCoreEntity
         {
             string columnValue = string.Empty;
             if (string.IsNullOrEmpty(propertyName) == false)
             {
                 var contextAttachedValue = propertyInfo.FirstOrDefault(pr => pr.Name == propertyName)?.GetValue(record)?.ToString();
                 if (contextAttachedValue != null)
                 {
                     columnValue = string.Format("({0})", contextAttachedValue);
                 }
             }
             return columnValue;
         }

        /// <summary>
        /// The default labelfunc
        /// </summary>
        public static readonly Func<ChangeTrackingInfo, string> DEFAULT_LABELFUNC = (info) => info.PropertyName;

        /// <summary>
        /// Gets the parent path adv.
        /// </summary>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static Queue<ParentRecordInfo> GetParentPathAdv(ORMModelMetaInfo modelMetaInfo, object entity)
         {
             Queue<ParentRecordInfo> GetParentPathInternal( object localEntity, Queue<ParentRecordInfo> rootObjectPath)
             {
                 var parentPropertyInfoList = modelMetaInfo.GetEntityRefPropertyInfo(localEntity.GetType());
                 if (parentPropertyInfoList == null || parentPropertyInfoList.Any() == false) return rootObjectPath;
                 foreach (var parentPropertyInfo in parentPropertyInfoList)
                 {
                     var parentPropertyNamePointingToChild =
                         modelMetaInfo.GetChildPropertyInfo(parentPropertyInfo.GetType(), localEntity.GetType());
                     var parentClassName = parentPropertyInfo.GetType().Name;
                     var parentClassType = parentPropertyInfo.GetType();
                     var parentTableName = modelMetaInfo.GetTableNameByType(parentPropertyInfo.GetType());

                     var parentEntity = parentPropertyInfo.GetValue(localEntity);
                     var primaryPropertyInfoList = modelMetaInfo.GetPrimaryKeyPropertyInfoList(parentEntity.GetType());
                
                     var parentRecordInfo = new ParentRecordInfo(parentClassType,parentClassName,parentTableName,parentPropertyNamePointingToChild.Name,primaryPropertyInfoList,parentEntity);
                     rootObjectPath.Enqueue(parentRecordInfo);    
                     GetParentPathInternal(parentEntity, rootObjectPath);
                 }
                 return rootObjectPath;
             }
             return GetParentPathInternal(entity, new Queue<ParentRecordInfo>());
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