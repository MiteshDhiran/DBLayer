

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.Dal.Generator.Common
{
    /// <summary>
    /// SQLMetadataProcessor
    /// </summary>
    public static class SQLMetadataProcessor
    {
        /// <summary>
        /// Gets the table data contract meta information.
        /// </summary>
        /// <param name="dataContextGenerationParams">The data context generation parameters.</param>
        /// <param name="parentChildInputList">The parent child input list.</param>
        /// <param name="dataContractNameSpace">The data contract name space.</param>
        /// <returns></returns>
        public static List<TableDataContractMetaInfo> GetTableDataContractMetaInfo(DataContextGeneratorParams dataContextGenerationParams,  List<ParentChildTableInfo> parentChildInputList, string dataContractNameSpace)
        {
            string connectionString = dataContextGenerationParams.ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var commandText = ReadCommandTextResource(); //File.ReadAllText(@"C:\VSTS\Sandbox1\mdhiran\CoreDAL\Utility\TableInfoCmd.txt");
            var inputCommandHeader = "DECLARE @xmltableInfo nvarchar(max);" +
                                    "SET @xmltableInfo = N";

            var parentChildXML = parentChildInputList.Aggregate(new StringBuilder(), (sb, pc) => string.IsNullOrEmpty(pc.ChildTableName) == false ?
            sb.AppendLine($"<T ParentTableName=\"{pc.ParentTableName}\" ChildTableName=\"{pc.ChildTableName}\"/> ")
            : sb.AppendLine($"<T ParentTableName=\"{pc.ParentTableName}\" /> ")
            ).ToString();

            var inputCommand = inputCommandHeader + "'<ROOT>" + parentChildXML + "</ROOT>'";
            var completeCommand = inputCommand + "\r\n" + commandText;
            //completeCommand.Dump();
            SqlCommand cmd = new SqlCommand(completeCommand);
            cmd.Connection = connection;
            DataSet dataSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd);
            sqlDataAdapter.Fill(dataSet);
            //dataSet.Dump();
            var allTableUniqueConstraintInfo = dataSet.Tables[2].AsEnumerable().Select(x =>
                    new NonPrimaryUniqueIndexRecordInfo(
                    
                        x.Field<string>("IndexName"), x.Field<string>("TableName"),
                        x.Field<byte>("ColumnOrder"), x.Field<string>("ColumnName")
                    )).ToList();

            
            var nonPrimaryUniqueDictionaryByTableAndIndexName =allTableUniqueConstraintInfo
                .ToLookup(x => x.TableName)
                    .ToDictionary(x => x.Key
                    , xv => xv.Select(xx => xx).ToList()
                        .ToLookup(xx => xx.IndexName)
                        .ToDictionary(xx => xx.Key, xxv => xxv.OrderBy(xx => xx.ColumnOrder).Select(xxxv => xxxv.ColumnName).ToList()) );
            
                
            var allTableNames = dataSet.Tables[0].AsEnumerable().Select(x => x.Field<string>("ChildTableName")).Distinct().ToList().Union(dataSet.Tables[0].AsEnumerable().Select(x => x.Field<string>("ParentTableName")).Distinct().ToList()).Distinct().ToList();
            

            var fkTableInfoList = dataSet.Tables[0].AsEnumerable().Select(t => new
                ParentChildInfoFromDB(
                     t.Field<string>("ParentTableName")
                    ,t.Field<string>("ChildTableName")
                    ,t.Field<string>("FKName")
                    ,t.Field<string>("ParentTableFKColumnNames")
                    ,t.Field<string>("ChildTableFKColumnNames")
                    , parentChildInputList.FirstOrDefault(f => f.ChildTableName == t.Field<string>("ChildTableName") && f.ParentTableName == t.Field<string>("ParentTableName"))?.EntitySetPropertyName 
                    ,parentChildInputList.FirstOrDefault(f => f.ChildTableName == t.Field<string>("ChildTableName") && f.ParentTableName == t.Field<string>("ParentTableName"))?.EntityRefPropertyName 
                     
                    )
                ).ToList();


            //get the FKName based on ParentChildTableTuple
           var dicOfParentChildWithFKNameFromConfig = parentChildInputList.Where(k =>
                    string.IsNullOrEmpty(k.ParentTableName) == false &&
                    string.IsNullOrEmpty(k.ChildTableName) == false )
                .ToLookup(k =>  (k.ParentTableName + "_" + k.ChildTableName).ToLower())
                .ToDictionary(kv => kv.Key, kvg => kvg.First());

           var dicOfParentChildFromDB = fkTableInfoList
               .Where(t => string.IsNullOrEmpty(t.ParentTableName) == false &&
                           string.IsNullOrEmpty(t.ChildTableName) == false)
               .ToLookup(c => (c.ParentTableName + "_" + c.ChildTableName).ToLower())
               .ToDictionary(k => k.Key, kv => kv.ToList());

           var keysFromConfig = dicOfParentChildWithFKNameFromConfig.Keys.ToList();
           var keysFromDb = dicOfParentChildFromDB.Keys.ToList();
           var nonMatchingKeys = keysFromConfig.Except(keysFromDb).ToList();
           
           var bestParentChildInfoFromDbList = dicOfParentChildWithFKNameFromConfig
               .Select(d => GetBestParentChild(dicOfParentChildFromDB, d.Key, d.Value.FKName)).ToList();
            
            
            var parentInfoDictionary = bestParentChildInfoFromDbList.ToLookup(til => til.ChildTableName).ToDictionary(k => k.Key, v => v.ToLookup(til => til.FKName).ToDictionary(k => k.Key, v1 => v1.Select(id => 
                new ParentPropertyMetaInfo(dataContractNameSpace, id.ParentTableName,dataContractNameSpace ,id.ChildTableName ,id.ChildTableFKColumnNames.Split(',').ToList(), id.ParentTableFKColumnNames.Split(',').ToList()
                    ,id.EntityRefPropertyName ?? id.ParentTableName
                    ,id.EntitySetPropertyName ?? id.ChildTableName 
                     , id.ParentTableName
                     )
            ).First()));


            var tableNameDic = parentChildInputList.ToLookup(t => t.ParentTableName)
                 .ToDictionary(k => k.Key, kv => kv.First(), StringComparer.InvariantCultureIgnoreCase);//, StringComparer.OrdinalIgnoreCase);
            
            var childInfoDictionary = bestParentChildInfoFromDbList.ToLookup(til => til.ParentTableName)
                                .ToDictionary(til => til.Key
                                    , v => v.ToLookup(x => x.FKName)
                                                .ToDictionary(k => k.Key,
                                                                    v1 => v1.Select(id => new ChildPropertyMetaInfo(id.EntitySetPropertyName ?? id.ChildTableName, $"List<{id.ChildTableName}>"
                                                                    , id.FKName, id.ChildTableFKColumnNames.Split(',').ToList(), id.ParentTableFKColumnNames.Split(',').ToList()
                                                                    , id.EntityRefPropertyName ?? id.ParentTableName, dataContractNameSpace,id.ChildTableName,id.ChildTableName, id.ParentTableName
                                                                    )).ToList())
                                            );

            var tableDataTableList = dataSet.Tables[1].AsEnumerable().Select(t => new TableDataTable(
            t.Field<string>("TABLE_NAME"),
            t.Field<string>("COLUMN_NAME"),
            t.Field<string>("DATA_TYPE")
            , t.Field<string>("Column_default")
            , t.Field<int?>("character_maximum_length")
            , t.Field<byte?>("numeric_precision")
            , t.Field<int?>("numeric_scale")
            , t.Field<Int16?>("DATETIME_PRECISION")
            , t.Field<string>("is_nullable") != "NO"
            , t.Field<string>("KeyType") == "PRIMARY KEY"
            , t.Field<int?>("PKColumnOrdinalPostion")
            , t.Field<int>("IsIdentity") == 1
            )).ToList();
            
            var tableDic = tableDataTableList.ToLookup(dtl => dtl.TableName).ToDictionary(k => k.Key, kv => kv.ToList());
            var r = tableDic.Select(d => new
            {
                TableName = d.Key
                ,
                ChildParentChildTableInfoList = d.Value
                ,
                CurrentParentChildTableInfo = tableNameDic[d.Key]
            }).ToList();
            var tableDataContractList = new List<TableDataContractMetaInfo>();
            foreach( var d in r )
            {
                var primaryKeyColumns = d.ChildParentChildTableInfoList.Where(t => t.IsPKKey).OrderBy(t => t.PKColumnOrdinalPosition).Select(t => t.ColumnName).ToList();
                var parentPropertyMetaInfoNew = parentInfoDictionary.TryGetValue(d.TableName,out var parentPropertyMetaInfo) ? parentPropertyMetaInfo.First().Value : null;
                var childPropertiesList = d.ChildParentChildTableInfoList.Select(v => new ColumnPropertyMetaInfo(v.ColumnName
                                       ,v.ColumnName
                                       ,null
                                       ,v.IsNullable ? MappingInfo.ConvertToNullableDataType(MappingInfo.SQLDataTypeDic[v.SqlDataType]) : MappingInfo.SQLDataTypeDic[v.SqlDataType],v.SqlDataType
                                       ,v.IsIdentity
                                       ,v.SqlDataType == "timestamp" ? true : false
                                       ,v.SqlColumnDefaultValue
                                       ,v.IsNullable
                                       ,v.IsPKKey
                                       ,v.PKColumnOrdinalPosition
                                       ,v.StringMaxLength
                                       ,v.IsIdentity || (v.SqlDataType == "timestamp" ? true : false) || dataContextGenerationParams.AlwaysDBGeneratedFieldList.Contains(v.ColumnName)
                                       ,(v.SqlDataType == "timestamp" ? true : false) || dataContextGenerationParams.AlwaysDBGeneratedFieldList.Contains(v.ColumnName)
                                       ,tableNameDic.TryGetValue(d.TableName,out var parentChildTableInfo3) ? parentChildTableInfo3.ColumnExtraInfoList.FirstOrDefault(c => c.ColumnName == v.ColumnName) : null
                                       )
                                    ).ToList();
                var childEntityPropertyInfoList = childInfoDictionary.TryGetValue(d.TableName,out var childEntitySetPropertiesDictionary) ? childEntitySetPropertiesDictionary.SelectMany(espd => espd.Value.Select(v => v)).ToList() : null;
                var ignoreColumnList = Utility.CreateArrayFromCommaSeparatedString(string.IsNullOrEmpty(parentChildInputList.First(p => p.ParentTableName == d.TableName).IgnoreFields) ? dataContextGenerationParams.IgnoreFields : parentChildInputList.First(p => p.ParentTableName == d.TableName).IgnoreFields);
                var alwaysDbGeneratedList = Utility.CreateArrayFromCommaSeparatedString(string.IsNullOrEmpty(parentChildInputList.First(p => p.ParentTableName == d.TableName).AlwaysDBGeneratedFields) ? dataContextGenerationParams.AlwaysDBGeneratedFields : parentChildInputList.First(p => p.ParentTableName == d.TableName).AlwaysDBGeneratedFields);
                var nonPrimaryUniqueConstraint = nonPrimaryUniqueDictionaryByTableAndIndexName.ContainsKey(d.TableName) ? nonPrimaryUniqueDictionaryByTableAndIndexName[d.TableName] : new Dictionary<string,List<string>>();

                var td = new TableDataContractMetaInfo(
                                    d.CurrentParentChildTableInfo.DomainName
                                    ,d.TableName
                                    ,dataContractNameSpace
                                    ,d.TableName
                                    ,primaryKeyColumns
                                    ,parentPropertyMetaInfoNew
                                    ,childPropertiesList
                                    ,childEntityPropertyInfoList
                                    ,ignoreColumnList
                                    ,alwaysDbGeneratedList
                                    ,nonPrimaryUniqueConstraint
                                    ,d.CurrentParentChildTableInfo.PrimaryIDLookupValueRecordInfo
                                    ,d.CurrentParentChildTableInfo.ResolveLookUps
                                    ,d.CurrentParentChildTableInfo.ClassNameWithResolvedProperties
                                    );
                tableDataContractList.Add(td);

            }
            //.Select(
            //            d => new TableDataContractMetaInfo(
            //                        d.CurrentParentChildTableInfo.DomainName
            //                        , d.TableName
            //                        , dataContractNameSpace
            //                        , d.TableName
            //                        , d.ChildParentChildTableInfoList.Where(t => t.IsPKKey).OrderBy(t => t.PKColumnOrdinalPosition).Select(t => t.ColumnName).ToList()
            //                        , parentInfoDictionary.TryGetValue(d.TableName, out var parentPropertyMetaInfo) ? parentPropertyMetaInfo.First().Value : null
            //                        , d.ChildParentChildTableInfoList.Select(v => new ColumnPropertyMetaInfo(v.ColumnName
            //                            , v.ColumnName
            //                            ,null
            //                            , v.IsNullable? MappingInfo.ConvertToNullableDataType(MappingInfo.SQLDataTypeDic[v.SqlDataType] ) : MappingInfo.SQLDataTypeDic[v.SqlDataType], v.SqlDataType
            //                            , v.IsIdentity
            //                            , v.SqlDataType == "timestamp" ? true : false
            //                            , v.SqlColumnDefaultValue
            //                            , v.IsNullable
            //                            , v.IsPKKey
            //                            , v.PKColumnOrdinalPosition
            //                            , v.StringMaxLength
            //                            ,v.IsIdentity || (v.SqlDataType == "timestamp" ? true : false) || dataContextGenerationParams.AlwaysDBGeneratedFieldList.Contains(v.ColumnName)
            //                            ,(v.SqlDataType == "timestamp" ? true : false ) || dataContextGenerationParams.AlwaysDBGeneratedFieldList.Contains(v.ColumnName)
            //                            , tableNameDic.TryGetValue(d.TableName, out var parentChildTableInfo3) ? parentChildTableInfo3.ColumnExtraInfoList.FirstOrDefault(c => c.ColumnName == v.ColumnName) : null
            //                            )
            //                        ).ToList()
            //                        , childInfoDictionary.TryGetValue(d.TableName, out var childEntitySetPropertiesDictionary) ? childEntitySetPropertiesDictionary.SelectMany(espd => espd.Value.Select(v => v)).ToList() : null
            //                        , Utility.CreateArrayFromCommaSeparatedString(string.IsNullOrEmpty(parentChildInputList.First(p => p.ParentTableName == d.TableName).IgnoreFields) ? dataContextGenerationParams.IgnoreFields : parentChildInputList.First(p => p.ParentTableName == d.TableName).IgnoreFields)
            //                        , Utility.CreateArrayFromCommaSeparatedString(string.IsNullOrEmpty(parentChildInputList.First(p => p.ParentTableName == d.TableName).AlwaysDBGeneratedFields) ? dataContextGenerationParams.AlwaysDBGeneratedFields : parentChildInputList.First(p => p.ParentTableName == d.TableName).AlwaysDBGeneratedFields)
            //                        , nonPrimaryUniqueDictionaryByTableAndIndexName.ContainsKey(d.TableName) ? nonPrimaryUniqueDictionaryByTableAndIndexName[d.TableName]: new Dictionary<string,List<string>>()
            //                        , d.CurrentParentChildTableInfo.PrimaryIDLookupValueRecordInfo 
            //                        , d.CurrentParentChildTableInfo.ResolveLookUps
            //                        , d.CurrentParentChildTableInfo.ClassNameWithResolvedProperties
            //                        )
            //    ).ToList();
            // return r;
            return tableDataContractList;

        }
        /// <summary>
        /// Gets the best parent child.
        /// </summary>
        /// <param name="dicOfParentChildFromDB">The dic of parent child from database.</param>
        /// <param name="parentChildName">Name of the parent child.</param>
        /// <param name="fkName">Name of the fk.</param>
        /// <returns></returns>
        public static ParentChildInfoFromDB GetBestParentChild(Dictionary<string,List<ParentChildInfoFromDB>> dicOfParentChildFromDB,string parentChildName, string fkName)
        {
            var list = dicOfParentChildFromDB[parentChildName];
            if (list.Count == 1)
            {
                return list.First();
            }
            else
            {
                return list.FirstOrDefault(l =>
                    string.Compare(l.FKName, fkName, StringComparison.InvariantCultureIgnoreCase) == 0) ?? list.First();
            }
        }
        
        private static string ReadCommandTextResource()
        {
            var assembly = typeof(MappingInfo).Assembly;
            using (var stream = assembly.GetManifestResourceStream("ConnectAndSell.Dal.Generator.Common.TableInfoCmd.txt"))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException("Could not load manifest resource stream.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// Gets the sp metadata.
        /// </summary>
        /// <param name="spCommandTextAlongWithParameterValue">The sp command text along with parameter value.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static DataSet GetSPMetadata(string spCommandTextAlongWithParameterValue, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var commandText = $"SET NOCOUNT ON;SET FMTONLY ON;exec  {spCommandTextAlongWithParameterValue}; SET FMTONLY OFF; ";
                var cmd = new SqlCommand(commandText) {Connection = connection};
                var dataSet = new DataSet();
                var sqlDataAdapter = new SqlDataAdapter(cmd);
                sqlDataAdapter.Fill(dataSet);
                return dataSet;
            }

        }
        /// <summary>
        /// Gets the sp input parameter information.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="spName">Name of the sp.</param>
        /// <param name="resultSetMetaInfo">The result set meta information.</param>
        /// <param name="dalHelperName">Name of the dal helper.</param>
        /// <returns></returns>
        public static SPMetaInfoForCodeGeneration GetSPInputParameterInfo(string connectionString, string spName, List<SPResultSetMetaInfo> resultSetMetaInfo, string dalHelperName)
        {
            
            var sqlDataAdapter = new SqlDataAdapter(
                $"SELECT sc.id, so.name procedureName,sc.name paramName,ST.NAME dbtype,sc.length length, sc.status, sc.isoutparam, sc.isnullable, sc.xprec as precision,sc.xscale as scale FROM sysobjects SO LEFT JOIN syscolumns sc ON sc.id = so.id LEFT JOIN systypes st ON sc.xusertype = st.xusertype  aND st.name not in ('sql_variant', 'sysname') WHERE SO.TYPE = 'P' and so.name = '{spName}'", new SqlConnection(connectionString));
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            var retVal = new SPMetaInfoForCodeGeneration(spName,dataTable,resultSetMetaInfo, dalHelperName);
            return retVal;
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