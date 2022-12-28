

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using Microsoft.Data.SqlClient;
//


namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    /// <summary>
    /// DBBulkInsertUtility
    /// </summary>
    public static class DBBulkInsertUtility
    {
        //const string ColumnInfoSelectStatement = @"SELECT Name,column_id FROM sys.columns WHERE object_id IN (SELECT type_table_object_id FROM sys.table_types WHERE name = @p1) ORDER BY column_id";
        const string ParentRecordIDColumnName = "PRID";
        const string RecordIDColumnName = "RID";
        static readonly Func<ORMModelMetaInfo,Type, Type, string> GetInputTableArg = (modelInfo,currentType, parentType) =>
            $"@{(parentType == null ? modelInfo.GetBulkInsertUDTTableTypeName(currentType) : modelInfo.GetTableNameByType(parentType) + "_" + modelInfo.GetBulkInsertUDTTableTypeName(currentType))}InputArg";
      
        private static readonly SortedSet<string> DBSysColumns = new SortedSet<string>() { "CreatedBy", "CreatedWhenUTC", "TouchedBy", "TouchedWhenUTC" };


        
        /// <summary>
        /// Fills the DataTable with the data from <paramref name="records"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metaModelIfo"></param>
        /// <param name="records"></param>
        /// <param name="userName"></param>
        /// <param name="dicTablesArg"></param>
        /// <param name="parentType"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private static Dictionary<Tuple<Type, Type>, Tuple<DataTable, List<object>>> FillRecordsInDataTable<T>(ORMModelMetaInfo metaModelIfo, List<T> records, string userName, Dictionary<Tuple<Type, Type>, Tuple<DataTable, List<object>>> dicTablesArg, Type parentType, long? parentID)
        {
            var dicTables = dicTablesArg ?? new Dictionary<Tuple<Type, Type>, Tuple<DataTable, List<object>>>();
            if (records == null || records.Any() == false) return dicTables;
            var type = records[0].GetType();
            var list = metaModelIfo.GetTableColumnInfoForBulkInsert(type);
            var tuple = new Tuple<Type, Type>(type, parentType);
            var dtTuple = dicTables.ContainsKey(tuple) ? dicTables[tuple] : new Tuple<DataTable, List<object>>(metaModelIfo.GetDataTableForBulkInsert(tuple.Item1), new List<object>());
            if (dicTables.ContainsKey(tuple) == false)
            {
                dicTables.Add(tuple, dtTuple);
            }
            var dt = dtTuple.Item1;
            var listOfObject = dtTuple.Item2;
            var lastCounter = listOfObject.Count;
            var insertedColumnList = list.Where(l => l.IsAllowedForInsert == true).ToList();
            var recordSeq = lastCounter;
            foreach (var rec in records)
            {
                var dr = dt.NewRow();
                insertedColumnList.ForEach(ci =>
                {
                    var propertyValue = ci.PropertyInfo.GetValue(rec);
                    dr[ci.ColumnName] = propertyValue == null ? DBNull.Value : (ci.ConvertFromDataContractToDataTableTypeFunc != null ? ci.ConvertFromDataContractToDataTableTypeFunc(propertyValue) ?? DBNull.Value : propertyValue);
                });

                var currentDateTimeOffset = DateTimeOffset.UtcNow;

                dr["TouchedBy"] = userName;
                dr["CreatedBy"] = userName;
                dr["CreatedWhenUTC"] = currentDateTimeOffset;
                dr["TouchedWhenUTC"] = currentDateTimeOffset;
                if (parentID != null)
                {
                    dr[ParentRecordIDColumnName] = parentID.Value;
                }
                dr[RecordIDColumnName] = recordSeq;
                dt.Rows.Add(dr);
                listOfObject.Add(rec);
                //FOR EACH CHILD TABLE MAKE A CALL TO FillRecordsInDataTable
                metaModelIfo.GetAllTableEntitySetProperties(type).Select(c => c.PropertyInfo).ToList().ForEach(cpi =>
                {
                    FillRecordsInDataTable(metaModelIfo, cpi.GetValue(rec) != null ? ((IList)cpi.GetValue(rec)).Cast<object>().ToList() : new List<object>(), userName, dicTables, type, recordSeq);
                });
                recordSeq++;

            }
            return dicTables;
        }

        private static readonly Func<ORMModelMetaInfo, Type, List<Tuple<IColumnNetInfo, int>>> GetTheAutoSyncProperties 
            = (metaModelInfo, type) => 
                metaModelInfo.GetTableColumnInfoForBulkInsert(type)
                    .Where(p => p.SyncRequired == true || DBSysColumns.Contains(p.ColumnName, StringComparer.InvariantCultureIgnoreCase))
                    .Select(p => new Tuple<IColumnNetInfo, int>(p.PropertyInfo, p.ColumnPosition))
                    .ToList();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="traceInfo"></param>
        /// <param name="connection"></param>
        /// <param name="transaction"></param>
        /// <param name="metaModelInfo"></param>
        /// <param name="records"></param>
        /// <param name="typeHierarchyList"></param>
        /// <param name="insertTSQL"></param>
        /// <returns></returns>
        private static bool InsertRecordsInBulkSingleDBCallInternal<T>(TraceInfo traceInfo, SqlConnection connection, SqlTransaction transaction, ORMModelMetaInfo metaModelInfo, List<T> records,List<Tuple<Type, Type>> typeHierarchyList, string insertTSQL) where T : IMDRXCoreEntity
        {
            //fill the data in data tables for each table
            var dataTableDic = FillRecordsInDataTable(metaModelInfo, records, traceInfo.UserName, null, null, null);
            //create sql parameter for each table .. whether it have value or not
            var parameters = typeHierarchyList.Select(t => new SqlParameter(GetInputTableArg(metaModelInfo,t.Item1, t.Item2), SqlDbType.Structured)
            {
                Value = dataTableDic.ContainsKey(t) ? dataTableDic[t].Item1 : null,
                TypeName = metaModelInfo.GetBulkInsertUDTTableTypeName(t.Item1)
            }).ToList();


            //call the bulk insert sql
            using (var cmd = new SqlCommand(insertTSQL) { Connection = connection, CommandTimeout = 0, Transaction = transaction })
            {
                parameters.ForEach(p => cmd.Parameters.Add(p));
                using (var reader = cmd.ExecuteReader())
                {
                    typeHierarchyList.ForEach((tuple) =>
                    {
                        if (dataTableDic.ContainsKey(tuple))
                        {
                            var autoSyncProperties = GetTheAutoSyncProperties(metaModelInfo, tuple.Item1);
                            var listOfObjects = dataTableDic[tuple].Item2;
                            foreach (var rec in listOfObjects)
                            {
                                reader.Read();
                                autoSyncProperties.ForEach(pc =>
                                {
                                    pc.Item1.SetValue(rec,Utility.GetSqlToNetConvertedValueFunc(pc.Item1, reader.GetValue(pc.Item2)));
                                });
                            }
                        }
                        reader.NextResult();
                    });
                    reader.Close();
                }
            }
            
            foreach (var k in dataTableDic)
            {
                 k.Value.Item2.ForEach(p =>
                 {
                     if (p is IMDRXCoreEntity platformBase)
                     {
                         platformBase.DataEntityState = DomainEntityState.Unchanged;
                     }
                 });   
            }
            return true;
        }
        
        private static void PerformChangeTracking<T>(ORMModelMetaInfo metaModelInfo, List<T> records, Dictionary<object, List<ChangeTrackingInfo>> agg)
        {
            foreach (var record in records)
            {
                var currentEntityState = DomainEntityState.New;
                var currentTableName = record.GetType().Name;
                var allParentPath = DBUtility.GetParentPathAdv(metaModelInfo, record); //GetParentPath(dbContext, entityEntry);
                var primaryPropertyInfoList = metaModelInfo.GetAllTableColumnProperties(record.GetType())
                    .Where(c => c.IsPrimaryKey).Select(c => c.PropertyInfo).ToList();
                var allProperties = metaModelInfo.GetAllTableColumnProperties(record.GetType());
                var rootRecordInfo = allParentPath.Count == 0
                    ? new RootRecordInfo(record.GetType(), currentTableName,primaryPropertyInfoList,record)
                    : new RootRecordInfo(allParentPath.Last().ParentClassType,
                        allParentPath.Last().ParentTableName, allParentPath.Last().PrimaryKeyPropertyInfos, allParentPath.Last().ParentObject);
                var entityPropertiesOldValueNewValue = allProperties.Aggregate(new List<ChangeTrackingInfo>(), (list, property) =>
                {
                    list.Add(new ChangeTrackingInfo(rootRecordInfo,allParentPath,record,primaryPropertyInfoList,currentEntityState,record.GetType(),currentTableName,property.PropertyName,"", property.PropertyInfo.GetValue(record)));
                    return list;
                });
                agg.Add(record, entityPropertiesOldValueNewValue);
            }
        }

        /// <summary>
        /// Bulks the insert.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="traceInfo">The trace information.</param>
        /// <param name="connection">The connection.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="metaModelInfo">The meta model information.</param>
        /// <param name="isChangeTrackingRequired">if set to <c>true</c> [is change tracking required].</param>
        /// <param name="records">The records.</param>
        /// <returns></returns>
        /// <exception cref="Exception">All records should have object status New for bulk insert API</exception>
        public static Tuple<Dictionary<object, List<ChangeTrackingInfo>>,bool> BulkInsert<T>(TraceInfo traceInfo, SqlConnection connection, SqlTransaction transaction, ORMModelMetaInfo metaModelInfo,bool isChangeTrackingRequired, List<T> records) where T : IMDRXCoreEntity
        {
            Dictionary<object, List<ChangeTrackingInfo>> trackingDictionary = new Dictionary<object, List<ChangeTrackingInfo>>();
            if (records == null || records.Any() == false) return new Tuple<Dictionary<object, List<ChangeTrackingInfo>>, bool>(trackingDictionary, true);
            var areAllRecordsNew = records.TrueForAll(r => (r.DataEntityState == DomainEntityState.New));
            if (areAllRecordsNew != true)
            {
                throw new Exception("All records should have object status New for bulk insert API");
            }

            var sql = metaModelInfo.GetBulkInsertSQLForType(records[0].GetType()); //GetBulkInsertSQLForTypeInternal(traceInfo, connection, transaction, metaModelInfo, records[0].GetType());
            var typeHierarchyTuple = metaModelInfo.GetNestedChildTableType(records[0].GetType());
            records.PerformActionInBatch(100, (recs) => InsertRecordsInBulkSingleDBCallInternal(traceInfo,connection,transaction,metaModelInfo, recs,typeHierarchyTuple, sql));
            if (isChangeTrackingRequired)
            {
                PerformChangeTracking(metaModelInfo, records, trackingDictionary);
            }
            return new Tuple<Dictionary<object, List<ChangeTrackingInfo>>, bool>(trackingDictionary, true);
        }

        /// <summary>
        /// Performs the action in batch.
        /// </summary>
        /// <typeparam name="TI">The type of the i.</typeparam>
        /// <param name="inputList">The input list.</param>
        /// <param name="batchSize">Size of the batch.</param>
        /// <param name="action">The action.</param>
        /// <exception cref="ArgumentOutOfRangeException">Batch Size cannot be zero</exception>
        public static void PerformActionInBatch<TI>(this List<TI> inputList, int batchSize, Action<List<TI>> action)
        {
            if (inputList == null) return ;
            if (batchSize == 0) throw new ArgumentOutOfRangeException("Batch Size cannot be zero");
            inputList.GetBatch(batchSize).ToList().ForEach(action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="inputList"></param>
        /// <param name="batchSize"></param>
        /// <returns></returns>
        public static IEnumerable<List<T>> GetBatch<T>(this List<T> inputList, int batchSize)
        {
            if (inputList == null || inputList.Any() == false) yield return new List<T>(new List<T>());
            if (batchSize == 0) throw new ArgumentOutOfRangeException("Batch Size cannot be zero");
            if (inputList != null)
            {
                int totalRecords = inputList.Count();
                int remainder = totalRecords % batchSize;
                int roughPageNumbers = totalRecords / batchSize;
                int totalPages = remainder > 0 ? roughPageNumbers + 1 : roughPageNumbers;
                for (int i = 0; i < totalPages; i++)
                {
                    yield return inputList.Skip(i * batchSize).Take(batchSize).ToList();
                } 
            }
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