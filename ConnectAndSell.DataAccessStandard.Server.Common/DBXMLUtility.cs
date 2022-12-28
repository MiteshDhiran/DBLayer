

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DBXMLUtility
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelMetaInfo"></param>
        /// <param name="mainTableClrType"></param>
        /// <param name="primaryKeyValuesRecords"></param>
        /// <returns></returns>
        public static string GetTSQLForXMLBasedOnPrimaryKeyBasicValues<T>(ORMModelMetaInfo modelMetaInfo,Type mainTableClrType, List<T> primaryKeyValuesRecords) where T: struct
        {
            var primaryKeyColumnNames = modelMetaInfo.GetTableDataContractMetaInfoOfType(mainTableClrType).PrimaryKeyPropertiesInfoList
                .Select(t => t.Name).ToList();
            var allFilterString = primaryKeyValuesRecords.Aggregate(new StringBuilder(), (sb,rec) =>
            {
                var filterRec = primaryKeyColumnNames.Aggregate(new StringBuilder(), (sbi, primaryKeyProp) => sbi.AppendLine($"{primaryKeyProp}=\"{rec}\"")).ToString();
                sb.Append($"<FR {filterRec}/>");
                return sb;
            }).ToString();
            var pkTableName = $"#{typeof(T).Name}PKValues";
            var inputFilterArgValue =  $"'<ROOT>{allFilterString}</ROOT>'";
            return GetPrimaryKeyFilterTSQLFunc(
                new Tuple<ORMModelMetaInfo, Type, Type>(modelMetaInfo, mainTableClrType, typeof(T)))(inputFilterArgValue);
        }

        public static string GetTSQLForXMLBasedOnPrimaryKeyWithPartitionColumn<TK>(ORMModelMetaInfo modelMetaInfo,List<Tuple<long, int>> list) where TK : class
        {
            var inputFilterArgValue = GetFilterXMLArgValue<TK>(modelMetaInfo, list);
            return GetPrimaryKeyFilterJustByTableTypeTSQLFunc(new Tuple<ORMModelMetaInfo, Type>(modelMetaInfo, typeof(TK)))(inputFilterArgValue);
        }

        public static string GetFilterXMLArgValue<TK>(ORMModelMetaInfo modelMetaInfo,List<Tuple<long, int>> list)
        {
            var primaryKeyColumnInfos = modelMetaInfo.GetTableDataContractMetaInfoOfType(typeof(TK)).PrimaryKeyPropertiesInfoList;
            var allFilterString = list.Aggregate(new System.Text.StringBuilder(), (sb,rec) =>
            {
                var filterRec = primaryKeyColumnInfos.Aggregate(new System.Text.StringBuilder(), (sbi, primaryKeyProp) =>
                {
                    var valueForColumn = primaryKeyProp.PropertyType == typeof(long) ? rec.Item1 : rec.Item2;
                    return sbi.AppendLine($"{primaryKeyProp}=\"{valueForColumn}\"");
                }).ToString();
                sb.Append($"<FR {filterRec}/>");
                return sb;
            }).ToString();
            var inputFilterArgValue =  $"'<ROOT>{allFilterString}</ROOT>'";
            return inputFilterArgValue;
        }

        public static string GetFilterXMLFromDataTable<TK>(ORMModelMetaInfo modelMetaInfo,DataTable filterDT)
        {
            var allProperties = modelMetaInfo.GetAllTableColumnProperties(typeof(TK));
            //Check whether datatable columns are whats in table
            foreach (DataColumn col in filterDT.Columns)
            {
                var colProp = allProperties.FirstOrDefault(p => string.Equals(p.PropertyName, col.ColumnName, StringComparison.InvariantCultureIgnoreCase));
                if (colProp == null)
                {
                    throw new ArgumentException($"Column:{col.ColumnName} does not exists in table type:{typeof(TK).FullName}");
                }
            }
            var sb = new StringBuilder();
            foreach (DataRow row in filterDT.Rows)
            {
                var filterRec = new StringBuilder();    
                foreach (DataColumn col in filterDT.Columns)
                {
                    //var colProp = allProperties.First(p => string.Equals(p.PropertyName, col.ColumnName, StringComparison.InvariantCultureIgnoreCase));
                    var valueForColumn = row[col];
                    filterRec.AppendLine($"{col.ColumnName}=\"{valueForColumn}\"");
                } 
                sb.Append($"<FR {filterRec}/>");
            }

            var allFilterString = sb.ToString();
            var inputFilterArgValue =  $"'<ROOT>{allFilterString}</ROOT>'";
            return inputFilterArgValue;
        }


        /// <summary>
        /// Gets the TSQL query for XML result set.
        /// </summary>
        /// <param name="tableClrType">Type of the table color.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="joinClause">The join clause.</param>
        /// <returns></returns>
        public static string GetTSQLQueryForXmlResultSet(Type tableClrType, ORMModelMetaInfo modelMetaInfo, string whereClause, string? joinClause = null)
        {
            var tsql = modelMetaInfo.GetTSQLByXMLForDataContract(tableClrType);
            var tableName = modelMetaInfo.GetTableNameByType(tableClrType);
            var str = (
                $"{tsql} {joinClause ?? ""} {(string.IsNullOrEmpty(whereClause) == false ? " WHERE " + whereClause : string.Empty)} FOR XML PATH('{tableName}'), ROOT('ArrayOf{tableName}')"
            );
            return str;
        }

        /// <summary>
        /// Gets the TSQL query for json result set.
        /// </summary>
        /// <param name="tableClrType">Type of the table color.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="joinClause">The join clause.</param>
        /// <returns></returns>
        public static string GetTSQLQueryForJSONResultSet(Type tableClrType, ORMModelMetaInfo modelMetaInfo, string whereClause, string? joinClause = null)
        {
            var tsql = modelMetaInfo.GetTSQLByJSONForDataContract(tableClrType);
            var tableName = modelMetaInfo.GetTableNameByType(tableClrType);
            var str = (
                $"{tsql} {joinClause ?? ""} {(string.IsNullOrEmpty(whereClause) == false ? " WHERE " + whereClause : string.Empty)} FOR JSON PATH "
            );
            return str;
        }


        private static readonly Func<Tuple<ORMModelMetaInfo,Type,Type>, Func<string, string>> GetPrimaryKeyFilterTSQLFunc =
            DBUtility.Memonize<Tuple<ORMModelMetaInfo,Type,Type>, Func<string, string>>((_, tuple) =>
                GenerateXMLFilterCriteriaForPKValuesInternal(tuple));

        private static readonly Func<Tuple<ORMModelMetaInfo,Type>, Func<string, string>> GetPrimaryKeyFilterJustByTableTypeTSQLFunc =
            DBUtility.Memonize<Tuple<ORMModelMetaInfo,Type>, Func<string, string>>((_, tuple) =>
                GenerateXMLFilterCriteriaForPartitionPKValuesInternal(tuple));
        
        private static readonly Func<Tuple<ORMModelMetaInfo,Type,Type>, Func<string, string>> GetPrimaryKeyFilterTSQLJSONFunc =
            DBUtility.Memonize<Tuple<ORMModelMetaInfo,Type,Type>, Func<string, string>>((_, tuple) =>
                GenerateJSONFilterCriteriaForPKValuesInternal(tuple));

        private static Func<string, string> GenerateXMLFilterCriteriaForPartitionPKValuesInternal(
            Tuple<ORMModelMetaInfo, Type> mainTableAndFilterTypeTuple)
        {
            return (allFilterString) =>
            {
                var mainTableType = mainTableAndFilterTypeTuple.Item2;
                var modelMetaInfo = mainTableAndFilterTypeTuple.Item1;
                var mainTableName = mainTableAndFilterTypeTuple.Item1.GetTableNameByType(mainTableAndFilterTypeTuple.Item2);
                var primaryKeyProperties = modelMetaInfo.GetPrimaryKeyPropertyInfoList(mainTableType);
                var pkTableName = $"#{mainTableType}FilterPKValues";
                var inputFilterArgValue = $"{allFilterString}";
                var sbAll = new StringBuilder();
                sbAll.AppendLine(
                    $"DECLARE @primaryKeyFilterXML XML;\r\n DECLARE @idoc INT; \r\n SET @primaryKeyFilterXML = {inputFilterArgValue}; \r\n EXEC sp_xml_preparedocument @idoc OUTPUT, @primaryKeyFilterXML; ");
                var tempTableColumnDeclaration = string.Join(",",
                    primaryKeyProperties
                        .Select((prop) => $" {prop.Name} {GetSqlTypeStringBasedOnClrType(prop.PropertyType)} ")
                        .ToList());
                var onlyTempTableColumnNames =
                    string.Join(",", primaryKeyProperties.Select((prop) => $"{prop.Name}").ToList());
                sbAll.AppendLine($"CREATE TABLE {pkTableName} ({tempTableColumnDeclaration}) ;");
                var tempTableInsertStatementFromXML =
                    $" INSERT INTO {pkTableName}({onlyTempTableColumnNames}) \r\n SELECT {onlyTempTableColumnNames} \r\n FROM OPENXML(@idoc,'/ROOT/FR',1) \r\n WITH ({tempTableColumnDeclaration})";
                sbAll.AppendLine(tempTableInsertStatementFromXML);
                var joinColumnConstraint =
                    string.Join(" AND ", primaryKeyProperties.Select(p => $"{mainTableName}.{p.Name} = {pkTableName}.{p.Name}").ToList());
                var joinClause = $" INNER JOIN {pkTableName} ON {joinColumnConstraint}";
                var tsql = GetTSQLQueryForXmlResultSet(mainTableType,modelMetaInfo,"",joinClause);
                var retVal = $"{sbAll} \r\n ;{tsql}";
                return retVal;
            };
        }

        //GenerateJSONFilterCriteriaForPKValuesInternal
        private static Func<string,string> GenerateXMLFilterCriteriaForPKValuesInternal(Tuple<ORMModelMetaInfo,Type,Type> mainTableAndFilterTypeTuple)
        {
            return (allFilterString) =>
            {
                var mainTableType = mainTableAndFilterTypeTuple.Item2;
                var modelMetaInfo = mainTableAndFilterTypeTuple.Item1;
                var mainTableName = mainTableAndFilterTypeTuple.Item1.GetTableNameByType(mainTableAndFilterTypeTuple.Item2);
                var filterClrType = mainTableAndFilterTypeTuple.Item3;
                var primaryKeyProperties = filterClrType.GetProperties().ToList();
                var pkTableName = $"#{filterClrType.Name}PKValues";
                var inputFilterArgValue = $"{allFilterString}";
                var sbAll = new StringBuilder();
                sbAll.AppendLine(
                    $"DECLARE @primaryKeyFilterXML XML;\r\n DECLARE @idoc INT; \r\n SET @primaryKeyFilterXML = {inputFilterArgValue}; \r\n EXEC sp_xml_preparedocument @idoc OUTPUT, @primaryKeyFilterXML; ");
                var tempTableColumnDeclaration = string.Join(",",
                    primaryKeyProperties
                        .Select((prop) => $" {prop.Name} {GetSqlTypeStringBasedOnClrType(prop.PropertyType)} ")
                        .ToList());
                var onlyTempTableColumnNames =
                    string.Join(",", primaryKeyProperties.Select((prop) => $"{prop.Name}").ToList());
                sbAll.AppendLine($"CREATE TABLE {pkTableName} ({tempTableColumnDeclaration}) ;");
                var tempTableInsertStatementFromXML =
                    $" INSERT INTO {pkTableName}({onlyTempTableColumnNames}) \r\n SELECT {onlyTempTableColumnNames} \r\n FROM OPENXML(@idoc,'/ROOT/FR',1) \r\n WITH ({tempTableColumnDeclaration})";
                sbAll.AppendLine(tempTableInsertStatementFromXML);
                var joinColumnConstraint =
                    string.Join(" AND ", primaryKeyProperties.Select(p => $"{mainTableName}.{p.Name} = {pkTableName}.{p.Name}").ToList());
                var joinClause = $" INNER JOIN {pkTableName} ON {joinColumnConstraint}";
                var tsql = GetTSQLQueryForXmlResultSet(mainTableType,modelMetaInfo,"",joinClause);
                var retVal = $"{sbAll} \r\n ;{tsql}";
                return retVal;
            };
        }
        
        private static Func<string,string> GenerateJSONFilterCriteriaForPKValuesInternal(Tuple<ORMModelMetaInfo,Type,Type> mainTableAndFilterTypeTuple)
        {
            return (allFilterString) =>
            {
                var mainTableType = mainTableAndFilterTypeTuple.Item2;
                var modelMetaInfo = mainTableAndFilterTypeTuple.Item1;
                var mainTableName = mainTableAndFilterTypeTuple.Item1.GetTableNameByType(mainTableAndFilterTypeTuple.Item2);
                var filterClrType = mainTableAndFilterTypeTuple.Item3;
                var primaryKeyProperties = filterClrType.GetProperties().ToList();
                var pkTableName = $"#{filterClrType.Name}PKValues";
                var inputFilterArgValue = $"{allFilterString}";
                var sbAll = new StringBuilder();
                sbAll.AppendLine(
                    $"DECLARE @primaryKeyFilterXML XML;\r\n DECLARE @idoc INT; \r\n SET @primaryKeyFilterXML = {inputFilterArgValue}; \r\n EXEC sp_xml_preparedocument @idoc OUTPUT, @primaryKeyFilterXML; ");
                var tempTableColumnDeclaration = string.Join(",",
                    primaryKeyProperties
                        .Select((prop) => $" {prop.Name} {GetSqlTypeStringBasedOnClrType(prop.PropertyType)} ")
                        .ToList());
                var onlyTempTableColumnNames =
                    string.Join(",", primaryKeyProperties.Select((prop) => $"{prop.Name}").ToList());
                sbAll.AppendLine($"CREATE TABLE {pkTableName} ({tempTableColumnDeclaration}) ;");
                var tempTableInsertStatementFromXML =
                    $" INSERT INTO {pkTableName}({onlyTempTableColumnNames}) \r\n SELECT {onlyTempTableColumnNames} \r\n FROM OPENXML(@idoc,'/ROOT/FR',1) \r\n WITH ({tempTableColumnDeclaration})";
                sbAll.AppendLine(tempTableInsertStatementFromXML);
                var joinColumnConstraint =
                    string.Join(" AND ", primaryKeyProperties.Select(p => $"{mainTableName}.{p.Name} = {pkTableName}.{p.Name}").ToList());
                var joinClause = $" INNER JOIN {pkTableName} ON {joinColumnConstraint}";
                var tsql = GetTSQLQueryForJSONResultSet(mainTableType,modelMetaInfo,"",joinClause);
                var retVal = $"{sbAll} \r\n ;{tsql}";
                return retVal;
            };
        }

        /// <summary>
        /// Generates the XML filter criteria for pk values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="mainTableClrType">Type of the main table color.</param>
        /// <param name="primaryKeyValuesRecords">The primary key values records.</param>
        /// <returns></returns>
        public static string GenerateXMLFilterCriteriaForPKValues<T>(ORMModelMetaInfo modelMetaInfo,Type mainTableClrType, List<T> primaryKeyValuesRecords)
        {
            var primaryKeyProperties = typeof(T).GetProperties().ToList();
            var allFilterString = primaryKeyValuesRecords.Aggregate(new StringBuilder(), (sb,rec) =>
            {
                var filterRec = primaryKeyProperties.Aggregate(new StringBuilder(), (sbi, primaryKeyProp) => sbi.AppendLine($"{primaryKeyProp.Name}=\"{primaryKeyProp.GetValue(rec)}\"")).ToString();
                sb.Append($"<FR {filterRec}/>");
                return sb;
            }).ToString();
            var pkTableName = $"#{typeof(T).Name}PKValues";
            var inputFilterArgValue =  $"'<ROOT>{allFilterString}</ROOT>'";
            return GetPrimaryKeyFilterTSQLFunc(
                new Tuple<ORMModelMetaInfo, Type, Type>(modelMetaInfo, mainTableClrType, typeof(T)))(inputFilterArgValue);
        }

        /// <summary>
        /// Generates the json filter criteria for pk values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="mainTableClrType">Type of the main table color.</param>
        /// <param name="primaryKeyValuesRecords">The primary key values records.</param>
        /// <returns></returns>
        public static string GenerateJSONFilterCriteriaForPKValues<T>(ORMModelMetaInfo modelMetaInfo,Type mainTableClrType, List<T> primaryKeyValuesRecords)
        {
            var primaryKeyProperties = typeof(T).GetProperties().ToList();
            var allFilterString = primaryKeyValuesRecords.Aggregate(new StringBuilder(), (sb,rec) =>
            {
                var filterRec = primaryKeyProperties.Aggregate(new StringBuilder(), (sbi, primaryKeyProp) => sbi.AppendLine($"{primaryKeyProp.Name}=\"{primaryKeyProp.GetValue(rec)}\"")).ToString();
                sb.Append($"<FR {filterRec}/>");
                return sb;
            }).ToString();
            var pkTableName = $"#{typeof(T).Name}PKValues";
            var inputFilterArgValue =  $"'<ROOT>{allFilterString}</ROOT>'";
            return GetPrimaryKeyFilterTSQLJSONFunc(
                new Tuple<ORMModelMetaInfo, Type, Type>(modelMetaInfo, mainTableClrType, typeof(T)))(inputFilterArgValue);
        }

        /// <summary>
        /// Gets the type of the SQL type string based on color.
        /// </summary>
        /// <param name="clrType">Type of the color.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Mapping between CLR Type:{clrType.Name} and Sql Type does not exists</exception>
        public static string GetSqlTypeStringBasedOnClrType(Type clrType)
        {
            return ClrTypeSqlTypeConcurrentDictionary.TryGetValue(clrType, out var retVal)
                ? retVal
                : throw new InvalidOperationException(
                    $"Mapping between CLR Type:{clrType.Name} and Sql Type does not exists");
        }

        /// <summary>
        /// The color type SQL type concurrent dictionary
        /// </summary>
        public static ConcurrentDictionary<Type,string> ClrTypeSqlTypeConcurrentDictionary = new ConcurrentDictionary<Type, string>(new Dictionary<Type,string>()
        {
            {typeof(long), "BIGINT"}  
            ,{typeof(int), "INT" }
        });

        /// <summary>
        /// Gets the TSQL for data contract.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <returns></returns>
        public static string GetTSQLForDataContract(Type type, ORMModelMetaInfo modelMetaInfo)
        {
            StringBuilder FuncRec(Type tableDataContractType, bool isRootTableContract, StringBuilder sbQuery,HashSet<Type> fetchedType, Type parentTableType, Dictionary<Type,List<Type>> parentChildDic)
            {
                if (parentChildDic.ContainsKey(parentTableType) == false)
                {
                    //modelMetaInfo.GetTableNameByType(type)
                    parentChildDic.Add(parentTableType, new List<Type>() {tableDataContractType});
                }
                else if (parentChildDic[parentTableType].Contains(tableDataContractType) == false)
                {
                    parentChildDic[parentTableType].Add(tableDataContractType);
                }
                else
                {
                    var circularDetail =
                        $"RootTable: {type.Name} Child Table:{tableDataContractType.Name} is already considered under parent table {parentTableType.Name}";
                    //Console.WriteLine(circularDetail);
                    //Debug.WriteLine(circularDetail);
                    throw  new ApplicationException(circularDetail);
                    
                }
                if (fetchedType.Contains(tableDataContractType))
                {
                    Debug.WriteLine("Circular reference");
                }

                fetchedType.Add(tableDataContractType);
                var tableName = modelMetaInfo.GetTableNameByType(tableDataContractType);
                var childProperties = modelMetaInfo.GetAllTableColumnProperties(tableDataContractType).ToList();
                var childEntitySetPropertyNamesWithoutFilter =   modelMetaInfo.GetAllTableEntitySetProperties(tableDataContractType).ToList();

                var childEntitySetPropertyNames = childEntitySetPropertyNamesWithoutFilter;

                //var childEntitySetPropertyNames = childEntitySetPropertyNamesWithoutFilter.Where(c => parentChildDic.ContainsKey(tableDataContractType) == false ||
                //   parentChildDic[tableDataContractType].Contains(c.EntitySetTableType) == false).ToList();

                //var childEntitySetPropertyNames =  childEntitySetPropertyNamesWithoutFilter.Where(c =>
                //    parentChildDic[parentTableType].Contains(tableDataContractType) == false).ToList();
                //var childEntitySetPropertyNames = childEntitySetPropertyNamesWithoutFilter.Where(c =>
                //        c.EntitySetTableName.Equals(tableName, StringComparison.InvariantCultureIgnoreCase) == false)
                //    .ToList();
                if (childEntitySetPropertyNamesWithoutFilter.Count != childEntitySetPropertyNames.Count)
                {
                    var circularReferenceDetails = childEntitySetPropertyNamesWithoutFilter.Where(c =>
                        parentChildDic[tableDataContractType].Contains(c.EntitySetTableType) == true).ToList();
                    var message = $"Circular reference found for table {tableName}. ChildTableName = {string.Join(",", circularReferenceDetails.Select(x => x.EntitySetTableName))}";
                    //Debug.WriteLine(message);
                    //Console.WriteLine(message);
                }
                var sortedNodes = childProperties.Select(c => new
                    {
                        NodeType = "C",
                        Name = $"{tableName}.{c.ColumnName}",
                        PropertyName = c.PropertyName,
                        DbType = c.DbType,
                        ChildTableName = String.Empty,
                        ThisColumns = (List<string>) null,
                        ChildColumns = (List<string>) null,
                        ChildType = (Type) null
                    })
                    .Concat(childEntitySetPropertyNames.Select(c => new
                    {
                        NodeType = "N",
                        Name = c.PropertyName.EndsWith("_CL", StringComparison.CurrentCultureIgnoreCase) ? c.PropertyName.Replace("_CL","List"):c.PropertyName,
                        PropertyName = c.PropertyName.EndsWith("_CL", StringComparison.CurrentCultureIgnoreCase) ? c.PropertyName.Replace("_CL","List"):c.PropertyName,
                        DbType = "",
                        ChildTableName = c.EntitySetTableName,
                        ThisColumns = c.ParentTableColumnNames,
                        ChildColumns = c.ChildTableColumnNames,
                        ChildType = c.EntitySetTableType
                    }))
                    .OrderBy(c => c.PropertyName, StringComparer.Ordinal)
                    .ToList();
                if (isRootTableContract) sbQuery.Append($"WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/System' as ns1, DEFAULT 'http://schemas.datacontract.org/2004/07/{type.Namespace}')");
                var isFirst = true;
                foreach (var node in sortedNodes) //iterate over the columns and child tables and make it part of select statement
                {
                    if (node.NodeType == "C" || (parentChildDic.ContainsKey(tableDataContractType) == false || parentChildDic[tableDataContractType].Contains(node.ChildType) == false))//avoid circular calls
                    {
                        sbQuery.Append(isFirst == false ? " , " : " SELECT ");
                        isFirst = false;
                        if (node.NodeType == "C")
                        {
                            if (string.IsNullOrEmpty(node.DbType) == false &&
                                node.DbType.ToLower().Contains("xml") == true)
                            {
                                sbQuery.Append($"CONVERT(varchar(max),{node.Name}) AS {node.PropertyName} ");
                            }
                            else if (string.IsNullOrEmpty(node.DbType) == false &&
                                     node.DbType.ToLower().Contains("datetimeoffset") == false)
                            {
                                sbQuery.Append(node.Name + " AS " + node.PropertyName + " ");
                            }
                            else
                            {
                                var datetimeSelect =
                                    $"CONVERT(nvarchar(30),{node.Name},127) AS '{node.PropertyName}/ns1:DateTime',DATEPART (TZoffset, {node.Name}) AS '{node.PropertyName}/ns1:OffsetMinutes'";
                                sbQuery.Append(datetimeSelect);
                            }
                        }
                        else //for child table
                        {
                            sbQuery.Append(" ( ");
                            FuncRec(node.ChildType, false, sbQuery, fetchedType, tableDataContractType,
                                parentChildDic); //recursive call for child table
                            sbQuery.Append(
                                $" FROM {node.ChildTableName} WHERE {node.ChildColumns.Zip<string, string, string>(node.ThisColumns, (f, s) => string.Format("{0}.{1}={2}.{3}", node.ChildTableName, s, tableDataContractType.Name, f)).ToList().ConcatenateListOfStringUsingSeparator(" AND ")} FOR XML PATH ('{node.ChildTableName}'), TYPE) AS '{node.Name}' ");
                        }
                    }
                }
                
                if (isRootTableContract == true)
                {
                    sbQuery.Append(" FROM " + tableDataContractType.Name);
                }

                return sbQuery;
            }

            return FuncRec(type, true, new StringBuilder(), new HashSet<Type>(),type,new Dictionary<Type, List<Type>>()).ToString();
        }

        #region "Selected Columns"

        /// <summary>
        /// Gets the TSQL selected columns query for XML result set.
        /// </summary>
        /// <param name="tableClrType">Type of the table color.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <returns></returns>
        public static string GetTSQLSelectedColumnsQueryForXmlResultSet(Type tableClrType, ORMModelMetaInfo modelMetaInfo, List<PropertyInfo> selectedColumns)
        {
            var tsql = modelMetaInfo.GetTSQLByXMLForSelectedColumnsNonCached(tableClrType, selectedColumns);
            var tableName = modelMetaInfo.GetTableNameByType(tableClrType);
            var str = (
                $"{tsql} FOR XML PATH('{tableName}'), ROOT('ArrayOf{tableName}')"
            );
            return str;
        }

        /// <summary>
        /// Gets the TSQL selected columns query for XML result set.
        /// </summary>
        /// <param name="tableClrType">Type of the table color.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns></returns>
        public static string GetTSQLSelectedColumnsQueryForXmlResultSet(Type tableClrType, ORMModelMetaInfo modelMetaInfo, List<PropertyInfo> selectedColumns, string whereClause)
        {
            var tsql = modelMetaInfo.GetTSQLByXMLForSelectedColumnsNonCached(tableClrType, selectedColumns);
            var tableName = modelMetaInfo.GetTableNameByType(tableClrType);

            var str = (
                $"{tsql} {(string.IsNullOrEmpty(whereClause) == false ? " WHERE " + whereClause : string.Empty)} FOR XML PATH('{tableName}'), ROOT('ArrayOf{tableName}')"
            );
            return str;
        }

        /// <summary>
        /// Gets the TSQL selected columns query for json result set.
        /// </summary>
        /// <param name="tableClrType">Type of the table color.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <returns></returns>
        public static string GetTSQLSelectedColumnsQueryForJsonResultSet(Type tableClrType, ORMModelMetaInfo modelMetaInfo, List<PropertyInfo> selectedColumns)
        {
            var tsql = modelMetaInfo.GetTSQLByJsonForSelectedColumnsNonCached(tableClrType, selectedColumns);
            var tableName = modelMetaInfo.GetTableNameByType(tableClrType);
            var str = ($"{tsql} FOR JSON PATH ");
            return str;
        }

        /// <summary>
        /// Gets the TSQL selected columns query for json result set.
        /// </summary>
        /// <param name="tableClrType">Type of the table color.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns></returns>
        public static string GetTSQLSelectedColumnsQueryForJsonResultSet(Type tableClrType, ORMModelMetaInfo modelMetaInfo, List<PropertyInfo> selectedColumns, string whereClause)
        {
            var tsql = modelMetaInfo.GetTSQLByJsonForSelectedColumnsNonCached(tableClrType, selectedColumns);
            var tableName = modelMetaInfo.GetTableNameByType(tableClrType);
            var str = ($"{tsql} {(string.IsNullOrEmpty(whereClause) == false ? " WHERE " + whereClause : string.Empty)} FOR JSON PATH ");
            return str;
        }

        /// <summary>
        /// Gets the TSQL for selected columns.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <returns></returns>
        public static string GetTSQLForSelectedColumns(Type type, ORMModelMetaInfo modelMetaInfo, List<PropertyInfo> selectedColumns)
        {
            List<Node> nodes = GetNodeHierarchy(type, modelMetaInfo, null, selectedColumns);
            StringBuilder FuncRec(Type tableDataContractType, bool isRootTableContract, StringBuilder sbQuery, List<PropertyInfo> selectColumns, HashSet<Type> fetchedTypes)
            {
                if (fetchedTypes.Contains(tableDataContractType))
                {
                    return sbQuery;
                }

                fetchedTypes.Add(tableDataContractType);
                var tableName = modelMetaInfo.GetTableNameByType(tableDataContractType);
                var childProperties = modelMetaInfo.GetAllSelectedColumnPropertiesNonCached(tableDataContractType, selectColumns);
                var childEntitySetPropertyNames = modelMetaInfo.GetAllTableEntitySetProperties(tableDataContractType).ToList();

                var sortedNodes = childProperties.Select(c => new
                {
                    NodeType = "C",
                    Name = $"{tableName}.{c.ColumnName}",
                    PropertyName = c.PropertyName,
                    DbType = c.DbType,
                    ChildTableName = String.Empty,
                    ThisColumns = (List<string>)null,
                    ChildColumns = (List<string>)null,
                    ChildType = (Type)null
                })
                    .Concat(childEntitySetPropertyNames.Select(c => new
                    {
                        NodeType = "N",
                        Name = c.PropertyName,
                        PropertyName = c.PropertyName,
                        DbType = "",
                        ChildTableName = c.EntitySetTableName,
                        ThisColumns = c.ParentTableColumnNames,
                        ChildColumns = c.ChildTableColumnNames,
                        ChildType = c.EntitySetTableType
                    }))
                    .OrderBy(c => c.PropertyName, StringComparer.Ordinal)
                    .ToList();
                if (isRootTableContract) sbQuery.Append($"WITH XMLNAMESPACES ('http://schemas.datacontract.org/2004/07/System' as ns1, DEFAULT 'http://schemas.datacontract.org/2004/07/{type.Namespace}')");
                var isFirst = true;

                foreach (var node in sortedNodes) //iterate over the columns and child tables and make it part of select statement
                {
                    if (node.NodeType == "C")
                    {
                        sbQuery.Append(isFirst == false ? " , " : " SELECT ");
                        isFirst = false;
                        if (string.IsNullOrEmpty(node.DbType) == false && node.DbType.ToLower().Contains("xml") == true)
                        {
                            sbQuery.Append($"CONVERT(varchar(max),{node.Name}) AS {node.PropertyName} ");
                        }
                        else if (string.IsNullOrEmpty(node.DbType) == false && node.DbType.ToLower().Contains("datetimeoffset") == false)
                        {
                            sbQuery.Append(node.Name + " AS " + node.PropertyName + " ");
                        }
                        else
                        {
                            var datetimeSelect = $"CONVERT(nvarchar(30),{node.Name},127) AS '{node.PropertyName}/ns1:DateTime',DATEPART (TZoffset, {node.Name}) AS '{node.PropertyName}/ns1:OffsetMinutes'";
                            sbQuery.Append(datetimeSelect);
                        }
                    }
                    else //for child table
                    {
                        childProperties.ForEach(childProp =>
                        {
                            var column = selectColumns.FirstOrDefault(col => col.Name == childProp.PropertyName && col.ReflectedType.Name == tableName);
                            selectColumns.Remove(column);
                        });
                        var snode = nodes.Where(n => n.TableName == node.ChildTableName).FirstOrDefault();
                        if (selectColumns.Any() && snode != null && snode.FindColumnsByName(selectColumns, snode.TableName))
                        {
                            if (isFirst == false)
                                sbQuery.Append(",");

                            sbQuery.Append(" ( ");
                            FuncRec(node.ChildType, false, sbQuery, selectColumns,fetchedTypes); //recursive call for child table
                            sbQuery.Append($" FROM {node.ChildTableName} WHERE {node.ChildColumns.Zip<string, string, string>(node.ThisColumns, (f, s) => string.Format("{0}.{1}={2}.{3}", node.ChildTableName, f, tableDataContractType.Name, s)).ToList().ConcatenateListOfStringUsingSeparator(" AND ")} FOR XML PATH ('{node.ChildTableName}'), TYPE) AS '{node.Name}' ");
                        }
                    }
                }
                if (isRootTableContract == true)
                {
                    sbQuery.Append(" FROM " + tableDataContractType.Name);
                }
                return sbQuery;
            }
            return FuncRec(type, true, new StringBuilder(), new List<PropertyInfo>(selectedColumns), new HashSet<Type>()).ToString();
        }

        
        class Node
        {
            public List<string> Columns { get; set; }
            public string TableName { get; set; }
            public List<Node> ChildTables { get; set; }

            public Node()
            {
                Columns = new List<string>();
                ChildTables = new List<Node>();
            }

            public bool FindColumnsByName(List<PropertyInfo> queryColumns, string tableName, Node? Node = null)
            {
                bool FindColumnsByNameRecursively(List<PropertyInfo> selectedColumns, string sqlTableName, Node? nodeStruct = null)
                {
                    Node currentNode;
                    if (!selectedColumns.Any())
                        return false;

                    if (nodeStruct == null)
                        currentNode = this;
                    else
                        currentNode = nodeStruct;

                    bool currentResult = false; // = Columns.Exists(col => col == SelectedColumns.Where(c => c == col).FirstOrDefault());

                    foreach (string column in currentNode.Columns)
                    {
                        var qualifiedCols = selectedColumns.Where(scol => column == scol.Name && scol.ReflectedType.Name == sqlTableName).FirstOrDefault();
                        if (qualifiedCols != null)
                        {
                            currentResult = true;
                            selectedColumns.Remove(qualifiedCols);
                        }
                    }

                    foreach (Node childNode in currentNode.ChildTables)
                    {
                        if (FindColumnsByNameRecursively(selectedColumns, childNode.TableName, childNode))
                            return true;
                    }

                    return currentResult;
                }

                return FindColumnsByNameRecursively(new List<PropertyInfo>(queryColumns), tableName, Node);
            }

        }

        private static List<Node> GetNodeHierarchy(Type tableType, ORMModelMetaInfo mMetaInfo, Node pNode, List<PropertyInfo> queryColumns)
        {
            List<Node> GetNodeHierarchyRecursively(Type tableDataContractType, ORMModelMetaInfo modelMetaInfo, Node parentNode, List<PropertyInfo> selectedColumns)
            {
                List<Node> Nodes = new List<Node>();
                var tableName = modelMetaInfo.GetTableNameByType(tableDataContractType);
                var childProperties = modelMetaInfo.GetAllSelectedColumnPropertiesNonCached(tableDataContractType, selectedColumns);
                Node node = new Node();
                node.TableName = tableName;
                childProperties.ForEach(chp =>
                {
                    node.Columns.Add(chp.PropertyName);
                    var column = selectedColumns.FirstOrDefault(col => col.Name == chp.PropertyName && col.ReflectedType.Name == tableName);
                    selectedColumns.Remove(column);
                });

                var childEntitySetPropertyNames = modelMetaInfo.GetAllTableEntitySetProperties(tableDataContractType).ToList();
                Nodes.Add(node);

                if (parentNode != null)
                    parentNode.ChildTables.Add(node);

                if (selectedColumns.Any())
                {
                    foreach (var childNode in childEntitySetPropertyNames)
                    {
                        if (selectedColumns.Any())
                            Nodes.AddRange(GetNodeHierarchyRecursively(childNode.EntitySetTableType, modelMetaInfo, node, selectedColumns));
                        else
                            break;
                    }
                }

                return Nodes;
            }

            return GetNodeHierarchyRecursively(tableType, mMetaInfo, pNode, new List<PropertyInfo>(queryColumns));
        }

        /// <summary>
        /// Gets the TSQL for selected columns json.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="modelMetaInfo">The model meta information.</param>
        /// <param name="selectColumns">The select columns.</param>
        /// <returns></returns>
        public static string GetTSQLForSelectedColumnsJSON(Type type, ORMModelMetaInfo modelMetaInfo, List<PropertyInfo> selectColumns)
        {
            List<Node> nodes = GetNodeHierarchy(type, modelMetaInfo, null, selectColumns);
            StringBuilder FuncRec(Type tableDataContractType, bool isRootTableContract, StringBuilder sbQuery, List<PropertyInfo> selectedColumns, HashSet<Type> fetchedType )
            {
                if (fetchedType.Contains(tableDataContractType))
                {
                    Console.WriteLine($"Type is already fetched:{tableDataContractType.FullName}");
                    return sbQuery;
                }
                fetchedType.Add(tableDataContractType);
                var tableName = modelMetaInfo.GetTableNameByType(tableDataContractType);
                var childProperties = modelMetaInfo.GetAllSelectedColumnPropertiesNonCached(tableDataContractType, selectedColumns);
                var childEntitySetPropertyNames = modelMetaInfo.GetAllTableEntitySetProperties(tableDataContractType).ToList();
                var sortedNodes = childProperties.Select(c => new
                {
                    NodeType = "C",
                    Name = $"{tableName}.{c.ColumnName}",
                    PropertyName = c.PropertyName,
                    DbType = c.DbType,
                    ChildTableName = String.Empty,
                    ThisColumns = (List<string>)null,
                    ChildColumns = (List<string>)null,
                    ChildType = (Type)null
                })
                    .Concat(childEntitySetPropertyNames.Select(c => new
                    {
                        NodeType = "N",
                        Name = c.PropertyName,
                        PropertyName = c.PropertyName,
                        DbType = "",
                        ChildTableName = c.EntitySetTableName,
                        ThisColumns = c.ParentTableColumnNames,
                        ChildColumns = c.ChildTableColumnNames,
                        ChildType = c.EntitySetTableType
                    }))
                    .OrderBy(c => c.PropertyName, StringComparer.Ordinal)
                    .ToList();
                if (isRootTableContract) sbQuery.Append($"");
                var isFirst = true;
                foreach (var node in sortedNodes) //iterate over the columns and child tables and make it part of select statement
                {
                    if (node.NodeType == "C")
                    {
                        sbQuery.Append(isFirst == false ? " , " : " SELECT ");
                        isFirst = false;
                        if (String.IsNullOrEmpty(node.DbType) == false && node.DbType.ToLower().Contains("xml") == true)
                        {
                            sbQuery.Append($"CONVERT(varchar(max),{node.Name}) AS {node.PropertyName} ");
                        }
                        else if (String.IsNullOrEmpty(node.DbType) == false)
                        {
                            sbQuery.Append($"{node.Name} AS {node.PropertyName} ");
                        }
                    }
                    else //for child table
                    {
                        childProperties.ForEach(childProp =>
                        {
                            var column = selectedColumns.FirstOrDefault(col => col.Name == childProp.PropertyName && col.ReflectedType.Name == tableName);
                            selectedColumns.Remove(column);
                        });

                        var snode = nodes.Where(n => n.TableName == node.ChildTableName).FirstOrDefault();
                        if (snode!=null && selectedColumns.Any() && snode.FindColumnsByName(selectedColumns, snode.TableName))
                        {
                            if (isFirst == false)
                                sbQuery.Append(",");

                            sbQuery.Append(" ( ");
                            FuncRec(node.ChildType, false, sbQuery, selectedColumns,fetchedType); //recursive call for child table
                            sbQuery.Append($" FROM {node.ChildTableName} WHERE {node.ChildColumns.Zip<string, string, string>(node.ThisColumns, (f, s) => String.Format("{0}.{1}={2}.{3}", node.ChildTableName, f, tableDataContractType.Name, s)).ToList().ConcatenateListOfStringUsingSeparator(" AND ")} FOR JSON PATH ) AS '{node.Name}' ");
                        }
                    }
                }

                if (isRootTableContract == true)
                {
                    sbQuery.Append(" FROM " + tableDataContractType.Name);
                }

                return sbQuery;
            }
            return FuncRec(type, true, new StringBuilder(), new List<PropertyInfo>(selectColumns), new HashSet<Type>()).ToString();
        }

        #endregion
    }
}
/* P r o p r i e t a r y  N o t i c e */
/*
Confidential and proprietary information of Allscripts Healthcare, LLC and/or its affiliates. Authorized users only.
Notice to U.S. Government Users: This software is "Commercial Computer Software." Subject to full notice set
forth herein.
*/
/* P r o p r i e t a r y  N o t i c e */