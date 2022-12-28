

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>
    /// TableDataContractMetaInfoWithType class
    /// </summary>
    public class TableDataContractMetaInfoWithType 
    {
        /// <summary>
        /// Gets the table data contract meta information.
        /// </summary>
        /// <value>
        /// The table data contract meta information.
        /// </value>
        public TableDataContractMetaInfo TableDataContractMetaInfo { get; }

        /// <summary>
        /// The column properties with type list
        /// </summary>
        public readonly List<ColumnPropertyMetaInfoWithType> ColumnPropertiesWithTypeList ;

        /// <summary>
        /// Gets the table type class information.
        /// </summary>
        /// <value>
        /// The table type class information.
        /// </value>
        public Type TableTypeClassInfo { get; }

        /// <summary>
        /// Gets the child property meta information with type list.
        /// </summary>
        /// <value>
        /// The child property meta information with type list.
        /// </value>
        public List<ChildPropertyMetaInfoWithType> ChildPropertyMetaInfoWithTypeList { get; }

        /// <summary>
        /// Gets the type of the parent property meta information with.
        /// </summary>
        /// <value>
        /// The type of the parent property meta information with.
        /// </value>
        public ParentPropertyMetaInfoWithType[] ParentPropertyMetaInfoWithType { get; }
        /// <summary>
        /// The ignore columns property information list
        /// </summary>
        public readonly List<IColumnNetInfo> IgnoreColumnsPropertyInfoList;

        /// <summary>
        /// The always database generated property information list
        /// </summary>
        public readonly List<IColumnNetInfo> AlwaysDBGeneratedPropertyInfoList;

        /// <summary>
        /// The primary key properties information list
        /// </summary>
        public readonly List<IColumnNetInfo> PrimaryKeyPropertiesInfoList;

        /// <summary>
        /// The column dictionary
        /// </summary>
        public readonly Dictionary<string, ColumnPropertyMetaInfoWithType> ColumnDictionary;

        /// <summary>
        /// The primary key column creation definition
        /// </summary>
        public readonly string PrimaryKeyColumnCreationDefinition;

        /// <summary>
        /// The primary key with unique key column creation definition
        /// </summary>
        public readonly string PrimaryKeyWithUniqueKeyColumnCreationDefinition;

        /// <summary>
        /// The primary key columns comma separated
        /// </summary>
        public readonly string PrimaryKeyColumnsCommaSeparated;

        /// <summary>
        /// The primary key with unique key comma separated
        /// </summary>
        public readonly string PrimaryKeyWithUniqueKeyCommaSeparated;

        /// <summary>
        /// The primary key table qualified columns comma separated
        /// </summary>
        public readonly string PrimaryKeyTableQualifiedColumnsCommaSeparated;

        /// <summary>
        /// The primary key with unique key table qualified columns comma separated
        /// </summary>
        public readonly string PrimaryKeyWithUniqueKeyTableQualifiedColumnsCommaSeparated;

        /// <summary>
        /// The table class type with resolved properties
        /// </summary>
        public readonly Type TableClassTypeWithResolvedProperties;

        /// <summary>
        /// The primary identifier lookup class type
        /// </summary>
        public readonly Type PrimaryIDLookupClassType;

        /// <summary>
        /// The table primary key class type
        /// </summary>
        public readonly Type TablePrimaryKeyClassType;

        /// <summary>
        /// The source type resolve type mapping information
        /// </summary>
        public readonly SourceTypeResolveTypeMappingInfo SourceTypeResolveTypeMappingInfo;

        /// <summary>
        /// The list type of primary identifier lookup class type
        /// </summary>
        public readonly Type ListTypeOfPrimaryIDLookupClassType;

        /// <summary>
        /// The deserialize primary identifier lookup json to list of primary identifier lookup function
        /// </summary>
        public readonly Func<string, object> DeserializePrimaryIDLookupJSONToListOfPrimaryIDLookupFunc;

        /// <summary>
        /// The list type of primary identifier class type
        /// </summary>
        public readonly Type ListTypeOfPrimaryIDClassType;

        /// <summary>
        /// The type of primary identifier key base class type
        /// </summary>
        public readonly Type TypeOfPrimaryIDKeyBaseClassType;

        /// <summary>
        /// The list type of primary identifier key base class type
        /// </summary>
        public readonly Type ListTypeOfPrimaryIDKeyBaseClassType;

        /// <summary>
        /// The primary key class constructor information
        /// </summary>
        public readonly ConstructorInfo PrimaryKeyClassConstructorInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="TableDataContractMetaInfoWithType"/> class.
        /// </summary>
        /// <param name="tableDataContractMetaInfo">The table data contract meta information.</param>
        /// <param name="tableTypeClassInfo">The table type class information.</param>
        /// <param name="columnPropertiesWithTypeList">The column properties with type list.</param>
        /// <param name="childPropertyMetaInfoWithTypeList">The child property meta information with type list.</param>
        /// <param name="parentPropertyMetaInfoWithType">Type of the parent property meta information with.</param>
        /// <param name="ignoreColumnsPropertyInfoList">The ignore columns property information list.</param>
        /// <param name="alwaysDBGeneratedPropertyInfoList">The always database generated property information list.</param>
        /// <param name="primaryKeyPropertiesInfoList">The primary key properties information list.</param>
        /// <param name="tableClassTypeWithResolvedProperties">The table class type with resolved properties.</param>
        /// <param name="primaryIDLookupClassType">Type of the primary identifier lookup class.</param>
        /// <param name="tablePrimaryKeyClassType">Type of the table primary key class.</param>
        /// <param name="sourceTypeResolveTypeMappingInfo">The source type resolve type mapping information.</param>
        /// <exception cref="ArgumentException">Number of columns in Primary key does not have same number of property info</exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">Could not find constructor on Primary Key Class Type {TablePrimaryKeyClassType.FullName} which takes primary properties as input argument</exception>
        public TableDataContractMetaInfoWithType(TableDataContractMetaInfo tableDataContractMetaInfo
            , Type tableTypeClassInfo, List<ColumnPropertyMetaInfoWithType> columnPropertiesWithTypeList
            , List<ChildPropertyMetaInfoWithType> childPropertyMetaInfoWithTypeList
            , ParentPropertyMetaInfoWithType[] parentPropertyMetaInfoWithType
            , List<IColumnNetInfo> ignoreColumnsPropertyInfoList
            , List<IColumnNetInfo> alwaysDBGeneratedPropertyInfoList
            , List<IColumnNetInfo> primaryKeyPropertiesInfoList
            , Type tableClassTypeWithResolvedProperties, Type primaryIDLookupClassType,Type tablePrimaryKeyClassType, SourceTypeResolveTypeMappingInfo sourceTypeResolveTypeMappingInfo)
        {
            if (tableDataContractMetaInfo.TablePrimaryKeyColumns.Count != primaryKeyPropertiesInfoList.Count)
            {
                throw new ArgumentException($" Number of columns in Primary key does not have same number of property info");
            }

            if (childPropertyMetaInfoWithTypeList == null)
            {
                throw new ArgumentNullException($" {nameof(childPropertyMetaInfoWithTypeList)}");
            }
            
            TableDataContractMetaInfo = tableDataContractMetaInfo;
            TableTypeClassInfo = tableTypeClassInfo;
            ChildPropertyMetaInfoWithTypeList = childPropertyMetaInfoWithTypeList;
            ParentPropertyMetaInfoWithType = parentPropertyMetaInfoWithType;
            ColumnPropertiesWithTypeList = columnPropertiesWithTypeList;
            IgnoreColumnsPropertyInfoList = ignoreColumnsPropertyInfoList ?? new List<IColumnNetInfo>();
            AlwaysDBGeneratedPropertyInfoList = alwaysDBGeneratedPropertyInfoList ?? new List<IColumnNetInfo>();
            PrimaryKeyPropertiesInfoList = primaryKeyPropertiesInfoList;
            ColumnDictionary = columnPropertiesWithTypeList.ToLookup(c => c.ColumnPropertyMetaInfo.ColumnName)
                .ToDictionary(k => k.Key, kv => kv.First(),StringComparer.InvariantCultureIgnoreCase);
            var columnDictionaryLowerCase = columnPropertiesWithTypeList.ToLookup(c => c.ColumnPropertyMetaInfo.ColumnName.ToLower())
                .ToDictionary(k => k.Key, kv => kv.First());

            var propertyDictionary =  TableDataContractMetaInfo.ColumnPropertiesList.ToLookup(c => c.ColumnPropertyName)
                .ToDictionary(k => k.Key, kv => kv.First());

            
            PrimaryKeyColumnCreationDefinition = TableDataContractMetaInfo.TablePrimaryKeyColumns != null &&
                                                 TableDataContractMetaInfo.TablePrimaryKeyColumns.Any()
                ? string.Join(" , ", TableDataContractMetaInfo.TablePrimaryKeyColumns
                    .Select(primaryKeyColumnName =>
                        $" {primaryKeyColumnName} {columnDictionaryLowerCase[primaryKeyColumnName.ToLower()].ColumnPropertyMetaInfo.SqlDataType}"))
                : string.Empty;
            
            /*
            PrimaryKeyColumnCreationDefinition = TableDataContractMetaInfo.TablePrimaryKeyColumns != null &&
                                                 TableDataContractMetaInfo.TablePrimaryKeyColumns.Any()
                ? string.Join(" , ", TableDataContractMetaInfo.TablePrimaryKeyColumns
                    .Select(primaryKeyColumnName =>
                        $" {primaryKeyColumnName} {propertyDictionary[primaryKeyColumnName].SqlDataType}"))
                : string.Empty;
                */

            
            PrimaryKeyWithUniqueKeyColumnCreationDefinition = TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns != null &&
                                                              TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns.Any()
                                                          ?   string.Join(" , ", TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns
                                                              .Select(primaryKeyColumnName =>
                                                                  $" {primaryKeyColumnName} {columnDictionaryLowerCase[primaryKeyColumnName.ToLower()].ColumnPropertyMetaInfo.SqlDataType}"))
                                                          : string.Empty; 
            /*
            PrimaryKeyWithUniqueKeyColumnCreationDefinition = TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns != null &&
                                                              TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns.Any()
                ?   string.Join(" , ", TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns
                    .Select(primaryKeyColumnName =>
                        $" {primaryKeyColumnName} {propertyDictionary[primaryKeyColumnName].SqlDataType}"))
                : string.Empty; 
                */

            PrimaryKeyColumnsCommaSeparated = TableDataContractMetaInfo.TablePrimaryKeyColumns != null &&
                                              TableDataContractMetaInfo.TablePrimaryKeyColumns.Any()
                ? string.Join(" , ", TableDataContractMetaInfo.TablePrimaryKeyColumns)
                : string.Empty;
             
            PrimaryKeyWithUniqueKeyCommaSeparated = TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns != null &&
                                                    TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns.Any()
                ? string.Join(" , ", TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns)
                : string.Empty;
                
            PrimaryKeyTableQualifiedColumnsCommaSeparated = TableDataContractMetaInfo.TablePrimaryKeyColumns != null &&
                                                            TableDataContractMetaInfo.TablePrimaryKeyColumns.Any()
                ? string.Join(" , ", TableDataContractMetaInfo.TablePrimaryKeyColumns.Select(c => $"[{tableDataContractMetaInfo.TableName}].[{c}]"))
                : string.Empty;
            
            PrimaryKeyWithUniqueKeyTableQualifiedColumnsCommaSeparated = TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns != null &&
                                                                         TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns.Any()
                ? string.Join(" , ", TableDataContractMetaInfo.TablePrimaryKeyWithUniqueKeyColumns.Select(c => $"[{tableDataContractMetaInfo.TableName}].[{c}]"))
                : string.Empty;
            TableClassTypeWithResolvedProperties = tableClassTypeWithResolvedProperties;
            PrimaryIDLookupClassType = primaryIDLookupClassType;
            TablePrimaryKeyClassType = tablePrimaryKeyClassType;
            SourceTypeResolveTypeMappingInfo = sourceTypeResolveTypeMappingInfo;
            ListTypeOfPrimaryIDLookupClassType = PrimaryIDLookupClassType != null ? typeof(List<>).MakeGenericType(new Type[] {PrimaryIDLookupClassType}) : null;
            DeserializePrimaryIDLookupJSONToListOfPrimaryIDLookupFunc = PrimaryIDLookupClassType != null
                ? (Func<string, object>) ((string jsonText) =>
                    typeof(DataContractSerializerHelper)
                        .GetMethod("DataContractJSONDeserializer", new Type[] {typeof(string)})
                        ?.MakeGenericMethod(new Type[] {ListTypeOfPrimaryIDLookupClassType})
                        .Invoke(null, new object[] {jsonText}))
                : (string _) => throw new InvalidOperationException();
            
            PrimaryKeyClassConstructorInfo = TablePrimaryKeyClassType != null 
                ? TablePrimaryKeyClassType.GetConstructor(PrimaryKeyPropertiesInfoList.Select(c => c.PropertyType).ToArray()) 
                : null;
            if (TablePrimaryKeyClassType != null && PrimaryKeyClassConstructorInfo == null)
            {
                throw new InvalidOperationException($"Could not find constructor on Primary Key Class Type {TablePrimaryKeyClassType.FullName} which takes primary properties as input argument");
            }

            ListTypeOfPrimaryIDClassType = TablePrimaryKeyClassType != null ?  typeof(List<>).MakeGenericType(new[] {TablePrimaryKeyClassType}) : null;

            TypeOfPrimaryIDKeyBaseClassType = TablePrimaryKeyClassType != null
                ? typeof(IPrimaryKeyBase<>).MakeGenericType(new[] {TableTypeClassInfo})
                : null;
            ListTypeOfPrimaryIDKeyBaseClassType = TypeOfPrimaryIDKeyBaseClassType != null
                ? typeof(List<>).MakeGenericType(new Type[] {TypeOfPrimaryIDKeyBaseClassType})
                : null;
         
        }

        /// <summary>
        /// Gets the create primary key temporary table definition.
        /// </summary>
        /// <param name="tempTableName">Name of the temporary table.</param>
        /// <returns></returns>
        public string GetCreatePrimaryKeyTempTableDefinition(TempTableVariable tempTableName)
        {
            return $" CREATE TABLE {tempTableName.TempTableVariableName} ( {PrimaryKeyColumnCreationDefinition})";
        }

        /// <summary>
        /// Gets the create primary key with unique key temporary table definition.
        /// </summary>
        /// <param name="tempTableName">Name of the temporary table.</param>
        /// <returns></returns>
        public string GetCreatePrimaryKeyWithUniqueKeyTempTableDefinition(string tempTableName)
        {
            return $" CREATE TABLE {tempTableName} ( {PrimaryKeyWithUniqueKeyColumnCreationDefinition})";
        }

        /// <summary>
        /// Gets the insert primary key values from XML into temporary table.
        /// </summary>
        /// <param name="tempTableName">Name of the temporary table.</param>
        /// <param name="xmlVariableNameWithAtSign">The XML variable name with at sign.</param>
        /// <returns></returns>
        public string GetInsertPrimaryKeyValuesFromXMLIntoTempTable(TempTableVariable tempTableName, string xmlVariableNameWithAtSign)
        {
            return $@"
                     DECLARE @idoc INT; 
                     EXEC sp_xml_preparedocument @idoc OUTPUT, {xmlVariableNameWithAtSign};
                     INSERT INTO  {tempTableName.TempTableVariableName}({PrimaryKeyColumnsCommaSeparated})
                     SELECT  {PrimaryKeyColumnsCommaSeparated}
                     FROM OPENXML(@idoc,'/ArrayOf{TableDataContractMetaInfo.TablePrimaryKeyClassName}/{TableDataContractMetaInfo.TablePrimaryKeyClassName}',2)   
                     WITH
                     (
                        {PrimaryKeyColumnCreationDefinition}
                     );   
                    ";
        }

        /// <summary>
        /// Gets the insert primary key values from json into temporary table.
        /// </summary>
        /// <param name="tempTableName">Name of the temporary table.</param>
        /// <param name="jsonVariableNameWithAtSign">The json variable name with at sign.</param>
        /// <returns></returns>
        public string GetInsertPrimaryKeyValuesFromJSONIntoTempTable(TempTableVariable tempTableName,
            ArgumentVariable jsonVariableNameWithAtSign)
        {
            return $@"INSERT INTO {tempTableName.TempTableVariableName}({PrimaryKeyColumnsCommaSeparated})
               SELECT {PrimaryKeyColumnsCommaSeparated} FROM OPENJSON ({jsonVariableNameWithAtSign.ArgumentVariableName})
                WITH (
	                {PrimaryKeyColumnCreationDefinition}
	                )";
        }


        private string GetINNERJoinConditionWithTableOnPKColumnsWithPKTempTable(TempTableVariable tempTableNameWithHashPrefixed)
        {
            var joinCondition =
                string.Join(" AND ",
                    TableDataContractMetaInfo.TablePrimaryKeyColumns.Zip(TableDataContractMetaInfo.TablePrimaryKeyColumns, (t, o) =>
                        $@"
                [{TableDataContractMetaInfo.TableName}].[{t}] = [{tempTableNameWithHashPrefixed.TempTableVariableName}].[{o}]
                "));
            return $" INNER JOIN {tempTableNameWithHashPrefixed.TempTableVariableName} ON {joinCondition} ";
        }

        /// <summary>
        /// Gets the select query based on primary key json values.
        /// </summary>
        /// <param name="userQueryFullyQualifiedByTableName">Name of the user query fully qualified by table.</param>
        /// <param name="jsonVariableName">Name of the json variable.</param>
        /// <returns></returns>
        public string GetSelectQueryBasedOnPrimaryKeyJSONValues(string userQueryFullyQualifiedByTableName, ArgumentVariable jsonVariableName)
        {
            
            var tempPKTableName = new TempTableVariable ( $"#{TableDataContractMetaInfo.TableName}PK");
            var pkTableDefinition = GetCreatePrimaryKeyTempTableDefinition(tempPKTableName);
            var insertIntoTempTableStatement =
                GetInsertPrimaryKeyValuesFromJSONIntoTempTable(tempPKTableName,
                    jsonVariableName);
            var innerJoinCondition =
                GetINNERJoinConditionWithTableOnPKColumnsWithPKTempTable(tempPKTableName);
            var fetchLookUpQuery = $@"
                                        {pkTableDefinition}
                                        {insertIntoTempTableStatement}
                                        {userQueryFullyQualifiedByTableName}
                                        {innerJoinCondition}
                                        FOR JSON PATH
                                     ";
            return fetchLookUpQuery;
        }

        /// <summary>
        /// Gets the type of the lookup resolve query without filter for.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Table {TableDataContractMetaInfo.TableName} does not contains lookup information</exception>
        public string GetLookupResolveQueryWithoutFilterForType()
        {
            
                if (TableDataContractMetaInfo.GetLookupColumnPropertiesWithPrimaryKeyProperties() != null)
                {
                    
                    var selectColumns = string.Join(",",
                        TableDataContractMetaInfo.GetLookupColumnPropertiesWithPrimaryKeyProperties().Select(c =>
                        {
                            if (c.IsPKColumn)
                            {
                                return $"[{TableDataContractMetaInfo.TableName}].{c.ColumnName} AS 'PrimaryKeyRecordInfo.{c.ColumnPropertyName}'";
                            }
                            else
                            {
                                return $"[{TableDataContractMetaInfo.TableName}].{c.ColumnName} AS '{c.ColumnPropertyName}'";
                            }
                        }).ToList());

                    var sql = $@" SELECT {selectColumns} FROM [{TableDataContractMetaInfo.TableName}]";
                    return sql;
                }
                else
                {
                    throw new InvalidOperationException($"Table {TableDataContractMetaInfo.TableName} does not contains lookup information");
                }

                //PrimaryIDLookupValueRecordInfo
                /*return $@"SELECT {TableDataContractMetaInfo.TableName}.TransactionCodeMasterID AS 'PrimaryKeyRecordInfo.TransactionCodeMasterID'
                        , SXARCMTransactionCodeMaster.TransactionCode AS 'PrimaryKeyRecordInfo.TransactionCode'
                        , SXARCMTransactionTypeMaster.TransactionTypeName
                        , SXARCMTransactionSubTypeMaster.TransactionSubTypeName
                        FROM SXARCMTransactionCodeMaster
                        INNER JOIN SXARCMTransactionSubTypeMaster ON SXARCMTransactionCodeMaster.TransactionSubTypeMasterID = SXARCMTransactionSubTypeMaster.TransactionSubTypeMasterID
                        INNER JOIN SXARCMTransactionTypeMaster ON SXARCMTransactionTypeMaster.TransactionTypeMasterID = SXARCMTransactionSubTypeMaster.TransactionTypeMasterID";*/
            
        }

        /// <summary>
        /// Creates the insert statement of primary key values joining with other table.
        /// </summary>
        /// <param name="tempTableName">Name of the temporary table.</param>
        /// <param name="thisTableColumnsUsedForJoining">The this table columns used for joining.</param>
        /// <param name="otherTableNameToJoinWith">The other table name to join with.</param>
        /// <param name="otherTableColumnNames">The other table column names.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public string CreateInsertStatementOfPrimaryKeyValuesJoiningWithOtherTable(string tempTableName
            , List<string> thisTableColumnsUsedForJoining 
            , string otherTableNameToJoinWith
            , List<string> otherTableColumnNames)
        {
            if (thisTableColumnsUsedForJoining == null || thisTableColumnsUsedForJoining.Any() == false)
            {
                throw new ArgumentException($"{nameof(thisTableColumnsUsedForJoining)} is null or empty");
            }
            if (otherTableColumnNames == null || otherTableColumnNames.Any() == false)
            {
                throw new ArgumentException($"{nameof(thisTableColumnsUsedForJoining)} is null or empty");
            }

            if (thisTableColumnsUsedForJoining.Count != otherTableColumnNames.Count)
            {
                throw new ArgumentException($"{nameof(thisTableColumnsUsedForJoining)} joining columns count should be same");
            }
            var joinCondition =
                string.Join(" AND ",
                    thisTableColumnsUsedForJoining.Zip(otherTableColumnNames, (t, o) =>
                        $@"
                [{TableDataContractMetaInfo.TableName}].[{t}] = [{otherTableNameToJoinWith}].[{o}]
                "));
            return $@"
                    INSERT INTO {tempTableName}({PrimaryKeyColumnsCommaSeparated})
                    SELECT {PrimaryKeyTableQualifiedColumnsCommaSeparated}
                    FROM [{TableDataContractMetaInfo.TableName}] INNER JOIN [{otherTableNameToJoinWith}] ON
                        {joinCondition}
                    ";
        }

        /// <summary>
        /// Creates the insert statement of primary key with unique key values joining with other table.
        /// </summary>
        /// <param name="tempTableName">Name of the temporary table.</param>
        /// <param name="thisTableColumnsUsedForJoining">The this table columns used for joining.</param>
        /// <param name="otherTableNameToJoinWith">The other table name to join with.</param>
        /// <param name="otherTableColumnNames">The other table column names.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        /// </exception>
        public string CreateInsertStatementOfPrimaryKeyWithUniqueKeyValuesJoiningWithOtherTable(string tempTableName
            , List<string> thisTableColumnsUsedForJoining 
            , string otherTableNameToJoinWith
            , List<string> otherTableColumnNames)
        {
            if (thisTableColumnsUsedForJoining == null || thisTableColumnsUsedForJoining.Any() == false)
            {
                throw new ArgumentException($"{nameof(thisTableColumnsUsedForJoining)} is null or empty");
            }
            if (otherTableColumnNames == null || otherTableColumnNames.Any() == false)
            {
                throw new ArgumentException($"{nameof(thisTableColumnsUsedForJoining)} is null or empty");
            }

            if (thisTableColumnsUsedForJoining.Count != otherTableColumnNames.Count)
            {
                throw new ArgumentException($"{nameof(thisTableColumnsUsedForJoining)} joining columns count should be same");
            }
            var joinCondition =
                string.Join(" AND ",
                    thisTableColumnsUsedForJoining.Zip(otherTableColumnNames, (t, o) =>
                        $@"
                [{TableDataContractMetaInfo.TableName}].[{t}] = [{otherTableNameToJoinWith}].[{o}]
                "));
            return $@"
                    INSERT INTO {tempTableName}({PrimaryKeyWithUniqueKeyCommaSeparated})
                    SELECT {PrimaryKeyWithUniqueKeyTableQualifiedColumnsCommaSeparated}
                    FROM [{TableDataContractMetaInfo.TableName}] INNER JOIN [{otherTableNameToJoinWith}] ON
                        {joinCondition}
                    ";
        }

        /// <summary>
        /// Gets the delete TSQL statement based on primary key from temporary table.
        /// </summary>
        /// <param name="tempTableName">Name of the temporary table.</param>
        /// <returns></returns>
        public string GetDeleteTSQLStatementBasedOnPrimaryKeyFromTempTable(string tempTableName)
        {
            var joinCondition = string.Join(" AND "
                ,TableDataContractMetaInfo.TablePrimaryKeyColumns.Zip(
                    TableDataContractMetaInfo.TablePrimaryKeyColumns
                    ,(c, t) => $"[{TableDataContractMetaInfo.TableName}].[{c}] = [{tempTableName}].[{t}]"));
            
            return $@"
                DELETE FROM [{TableDataContractMetaInfo.TableName}]
                FROM [{TableDataContractMetaInfo.TableName}]
                INNER JOIN {tempTableName} 
                    ON {joinCondition}
                    ";
        }
        
        
    }

    /// <summary>
    ///  ParentPropertyMetaInfoWithType class
    /// </summary>
    public class ParentPropertyMetaInfoWithType
    {
        /// <summary>
        /// Gets the parent property meta information.
        /// </summary>
        /// <value>
        /// The parent property meta information.
        /// </value>
        public ParentPropertyMetaInfo ParentPropertyMetaInfo { get; }

        /// <summary>
        /// Gets the type of the parent class.
        /// </summary>
        /// <value>
        /// The type of the parent class.
        /// </value>
        public Type ParentClassType { get; }

        /// <summary>
        /// Gets the type of the child class.
        /// </summary>
        /// <value>
        /// The type of the child class.
        /// </value>
        public Type ChildClassType { get; }

        /// <summary>
        /// Gets the entity reference property information.
        /// </summary>
        /// <value>
        /// The entity reference property information.
        /// </value>
        public IColumnNetInfo EntityRefPropertyInfo { get; }

        /// <summary>
        /// Gets the entity set property information.
        /// </summary>
        /// <value>
        /// The entity set property information.
        /// </value>
        public IColumnNetInfo EntitySetPropertyInfo { get; }

        /// <summary>
        /// Gets the parent participating property information list.
        /// </summary>
        /// <value>
        /// The parent participating property information list.
        /// </value>
        public List<IColumnNetInfo> ParentParticipatingPropertyInfoList { get; }

        /// <summary>
        /// Gets the child participating property information list.
        /// </summary>
        /// <value>
        /// The child participating property information list.
        /// </value>
        public List<IColumnNetInfo> ChildParticipatingPropertyInfoList { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParentPropertyMetaInfoWithType"/> class.
        /// </summary>
        /// <param name="parentPropertyMetaInfo">The parent property meta information.</param>
        /// <param name="parentClassType">Type of the parent class.</param>
        /// <param name="childClassType">Type of the child class.</param>
        /// <param name="entityRefPropertyInfo">The entity reference property information.</param>
        /// <param name="entitySetPropertyInfo">The entity set property information.</param>
        /// <param name="parentParticipatingPropertyInfoList">The parent participating property information list.</param>
        /// <param name="childParticipatingPropertyInfoList">The child participating property information list.</param>
        public ParentPropertyMetaInfoWithType(ParentPropertyMetaInfo parentPropertyMetaInfo, Type parentClassType, Type childClassType, IColumnNetInfo entityRefPropertyInfo, IColumnNetInfo entitySetPropertyInfo, List<IColumnNetInfo> parentParticipatingPropertyInfoList, List<IColumnNetInfo> childParticipatingPropertyInfoList)
        {
            ParentPropertyMetaInfo = parentPropertyMetaInfo;
            ParentClassType = parentClassType;
            ChildClassType = childClassType;
            EntityRefPropertyInfo = entityRefPropertyInfo;
            EntitySetPropertyInfo = entitySetPropertyInfo;
            ParentParticipatingPropertyInfoList = parentParticipatingPropertyInfoList;
            ChildParticipatingPropertyInfoList = childParticipatingPropertyInfoList;
        }
        
    }

    /// <summary>
    ///  ChildPropertyMetaInfoWithType class
    /// </summary>
    public class ChildPropertyMetaInfoWithType
    {
        /// <summary>
        /// Gets the child property meta information.
        /// </summary>
        /// <value>
        /// The child property meta information.
        /// </value>
        public ChildPropertyMetaInfo ChildPropertyMetaInfo { get; }

        /// <summary>
        /// Gets the child entity set property information.
        /// </summary>
        /// <value>
        /// The child entity set property information.
        /// </value>
        public IColumnNetInfo ChildEntitySetPropertyInfo { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ChildPropertyMetaInfoWithType"/> class.
        /// </summary>
        /// <param name="childPropertyMetaInfo">The child property meta information.</param>
        /// <param name="childEntitySetPropertyInfo">The child entity set property information.</param>
        public ChildPropertyMetaInfoWithType(ChildPropertyMetaInfo childPropertyMetaInfo, IColumnNetInfo childEntitySetPropertyInfo)
        {
            ChildPropertyMetaInfo = childPropertyMetaInfo;
            ChildEntitySetPropertyInfo = childEntitySetPropertyInfo;
            
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