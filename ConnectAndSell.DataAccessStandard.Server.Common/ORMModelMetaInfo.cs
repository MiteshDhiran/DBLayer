
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using static ConnectAndSell.DataAccessStandard.Common.DataContract.Utility;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    /// <summary>
    ///  ORMModelMetaInfo class
    /// </summary>
    public class ORMModelMetaInfo
    {
        private ORMMetadata ORMMetadata { get; }
        private Assembly DataContractAssembly { get; }

        /// <summary>
        /// Gets the table type meta information dic.
        /// </summary>
        /// <value>
        /// The table type meta information dic.
        /// </value>
        public Dictionary<Type, TableDataContractMetaInfoWithType> TableTypeMetaInfoDic { get; }

        /// <summary>
        /// Gets the full name of the table type meta information dic by class.
        /// </summary>
        /// <value>
        /// The full name of the table type meta information dic by class.
        /// </value>
        public ReadOnlyDictionary<string, TableDataContractMetaInfoWithType> TableTypeMetaInfoDicByClassFullName { get; }

        /// <summary>
        /// Gets the table name table data contract meta information with type dic.
        /// </summary>
        /// <value>
        /// The table name table data contract meta information with type dic.
        /// </value>
        public ReadOnlyDictionary<string, TableDataContractMetaInfoWithType> TableNameTableDataContractMetaInfoWithTypeDic { get; }
        /// <summary>
        /// The table name type dic
        /// </summary>
        public readonly Dictionary<string, Type> TableNameTypeDic;
        /// <summary>
        /// The type full name dictionary
        /// </summary>
        public readonly Dictionary<string, Type> TypeFullNameDictionary;
        /// <summary>
        /// The get all child paths
        /// </summary>
        public readonly Func<Type, List<string>> GetAllChildPaths;

        public readonly Func<Tuple<Type,List<string>>, List<string>> GetFilteredChildPaths;
        /// <summary>
        /// The get immediate child property names
        /// </summary>
        public readonly Func<Type, List<string>> GetImmediateChildPropertyNames;
        /// <summary>
        /// The get all table column properties
        /// </summary>
        public readonly Func<Type, List<EntityColumnInfo>> GetAllTableColumnProperties;
        /// <summary>
        /// The get all table entity set properties
        /// </summary>
        public readonly Func<Type, List<EntitySetPropertyInfo>> GetAllTableEntitySetProperties;
        /// <summary>
        /// The get TSQL by XML for data contract
        /// </summary>
        public readonly Func<Type, string> GetTSQLByXMLForDataContract;
        /// <summary>
        /// The get TSQL by json for data contract
        /// </summary>
        public readonly Func<Type, string> GetTSQLByJSONForDataContract;
        /// <summary>
        /// The get TSQL query for associate json result set
        /// </summary>
        public readonly Func<Type, string> GetTSQLQueryForAssociateJSONResultSet;
        /// <summary>
        /// The get nested child table type
        /// </summary>
        public readonly Func<Type, List<Tuple<Type, Type>>> GetNestedChildTableType;
        /// <summary>
        /// The get bulk insert SQL for type
        /// </summary>
        public readonly Func<Type, string> GetBulkInsertSQLForType;
        /// <summary>
        /// The get table column information for bulk insert
        /// </summary>
        public readonly Func<Type, List<SQLTableTypeColumnInfo>> GetTableColumnInfoForBulkInsert;
        /// <summary>
        /// The get delete TSQL for type
        /// </summary>
        public readonly Func<Type, string> GetDeleteTSQLForType;
        /// <summary>
        /// The get nested child table type with entity set information
        /// </summary>
        public readonly Func<Type, List<ChildTypeParentType>> GetNestedChildTableTypeWithEntitySetInfo;

        /// <summary>
        /// The pk lookup class dependency dictionary by table data contract meta information
        /// </summary>
        public readonly
            ReadOnlyDictionary<TableDataContractMetaInfoWithType, List<ResolvePropertyDependencyOnAnotherTableInfoWithType>>
            PKLookupClassDependencyDictionaryByTableDataContractMetaInfo ;

        /// <summary>
        /// Initializes a new instance of the <see cref="ORMModelMetaInfo"/> class.
        /// </summary>
        /// <param name="ormMetadata">The orm metadata.</param>
        /// <param name="dataContractAssembly">The data contract assembly.</param>
        public ORMModelMetaInfo(ORMMetadata ormMetadata, Assembly dataContractAssembly)
        {
            ORMMetadata = ormMetadata;
            DataContractAssembly = dataContractAssembly;
            TypeFullNameDictionary = DataContractAssembly.GetTypes().ToLookup(c => c.FullName).ToDictionary(k => k.Key, kv => kv.First());
            TableTypeMetaInfoDic = GetTableDataContractMetaInfoWithType();
            TableNameTypeDic = GetTableNameTypeDic();
            TableTypeMetaInfoDicByClassFullName = new ReadOnlyDictionary<string, TableDataContractMetaInfoWithType>(
                    TableTypeMetaInfoDic.Select(kv =>
                        new KeyValuePair<string, TableDataContractMetaInfoWithType>(kv.Key.FullName, kv.Value)).ToDictionary(k => k.Key, kv => kv.Value)
                );
            
            TableNameTableDataContractMetaInfoWithTypeDic = new ReadOnlyDictionary<string, TableDataContractMetaInfoWithType>(
                TableTypeMetaInfoDic.Select(kv =>
                    new KeyValuePair<string, TableDataContractMetaInfoWithType>(kv.Value.TableDataContractMetaInfo.TableName, kv.Value)).ToDictionary(k => k.Key, kv => kv.Value)
            );

            GetAllChildPaths = DBUtility.Memonize<Type, List<string>>((_, type) =>
                GetChildProperties(type, type, "", null,
                    new Dictionary<Type, List<IColumnNetInfo>>(), new List<string>()).Item1);

            GetFilteredChildPaths = DBUtility.Memonize<Tuple<Type,List<string>>, List<string>>((_, typeWithFilteredTableList) =>
                GetChildPropertiesFilteredByTables(
                    rootType:typeWithFilteredTableList.Item1
                    ,childTablesToBeConsidered: typeWithFilteredTableList.Item2
                    , type: typeWithFilteredTableList.Item1
                    ,grandParentPath: string.Empty
                    , parentPriopertyInfo:null
                    , agg: new Dictionary<Type, List<IColumnNetInfo>>()
                    ,aggPathList: new List<string>()).Item1);

            GetImmediateChildPropertyNames =
                DBUtility.Memonize<Type, List<string>>((_, type) => GetImmediateChildPropertyNamesNonCached(type));
            GetAllTableColumnProperties = DBUtility.Memonize<Type, List<EntityColumnInfo>>((_, type) => GetAllTableColumnPropertiesNonCached(type));
            GetAllTableEntitySetProperties = DBUtility.Memonize<Type, List<EntitySetPropertyInfo>>((_, type) => GetAllTableEntitySetPropertiesNonCached(type));
            GetTSQLByXMLForDataContract = DBUtility.Memonize<Type, string>((_, type) => GetTSQLByXMLForDataContractNonCached(type));
            GetTSQLByJSONForDataContract = DBUtility.Memonize<Type, string>((_, type) => GetTSQLByJSONForDataContractNonCached(type));
            GetTSQLQueryForAssociateJSONResultSet = DBUtility.Memonize<Type, string>((_, type) => GetTSQLQueryForAssociateJSONResultSetNonCached(type));
            GetNestedChildTableType =
                DBUtility.Memonize<Type, List<Tuple<Type, Type>>>((_, type) => GetNestedChildTableTypeNonCached(type));
            GetBulkInsertSQLForType = DBUtility.Memonize<Type, string>((_, type) => GetBulkInsertSQLForTypeInternalNonCached(type));
            GetTableColumnInfoForBulkInsert = DBUtility.Memonize<Type, List<SQLTableTypeColumnInfo>>((_, type) => GetTableColumnInfoForBulkInsertNonCached(type));
            GetNestedChildTableTypeWithEntitySetInfo =
                DBUtility.Memonize<Type, List<ChildTypeParentType>>((_, type) =>
                    GetNestedChildTableTypeWithEntitySetInfoNonCached(type));
            GetDeleteTSQLForType = DBUtility.Memonize<Type, string>((_, type) => GetDeleteTSQLForTypeInternal(type));
            
            
            PKLookupClassDependencyDictionaryByTableDataContractMetaInfo =
                new ReadOnlyDictionary<TableDataContractMetaInfoWithType,
                    List<ResolvePropertyDependencyOnAnotherTableInfoWithType>>(
                    TableTypeMetaInfoDic.Where(k => k.Value.TableDataContractMetaInfo?.TablePrimaryKeyLookupFurtherDependenciesInfo != null 
                                                    && k.Value.TableDataContractMetaInfo.TablePrimaryKeyLookupFurtherDependenciesInfo.Any(c => string.IsNullOrEmpty(c.AnotherPKTableName) == false))
                        .Select(kv =>
                        new KeyValuePair<TableDataContractMetaInfoWithType,List<ResolvePropertyDependencyOnAnotherTableInfoWithType>>(kv.Value
                            ,kv.Value.TableDataContractMetaInfo.TablePrimaryKeyLookupFurtherDependenciesInfo?.Select(c => new ResolvePropertyDependencyOnAnotherTableInfoWithType(c,TableNameTableDataContractMetaInfoWithTypeDic[c.AnotherPKTableName],kv.Value.PrimaryIDLookupClassType.GetProperty(c.PropertyName))).ToList()
                        )
                    ).ToDictionary(k => k.Key, kkv => kkv.Value)
                    );
                
        }

        private Tuple<List<string>, Dictionary<Type, List<IColumnNetInfo>>> GetChildPropertiesFilteredByTables(Type rootType, List<string> childTablesToBeConsidered, Type type, string grandParentPath, IColumnNetInfo parentPriopertyInfo, Dictionary<Type, List<IColumnNetInfo>> agg, List<string> aggPathList)
        {
            var allChildEntitySetProperties = childTablesToBeConsidered.Any() ? TableTypeMetaInfoDic[type].ChildPropertyMetaInfoWithTypeList.Where(n => childTablesToBeConsidered.Contains(n.ChildPropertyMetaInfo.ChildTableName,StringComparer.InvariantCultureIgnoreCase)).Select(n => n.ChildEntitySetPropertyInfo).ToList() 
                : TableTypeMetaInfoDic[type].ChildPropertyMetaInfoWithTypeList.Select(n => n.ChildEntitySetPropertyInfo).ToList();
            agg.Add(type, allChildEntitySetProperties);
            var grandParentPathWithDot = string.IsNullOrEmpty(grandParentPath) == false ? $"{grandParentPath}." : grandParentPath;
            aggPathList.AddRange(allChildEntitySetProperties.Select(c => parentPriopertyInfo != null ?
                grandParentPathWithDot + parentPriopertyInfo.Name + "." + c.Name : "" + c.Name));
            foreach (var childPropertyInfo in allChildEntitySetProperties)
            {
                var nextGrandParentPath = rootType == type
                    ? ""
                    : $"{grandParentPathWithDot}{parentPriopertyInfo.Name}";
                Type innerType = childPropertyInfo.PropertyType.IsGenericType
                    ? childPropertyInfo.PropertyType.GenericTypeArguments[0]
                    : childPropertyInfo.PropertyType;
                if (agg.ContainsKey(innerType) == false)
                {
                    GetChildPropertiesFilteredByTables(rootType, childTablesToBeConsidered,innerType, nextGrandParentPath, childPropertyInfo, agg, aggPathList);
                }
            }
            return new Tuple<List<string>, Dictionary<Type, List<IColumnNetInfo>>>(aggPathList, agg);
        }

        private Dictionary<string, Type> GetTableNameTypeDic()
        {
            Dictionary<string, Type> retVal = new Dictionary<string, Type>();
            retVal = TableTypeMetaInfoDic
                .Select(kv => new {Type = kv.Key, TableName = kv.Value.TableDataContractMetaInfo.TableName})
                .ToLookup(k => k.TableName).ToDictionary(k => k.Key, kv => kv.First().Type);
            return retVal;
        }

        /// <summary>
        /// Gets the type of the table data contract meta information of.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public TableDataContractMetaInfoWithType GetTableDataContractMetaInfoOfType(Type type)
        {
            return TableTypeMetaInfoDic[type];
        }

        /// <summary>
        /// Gets the type of the associate parent child table information by.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public AssociatedParentChildTableInfo GetAssociateParentChildTableInfoByType(Type type)
        {
            return ORMMetadata.AssociateRootEntities.First(t => t.AssociateRootClassName == type.Name);
        }

        /// <summary>
        /// Gets the name of the type name based on table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public Type GetTypeNameBasedOnTableName(string tableName)
        {
            return TableNameTypeDic[tableName];
        }

        /// <summary>
        /// Gets the delete TSQL for type internal.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetDeleteTSQLForTypeInternal(Type type)
        {
            return DeleteTSQLGenerator.GetDeleteSQLStatementForType(this, type);
        }

        
        private static List<IColumnNetInfo> GetColumnNetInfo(Type type, List<string> propertyNames)
        {
            List<IColumnNetInfo> retVal = new List<IColumnNetInfo>();
            var propertiesDic = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToLookup(f => f.Name)
                .ToDictionary(k => k.Key, kv => kv.First());
            retVal.AddRange( propertyNames.Where(x => propertiesDic.ContainsKey(x)).Select(x => new ColumnNetInfo(propertiesDic[x]))
                .ToList());
            
            //check if all properties have been found
            if (retVal.Count > propertyNames.Count)
            {
                throw  new InvalidDataException($"The number of properties returned is more that required");
            }
            else if (propertyNames.Count > retVal.Count)
            {
                var notFoundTillEnd = string.Join(",", propertyNames.Where(p =>
                        retVal.Any(x => string.Equals(p, x.Name, StringComparison.CurrentCultureIgnoreCase)) == false)
                    .ToList());
                throw  new InvalidDataException($"properties: {notFoundTillEnd} requested not found in Type: {type.FullName}");
            }

            return retVal;
        }

        private Dictionary<Type, TableDataContractMetaInfoWithType> GetTableDataContractMetaInfoWithType()
        {
            var ormMetadata = ORMMetadata;
            var dataContractAssembly = DataContractAssembly;
            var tableContracts = ormMetadata.RootTables.SelectMany(c => c.Flatten()).ToList();
            List<TableDataContractMetaInfoWithType> typeWithTableContractTupleList = tableContracts.Select(t =>
            {
                var classType = dataContractAssembly.GetType($"{t.TableClassNameSpace}.{t.TableClassName}");
                var tableClassTypeWithResolvedProperties = string.IsNullOrEmpty(t.ClassNameWithResolvedProperties) == false ?
                    dataContractAssembly.GetType($"{t.TableClassNameSpace}.{t.ClassNameWithResolvedProperties}") : null;
                var primaryIDLookupClassType =  string.IsNullOrEmpty(t.TablePrimaryKeyLookupResolveClassName) == false 
                    ?  dataContractAssembly.GetType($"{t.TableClassNameSpace}.{t.TablePrimaryKeyLookupResolveClassName}") : null;
                var tablePrimaryKeyClassType = string.IsNullOrEmpty(t.TablePrimaryKeyClassName) == false && t.IsCompositeRootTable 
                    ?  dataContractAssembly.GetType($"{t.TableClassNameSpace}.{t.TablePrimaryKeyClassName}") : null;
                var sourceTypeResolveTypeMappingInfo =
                    SourceTypeResolveTypeMappingInfo.GetSourceTypeResolveTypeMappingInfo(dataContractAssembly,
                        ormMetadata, t);
                
                   
                TableDataContractMetaInfoWithType tableInfoWithType = null;
                var columnNetInfoList = GetColumnNetInfo(classType,
                    t.ColumnPropertiesList.Select(c => c.ColumnPropertyName).ToList());
                
                var columnPropertyMetaInfoWithTypeList = t.ColumnPropertiesList.Select(c =>
                        new ColumnPropertyMetaInfoWithType(c, columnNetInfoList.First(x => x.Name.Equals(c.ColumnPropertyName,StringComparison.InvariantCultureIgnoreCase)))).ToList();
                /*    
                var columnPropertyMetaInfoWithTypeList = t.ColumnPropertiesList.Select(c =>
                        new ColumnPropertyMetaInfoWithType(c,
                        new ColumnNetInfo(classType.GetProperties().First(p => string.Compare(p.Name, c.ColumnPropertyName, StringComparison.InvariantCultureIgnoreCase) == 0)))
                        )
                    .ToList();
                */
                
                var childPropertyMetaInfoWithTypeList = t.ChildEntitySetPropertyInfoList.Select(c =>
                    new ChildPropertyMetaInfoWithType(c, GetColumnNetInfo(classType, new List<string>(){ c.ChildEntitySetPropertyName}).First())).ToList();

                var ignorePropertiesList = GetColumnNetInfo(classType, t.IgnoreColumnsList);

                var primaryKeyPropertiesList = t.TablePrimaryKeyColumns.Any()
                    ? GetColumnNetInfo(classType, t.TablePrimaryKeyColumns) : new List<IColumnNetInfo>();


                var alwaysDBGeneratedPropertiesList = GetColumnNetInfo(classType, t.AlwaysDBGenerated); 
                    

                ParentPropertyMetaInfoWithType[] parentPropertyMetaInfoWithTypeList = null;       
                if (t.ParentPropertyMetaInfoList != null && t.ParentPropertyMetaInfoList.Any())
                {
                    parentPropertyMetaInfoWithTypeList = t.ParentPropertyMetaInfoList.Select(parentMetaInfo =>
                    {
                        var parentClassType = dataContractAssembly.GetType(parentMetaInfo.ParentClassFullName);
                        var childClassType = dataContractAssembly.GetType(parentMetaInfo.ChildClassFullName);
                        var parentPropertyMetaInfoWithType = new ParentPropertyMetaInfoWithType(
                            parentMetaInfo
                            , parentClassType
                            , childClassType
                            , GetColumnNetInfo(childClassType, new List<string>(){parentMetaInfo.EntityRefPropertyName}).First()
                            , GetColumnNetInfo(parentClassType,new List<string>(){parentMetaInfo.EntitySetPropertyName}).First()
                            , GetColumnNetInfo(parentClassType, parentMetaInfo.ParentPropertyNames).ToList()
                            , GetColumnNetInfo(childClassType,parentMetaInfo.ChildPropertyNames).ToList()
                        );
                        return parentPropertyMetaInfoWithType;
                    }).ToArray();
                    
                    /*
                    parentPropertyMetaInfoWithType = new ParentPropertyMetaInfoWithType(
                        t.ParentPropertyMetaInfo
                        , parentClassType
                        , childClassType
                        , childClassType.GetProperties().First(p => string.Compare(p.Name, t.ParentPropertyMetaInfo.EntityRefPropertyName, StringComparison.InvariantCultureIgnoreCase) == 0)
                        , parentClassType.GetProperties().First(p => string.Compare(p.Name, t.ParentPropertyMetaInfo.EntitySetPropertyName, StringComparison.InvariantCultureIgnoreCase) == 0)
                        , t.ParentPropertyMetaInfo.ParentPropertyNames.Select(parentPropertyName => parentClassType.GetProperties().First(p => String.Compare(p.Name, parentPropertyName, StringComparison.InvariantCultureIgnoreCase) == 0)).ToList()
                        , t.ParentPropertyMetaInfo.ChildPropertyNames.Select(childPropertyName => childClassType.GetProperties().First(p => String.Compare(p.Name, childPropertyName, StringComparison.InvariantCultureIgnoreCase) == 0)).ToList()
                    );
                    */
                }

                tableInfoWithType = new TableDataContractMetaInfoWithType(t, classType,
                    columnPropertyMetaInfoWithTypeList, childPropertyMetaInfoWithTypeList,
                    parentPropertyMetaInfoWithTypeList, ignorePropertiesList, alwaysDBGeneratedPropertiesList, primaryKeyPropertiesList, tableClassTypeWithResolvedProperties, primaryIDLookupClassType, tablePrimaryKeyClassType,sourceTypeResolveTypeMappingInfo
                    );
                return tableInfoWithType;
            }).ToList();
            var dic = typeWithTableContractTupleList.ToLookup(t => t.TableTypeClassInfo)
                .ToDictionary(k => k.Key, kv => kv.First());
            return dic;
        }

        private Tuple<List<string>, Dictionary<Type, List<IColumnNetInfo>>> GetChildProperties(Type rootType, Type type, string grandParentPath, IColumnNetInfo parentPriopertyInfo, Dictionary<Type, List<IColumnNetInfo>> agg, List<string> aggPathList)
        {
            var allChildEntitySetProperties = TableTypeMetaInfoDic[type].ChildPropertyMetaInfoWithTypeList.Select(n => n.ChildEntitySetPropertyInfo).ToList();
            agg.Add(type, allChildEntitySetProperties);
            var grandParentPathWithDot = String.IsNullOrEmpty(grandParentPath) == false ? $"{grandParentPath}." : grandParentPath;
            aggPathList.AddRange(allChildEntitySetProperties.Select(c => parentPriopertyInfo != null ?
                grandParentPathWithDot + parentPriopertyInfo.Name + "." + c.Name : "" + c.Name));
            foreach (var childPropertyInfo in allChildEntitySetProperties)
            {
                var nextGrandParentPath = rootType == type
                    ? ""
                    : $"{grandParentPathWithDot}{parentPriopertyInfo.Name}";
                Type innerType = childPropertyInfo.PropertyType.IsGenericType
                    ? childPropertyInfo.PropertyType.GenericTypeArguments[0]
                    : childPropertyInfo.PropertyType;
                if (agg.ContainsKey(innerType) == false)
                {
                    GetChildProperties(rootType, innerType, nextGrandParentPath, childPropertyInfo, agg, aggPathList);
                }
            }
            return new Tuple<List<string>, Dictionary<Type, List<IColumnNetInfo>>>(aggPathList, agg);
        }
        private List<string> GetImmediateChildPropertyNamesNonCached(Type type)
        {
            var allChildEntitySetProperties = TableTypeMetaInfoDic[type].ChildPropertyMetaInfoWithTypeList.Select(n => n.ChildEntitySetPropertyInfo).ToList();
            return allChildEntitySetProperties.Select(p => p.Name).ToList();
        }

        private static readonly ConcurrentDictionary<SystemDefinedColumn, string> SystemDefinedColumnTypeNameDictionary = new ConcurrentDictionary<SystemDefinedColumn, string>(
                new Dictionary<SystemDefinedColumn, string>()
                {
                    {SystemDefinedColumn.CreatedBy, "CreatedBy"}, {SystemDefinedColumn.TouchedBy, "TouchedBy"},
                    {SystemDefinedColumn.CreatedWhenUTC, "CreatedWhenUTC"},
                    {SystemDefinedColumn.TouchedWhenUTC, "TouchedWhenUTC"}
                }
            );

        /// <summary>
        /// Gets the system defined column type name dictionary.
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<SystemDefinedColumn, string> GetSystemDefinedColumnTypeNameDictionary() => SystemDefinedColumnTypeNameDictionary;

        private List<EntityColumnInfo> GetAllTableColumnPropertiesNonCached(Type type)
        {
            var tableName = GetTableNameByType(type);
            var tableType = TableNameTypeDic[tableName];
            var res = TableTypeMetaInfoDic[tableType].ColumnPropertiesWithTypeList.Select(c =>
                new EntityColumnInfo(c.ColumnPropertyMetaInfo.ColumnName, c.ColumnPropertyInfo, c.Name, c.PropertyType,
                    c.ColumnPropertyMetaInfo.SqlDataType, c.ColumnPropertyMetaInfo.IsGeneratedOnUpdate,
                    c.ColumnPropertyMetaInfo.IsGeneratedOnAdd, c.ColumnPropertyMetaInfo.IsPKColumn)).ToList();
            return res;
        }

        /// <summary>
        /// Gets all selected column properties non cached.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="selectedColumns">The selected columns.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public List<EntityColumnInfo> GetAllSelectedColumnPropertiesNonCached(Type type, List<PropertyInfo> selectedColumns)
        {
            if (selectedColumns == null || selectedColumns.Any() == false) 
                throw new ArgumentNullException($"{nameof(selectedColumns)}");
            var tableName = GetTableNameByType(type);
            var tableType = TableNameTypeDic[tableName];
            var res = TableTypeMetaInfoDic[tableType].ColumnPropertiesWithTypeList.Where(field => (selectedColumns.Exists(s => s.Name == field.ColumnPropertyInfo.Name && s.ReflectedType.Name == tableName) || field.ColumnPropertyMetaInfo.IsPKColumn)).Select(c =>
                    new EntityColumnInfo(c.ColumnPropertyMetaInfo.ColumnName, c, c.Name, c.PropertyType,
                        c.ColumnPropertyMetaInfo.SqlDataType, c.ColumnPropertyMetaInfo.IsGeneratedOnUpdate,
                        c.ColumnPropertyMetaInfo.IsGeneratedOnAdd, c.ColumnPropertyMetaInfo.IsPKColumn)).ToList();
            return res;
        }

        /// <summary>
        /// Gets all table entity set properties non cached.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public List<EntitySetPropertyInfo> GetAllTableEntitySetPropertiesNonCached(Type type)
        {
            var res = TableTypeMetaInfoDic[type].ChildPropertyMetaInfoWithTypeList.Select(
                    n => new EntitySetPropertyInfo(n.ChildEntitySetPropertyInfo
                        , n.ChildEntitySetPropertyInfo.Name
                        , GetTableContractInfoByClassFullName(n.ChildPropertyMetaInfo.ChildClassFullName).TableTypeClassInfo
                        , GetTableContractInfoByClassFullName(n.ChildPropertyMetaInfo.ChildClassFullName).TableDataContractMetaInfo.TableName
                        , TableTypeMetaInfoDic[type].TableDataContractMetaInfo.TableName
                        , TableTypeMetaInfoDic[type].TableTypeClassInfo
                        , n.ChildPropertyMetaInfo.ParentPropertyNames
                        , n.ChildPropertyMetaInfo.ChildPropertyNames
                )).ToList();
            return res;
        }

        private TableDataContractMetaInfoWithType GetTableContractInfoByClassFullName(string classFullName)
        {
            if (TableTypeMetaInfoDicByClassFullName.TryGetValue(classFullName, out var retValue))
            {
                return retValue;
            }
            else
            {
                var classFullNameLower = classFullName.ToLower();

                var keyInProperCase = TableTypeMetaInfoDicByClassFullName.Keys?.FirstOrDefault(k => k.ToLower() == classFullNameLower);
                if (keyInProperCase == null)
                {
                    throw new InvalidProgramException($"Could not find type {classFullNameLower}");
                }
                return TableTypeMetaInfoDicByClassFullName[keyInProperCase];
            }
        }

        /// <summary>
        /// Gets the entity reference property information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IColumnNetInfo[] GetEntityRefPropertyInfo(Type type)
        {
            return TableTypeMetaInfoDic[type].ParentPropertyMetaInfoWithType.Select(t => t.EntityRefPropertyInfo).ToArray();
        }

        /// <summary>
        /// Gets the child property information.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="childType">Type of the child.</param>
        /// <returns></returns>
        public IColumnNetInfo GetChildPropertyInfo(Type type, Type childType)
        {
            return TableTypeMetaInfoDic[type].ChildPropertyMetaInfoWithTypeList
                .First(c => c.ChildPropertyMetaInfo.ChildClassFullName == childType.FullName).ChildEntitySetPropertyInfo;
        }

        /// <summary>
        /// Gets the type of the table name by.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Could not get table name associated with type {type.FullName}</exception>
        public string GetTableNameByType(Type type)
        {
            if (TableTypeMetaInfoDic.ContainsKey(type))
            {
                return TableTypeMetaInfoDic[type].TableDataContractMetaInfo.TableName;
            }
            else
            {
                var retVal = this.ORMMetadata.AssociateRootEntities
                    .FirstOrDefault(t => t.AssociateRootClassFullName == type.FullName)?.AssociateRootTableName;
                if (string.IsNullOrEmpty(retVal))
                {
                    throw new InvalidOperationException($"Could not get table name associated with type {type.FullName}");
                }

                return retVal;
            }
        }

        /// <summary>
        /// Gets the primary key property information list.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public List<IColumnNetInfo> GetPrimaryKeyPropertyInfoList(Type type)
        {
            return TableTypeMetaInfoDic[type].PrimaryKeyPropertiesInfoList;
        }

        
        private string GetTSQLByXMLForDataContractNonCached(Type type)
        {
            return DBXMLUtility.GetTSQLForDataContract(type, this);
        }
        
        private string GetTSQLByJSONForDataContractNonCached(Type type)
        {
            return GetTSQLForJSONDataContract(type, this);
        }

        #region "Selected Columns"

        public string GetTSQLByXMLForSelectedColumnsNonCached(Type type, List<PropertyInfo> selectedColumns)
        {
            return DBXMLUtility.GetTSQLForSelectedColumns(type, this, selectedColumns);
        }


        public string GetTSQLByJsonForSelectedColumnsNonCached(Type type, List<PropertyInfo> selectedColumns)
        {
            return DBXMLUtility.GetTSQLForSelectedColumnsJSON(type, this, selectedColumns);
        }

        #endregion
         

        private List<Tuple<Type, Type>> GetNestedChildTableTypeNonCached(Type type)
        {
            List<Tuple<Type, Type>> GetChildTablesInternal(Type internalType, List<Tuple<Type, Type>> childTables)
            {
                var propertyInfos = GetAllTableEntitySetProperties(internalType);
                propertyInfos.ForEach(pi =>
                {
                    var childType = pi.EntitySetTableType;
                    var tuple = new Tuple<Type, Type>(childType, internalType);
                    if (childTables.Contains(tuple) == false)
                    {
                        childTables.Add(tuple);
                        GetChildTablesInternal(childType, childTables);
                    }
                });
                return childTables;
            }
            return GetChildTablesInternal(type, new List<Tuple<Type, Type>>() { new Tuple<Type, Type>(type, null) });

        }
        
        private List<ChildTypeParentType> GetNestedChildTableTypeWithEntitySetInfoNonCached(Type type)
        {
            List<ChildTypeParentType> GetChildTablesInternal(Type internalType,Type grandParentType, List<ChildTypeParentType> childTables)
            {
                var propertyInfos = GetAllTableEntitySetProperties(internalType);
                propertyInfos.ForEach(pi =>
                {
                    var childType = pi.EntitySetTableType;
                    var grandParentTableName = grandParentType != null ? GetTableName(grandParentType) : String.Empty;
                    var childTypeParentType = new ChildTypeParentType(childType, internalType,grandParentType,GetTableName(childType),GetTableName(internalType),grandParentTableName,pi);
                    if (childTables.Contains(childTypeParentType) == false)
                    {
                        childTables.Add(childTypeParentType);
                        GetChildTablesInternal(childType,internalType, childTables);
                    }
                });
                return childTables;
            }
            return GetChildTablesInternal(type,null, new List<ChildTypeParentType>() { new ChildTypeParentType(type, null,null,GetTableName(type),null,String.Empty,null)});

        }

        private string GetUDTTableTypeName(string tableName) => $"{tableName}AppDtTbl";

        private List<Tuple<string, IColumnNetInfo, EntityColumnInfo>> GetTableTypeColumnAndContractMappingNonCached(Type tableDataContractType)
        {

            var tableDataType = GetUDTTableTypeName(TableTypeMetaInfoDic[tableDataContractType].TableDataContractMetaInfo.TableName);
            var columnProperties = GetAllTableColumnProperties(tableDataContractType);
            var list = columnProperties.Select((c) =>
                new Tuple<string, IColumnNetInfo, EntityColumnInfo>(c.ColumnName, c.PropertyInfo, c)).ToList();
            list.Add(new Tuple<string, IColumnNetInfo, EntityColumnInfo>("RID", null, null));
            list.Add(new Tuple<string, IColumnNetInfo, EntityColumnInfo>("PRID", null, null));
            return list;
        }

        /// <summary>
        /// Gets the data table for bulk insert.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public DataTable GetDataTableForBulkInsert(Type type)
        {
            return GetTableColumnInfoForBulkInsert(type).Aggregate(new DataTable(GetUDTTableTypeName(TableTypeMetaInfoDic[type].TableDataContractMetaInfo.TableName)),
                (dtc, pc) =>
                {
                    dtc.Columns.Add(new DataColumn(pc.ColumnName, pc.DataTableType));
                    return dtc;
                });
        }

        /// <summary>
        /// Gets the name of the bulk insert udt table type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetBulkInsertUDTTableTypeName(Type type) => 
            GetUDTTableTypeName(GetTableName(type));

        private List<SQLTableTypeColumnInfo> GetTableColumnInfoForBulkInsertNonCached(Type type)
        {
            var columnNamePropertyInfoColumnAttributeList = GetTableTypeColumnAndContractMappingNonCached(type);
            var res = columnNamePropertyInfoColumnAttributeList.Select((t, columnPosition) =>
            {
                if (t.Item2 != null)
                {
                    var autoSync = t.Item3.IsDbGeneratedOnAdd || t.Item3.IsDbGeneratedOnUpdate;
                    var propertyInfo = t.Item3.PropertyInfo;
                    var columnAttribute = t.Item3;
                    var underlyingType = propertyInfo.PropertyType.IsGenericType ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;
                    if (underlyingType == null) throw new InvalidOperationException($"Could Not determine underlying CLR type of property {t.Item3.PropertyName} in type {type.FullName} ");
                    var isAllowedForInsert = ((columnAttribute.IsDbGeneratedOnAdd == true || columnAttribute.IsDbGeneratedOnUpdate == true) == false);
                    var dataTableType = Utility.TypeConversion.ContainsKey(underlyingType) ? Utility.TypeConversion[underlyingType].Item1 : underlyingType;

                    if (underlyingType.IsEnum)
                    {
                        dataTableType = Enum.GetUnderlyingType(underlyingType);
                    }
                    Func<object, object> convertFunc = Utility.TypeConversion.ContainsKey(underlyingType) ? Utility.TypeConversion[underlyingType].Item2 : (Func<object, object>)null;
                    return new SQLTableTypeColumnInfo(columnPosition, propertyInfo.Name, propertyInfo, columnAttribute, isAllowedForInsert, dataTableType, convertFunc, autoSync, t.Item3.IsPrimaryKey);
                }
                else
                {
                    return new SQLTableTypeColumnInfo(columnPosition, t.Item1, null, null, false, typeof(long), null, false, false);
                }
            }).ToList();
            
            return res;
            
        }
        
        private string GetBulkInsertSQLForTypeInternalNonCached(Type type)
        {
            
            var allTableList = GetNestedChildTableType(type); 
            var typeHierarchyTuple = new Tuple<Type, List<Tuple<Type, Type>>>(type, allTableList);
            var sql = GetBulkInsertStatement(typeHierarchyTuple);
            return sql;    
        }

        string GetInputTableArg(Type currentType, Type parentType) =>
            $"@{(parentType == null ? GetTableTypeName(currentType) : GetTableName(parentType) + "_" + GetTableTypeName(currentType))}InputArg";

        string GetOutputTableArg(Type currentType, Type parentType) =>
            $"@{(parentType == null ? GetTableTypeName(currentType) : GetTableName(parentType) + "_" + GetTableTypeName(currentType))}outputArg";
        
        string GetOutputSyncTableArg(Type currentType, Type parentType) =>
            $"@{(parentType == null ? GetTableTypeName(currentType) : GetTableName(parentType) + "_" + GetTableTypeName(currentType))}OutputSyncArg";
        
        string GetCurrentTypeHierarchyString(Type currentType, Type parentType) => (parentType == null ? GetTableTypeName(currentType) : GetTableName(parentType) + "_" + GetTableTypeName(currentType));
        
        private string GetTableTypeName(Type type) => $"{TableTypeMetaInfoDic[type].TableDataContractMetaInfo.TableName}AppDtTbl";
        private string GetTableName(Type type) => $"{TableTypeMetaInfoDic[type].TableDataContractMetaInfo.TableName}";
        
        const string ParentRecordIDColumnName = "PRID";
        const string RecordIDColumnName = "RID";
        private string GetBulkInsertStatement(Tuple<Type, List<Tuple<Type, Type>>> typeHierarchytuple)
        {
            void GetInsertStatementForType(StringBuilder sb, Type currentType, Type parentType, Type grandParentType, List<Tuple<string, string>> parentChildFkPropertiesTupleList)
            {
                var list = GetTableColumnInfoForBulkInsertNonCached(currentType);
                var insertedColumnList = list.Where(l => l.IsAllowedForInsert == true).ToList();
                var insertColumnExpression = insertedColumnList.Select(p =>
                    {
                        var columnName = "CT." + p.ColumnName;
                        if (parentChildFkPropertiesTupleList != null && parentChildFkPropertiesTupleList.Any())
                        {
                            var parentChildPropertyTuple = parentChildFkPropertiesTupleList.FirstOrDefault(pct => pct.Item2 == p.ColumnName);
                            columnName = parentChildPropertyTuple != null ? "PT." + parentChildPropertyTuple.Item1 : columnName;
                        }

                        return TypeConversion.ContainsKey(p.UnderlyingType) ? TypeConversion[p.UnderlyingType].Item3(columnName) : columnName;
                    })
                    .ToList();

                var currentTableInputArg = GetInputTableArg(currentType, parentType);
                var parentTableOutputSyncArg = parentType != null ? GetOutputSyncTableArg(parentType, grandParentType) : null;
                var fromExpression = parentChildFkPropertiesTupleList != null && parentChildFkPropertiesTupleList.Any()
                    ? $"{currentTableInputArg} AS CT INNER JOIN {parentTableOutputSyncArg} AS PT ON {"PT." + RecordIDColumnName + " = " + "CT." + ParentRecordIDColumnName}"
                    : currentTableInputArg + " AS CT";
                var outputTableArg = GetOutputTableArg(currentType, parentType);
                var outputColumnInfo = list.Select(p => p.PropertyInfo == null ? "NULL" : (p.UnderlyingType != null && TypeConversion.ContainsKey(p.UnderlyingType) ? TypeConversion[p.UnderlyingType].Item3("Inserted." + p.ColumnName) : "Inserted." + p.ColumnName)).ToList();
                var sql = $" INSERT INTO dbo.[{GetTableName(currentType)}] \n ({insertedColumnList.Select(p => p.ColumnName).ToList<string>().ConcatenateListOfStringUsingSeparator(",")}) \n OUTPUT {outputColumnInfo.ConcatenateListOfStringUsingSeparator(",")} \n INTO {outputTableArg} \n SELECT {insertColumnExpression.ConcatenateListOfStringUsingSeparator(",")} \n FROM {fromExpression} ORDER BY CT.RID";
                if (list.Any(p => p.IsPrimaryKey) == false)
                {
                    Console.WriteLine($"No propeary key column in {currentType.FullName}");
                }
                var identityColumnName = list.Any(p => p.IsPrimaryKey && p.IsAllowedForInsert == false) ? list.First(p => p.IsPrimaryKey && p.IsAllowedForInsert == false).ColumnName : list.First(p => p.IsPrimaryKey ).ColumnName;
                var currentTableSynOutputTableArg = GetOutputSyncTableArg(currentType, parentType); //"@" + GetTableTypeName(currentType)+"OutputSyncArg";
                var commaSeparatedListOfColumns =
                    list.Select(p => p.ColumnName).ToList().ConcatenateListOfStringUsingSeparator(",");

                //verify inserted count to be same as what was expected
                var verifySql =
                    $@" DECLARE @actualInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)} int
                                                             DECLARE @tobeInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)} int
                                                             SELECT @tobeInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)} = COUNT(0) FROM {currentTableInputArg}
                                                             SELECT @actualInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)} = COUNT(0) FROM {outputTableArg}
                                                             IF @tobeInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)} <> @actualInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)}
                                                             BEGIN
                                                             RAISERROR (N'Mismatch in inserted records in table %s. Actual Records Inserted: %d , Records To Be Inserted : %d', -- Message text.
                                                                       16, -- Severity,
                                                                       1, -- State,
                                                                       N'{GetCurrentTypeHierarchyString(currentType, parentType)}', -- First argument.
                                                                       @actualInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)}, 
		                                                               @tobeInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)}); -- third argument.
                                                             END";

                var sqlSync =
                    $"INSERT INTO {currentTableSynOutputTableArg} \n SELECT {list.Select(p => (p.ColumnName != RecordIDColumnName && p.ColumnName != ParentRecordIDColumnName) ? "A." + p.ColumnName : "B." + p.ColumnName).ToList().ConcatenateListOfStringUsingSeparator(",")} \n FROM (SELECT ROW_NUMBER() OVER (ORDER BY {identityColumnName}) RN,{commaSeparatedListOfColumns} FROM {outputTableArg}) A \n INNER JOIN (SELECT ROW_NUMBER() OVER (ORDER BY {RecordIDColumnName}) RN, * FROM {currentTableInputArg}) B ON  A.RN = B.RN ORDER BY B.RID";

                //verify sync table inserted count to be same as what was expected
                var verifySyncSql = $@" 
                                                             DECLARE @syncInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)} int
                                                             SELECT @syncInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)} = COUNT(0) FROM {currentTableSynOutputTableArg}
                                                             IF @tobeInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)} <> @syncInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)}
                                                             BEGIN
                                                             RAISERROR (N'Mismatch in inserted records in sync table %s. Actual Records Inserted: %d , Records To Be Inserted : %d', -- Message text.
                                                                       16, -- Severity,
                                                                       1, -- State,
                                                                       N'{GetCurrentTypeHierarchyString(currentType, parentType)}', -- First argument.
                                                                       @syncInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)}, 
		                                                               @tobeInsertedCount{GetCurrentTypeHierarchyString(currentType, parentType)}); -- third argument.
                                                             END
                                                              ";

                var currentTableOutputSqlDeclaration =
                    $"DECLARE {outputTableArg} AS {GetTableTypeName(currentType)}";
                var currentTableSyncOutputSqlDeclaration =
                    $"DECLARE {currentTableSynOutputTableArg} AS {GetTableTypeName(currentType)}";
                sb.AppendLine(currentTableOutputSqlDeclaration);
                sb.AppendLine(currentTableSyncOutputSqlDeclaration);
                sb.AppendLine();
                sb.AppendLine(sql);
                sb.AppendLine();
                sb.AppendLine(verifySql);
                sb.AppendLine();
                sb.AppendLine(sqlSync);
                sb.AppendLine();
                sb.AppendLine(verifySyncSql);
                sb.AppendLine();
                sb.AppendLine();
                sb.AppendLine();
            }

            //create an insert statement for type
            void InsertStatementBuilderFunc(StringBuilder sb, Type currentType, Type parentType, Type grandParentType, List<Tuple<string, string>> sourceDestinationPropertyPairList,HashSet<Type> fetchedTypes)
            {
                if (fetchedTypes.Contains(currentType) == true)
                {
                    return;
                }

                fetchedTypes.Add(currentType);
                var propertyInfos = GetAllTableEntitySetProperties(currentType);
                GetInsertStatementForType(sb, currentType, parentType, grandParentType, sourceDestinationPropertyPairList);
                propertyInfos.ForEach(pi =>
                {
                    var childContractType = pi.EntitySetTableType;
                    var thisPropertyInfos = pi.ParentTableColumnNames;
                    var otherPropertyInfos = pi.ChildTableColumnNames;
                    var sourceDestinationPropertyList = thisPropertyInfos.Zip(otherPropertyInfos, (sp, dp) => new Tuple<string, string>(sp, dp)).ToList();
                    InsertStatementBuilderFunc(sb, childContractType, currentType, parentType, sourceDestinationPropertyList,fetchedTypes);
                });
            }


            //insert declare statement with comment for input args
            var type = typeHierarchytuple.Item1;
            var allTableList = typeHierarchytuple.Item2; 
            var commentedDeclaration = allTableList.Select(p =>
                $"-- DECLARE {GetInputTableArg(p.Item1, p.Item2)}  {GetTableTypeName(p.Item1)}").ToList().ConcatenateListOfStringUsingSeparator(" \n");
            var finalSelectStatement = allTableList.Select(p =>
                $" SELECT * FROM {GetOutputSyncTableArg(p.Item1, p.Item2)}").ToList().ConcatenateListOfStringUsingSeparator(" \n");


            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(commentedDeclaration);
            InsertStatementBuilderFunc(stringBuilder, type, null, null, null, new HashSet<Type>());
            stringBuilder.AppendLine(finalSelectStatement);
            return stringBuilder.ToString();

        }
         
        //sbQuery.Append("'\\/Date(1406831400000+0530)\\/'" + " AS " + node.PropertyName + " ");
        private static string GetTSQLForJSONDataContract(Type type, ORMModelMetaInfo modelMetaInfo)
        {
            StringBuilder FuncRec(Type tableDataContractType, bool isRootTableContract, StringBuilder sbQuery, HashSet<Type> fetchedType)
            {
                if (fetchedType.Contains(tableDataContractType))
                {
                    return sbQuery;
                }
                fetchedType.Add(tableDataContractType);
                var tableName = modelMetaInfo.GetTableNameByType(tableDataContractType);
                var childProperties = modelMetaInfo.GetAllTableColumnProperties(tableDataContractType).ToList();
                var childEntitySetPropertyNames = modelMetaInfo.GetAllTableEntitySetProperties(tableDataContractType).ToList();
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
                    sbQuery.Append(isFirst == false ? " , " : " SELECT ");
                    isFirst = false;
                    if (node.NodeType == "C")
                    {
                        if (String.IsNullOrEmpty(node.DbType) == false && node.DbType.ToLower().Contains("xml") == true)
                        {
                            sbQuery.Append($"CONVERT(varchar(max),{node.Name}) AS {node.PropertyName} ");
                        }
                        else if (String.IsNullOrEmpty(node.DbType) == false )
                        {
                            sbQuery.Append($"{node.Name} AS {node.PropertyName} ");
                        }
                    }
                    else //for child table
                    {
                        sbQuery.Append(" ( ");
                        FuncRec(node.ChildType, false, sbQuery,fetchedType); //recursive call for child table
                        sbQuery.Append($" FROM {node.ChildTableName} WHERE {node.ChildColumns.Zip<string, string, string>(node.ThisColumns, (f, s) => String.Format("{0}.{1}={2}.{3}", node.ChildTableName, f, tableDataContractType.Name, s)).ToList().ConcatenateListOfStringUsingSeparator(" AND ")} FOR JSON PATH ) AS '{node.Name}' ");
                    }
                }
                
                if (isRootTableContract == true)
                {
                    sbQuery.Append(" FROM " + tableDataContractType.Name);
                }

                return sbQuery;
            }

            return FuncRec(type, true, new StringBuilder(), new HashSet<Type>()).ToString();
        }
        private string GetTSQLQueryForAssociateJSONResultSetNonCached(Type tableClrType)
        {
            
            StringBuilder FuncRec(Type tableDataContractType, bool isRootTableContract, StringBuilder sbQuery, HashSet<Type> fetchedType)
            {
                if (fetchedType.Contains(tableDataContractType))
                {
                    return sbQuery;
                }

                fetchedType.Add(tableDataContractType);
                var compositeAssociateInfo = GetAssociateParentChildTableInfoByType(tableDataContractType);
                var tableName = GetTableNameByType(tableDataContractType);
                var childProperties = GetAllTableColumnProperties(tableDataContractType).ToList();
                var associateChildEntitySetPropertyNames = compositeAssociateInfo.AssociateChildrenInfo.ToList();
                var sortedNodes = childProperties.Select(c => new
                    {
                        NodeType = "C",
                        Name = $"{tableName}.{c.ColumnName}",
                        PropertyName = c.PropertyName,
                        DbType = c.DbType,
                        ChildTableName = String.Empty,
                        JoinCondition = String.Empty,
                        ChildTableDataContractType = (Type)null
                    })
                    .Concat(associateChildEntitySetPropertyNames.Select(c => new
                    {
                        NodeType = "N",
                        Name = c.AssociatePropertyName,
                        PropertyName = c.AssociatePropertyName,
                        DbType = "",
                        ChildTableName = c.AssociateTableName,
                        JoinCondition = c.AssociateTableJoinCondition,
                        ChildTableDataContractType  = GetTypeNameBasedOnTableName(c.AssociateTableName)
                    }))
                    .OrderBy(c => c.PropertyName, StringComparer.Ordinal)
                    .ToList();
                if (isRootTableContract) sbQuery.Append($"");
                var isFirst = true;
                foreach (var node in sortedNodes) //iterate over the columns and child tables and make it part of select statement
                {
                    sbQuery.Append(isFirst == false ? " , " : " SELECT ");
                    isFirst = false;
                    if (node.NodeType == "C")
                    {
                        if (string.IsNullOrEmpty(node.DbType) == false && node.DbType.ToLower().Contains("xml") == true)
                        {
                            sbQuery.Append($"CONVERT(varchar(max),{node.Name}) AS {node.PropertyName} ");
                        }
                        else if (string.IsNullOrEmpty(node.DbType) == false )
                        {
                            sbQuery.Append($"{node.Name} AS {node.PropertyName} ");
                        }
                        
                        
                    }
                    else //for child table
                    {
                        //sbQuery.Append(" ( ");
                        //FuncRec(node.ChildType, false, sbQuery); //recursive call for child table
                        //sbQuery.Append($" FROM {node.ChildTableName} WHERE {node.ChildColumns.Zip<string, string, string>(node.ThisColumns, (f, s) => string.Format("{0}.{1}={2}.{3}", node.ChildTableName, f, tableDataContractType.Name, s)).ToList().ConcatenateListOfStringUsingSeparator(" AND ")} FOR JSON PATH ) AS '{node.Name}' ");
                        sbQuery.Append(" ( ");
                        sbQuery.Append( DBXMLUtility.GetTSQLQueryForJSONResultSet(node.ChildTableDataContractType, this,node.JoinCondition,string.Empty));
                        sbQuery.Append($" ) AS '{node.PropertyName}' ");
                    }
                }
                
                if (isRootTableContract == true)
                {
                    sbQuery.Append(" FROM " + tableName);
                }

                return sbQuery;
            }

            return FuncRec(tableClrType, true, new StringBuilder(),new HashSet<Type>()).ToString();
        }

        /// <summary>
        /// Gets the TSQL query for associate entities filtered by where clause.
        /// </summary>
        /// <param name="associateRootEntityType">Type of the associate root entity.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="joinClause">The join clause.</param>
        /// <returns></returns>
        public string GetTSQLQueryForAssociateEntitiesFilteredByWhereClause(Type associateRootEntityType,
            string whereClause, string? joinClause = null)
        {
            var sb = new StringBuilder(GetTSQLQueryForAssociateJSONResultSet(associateRootEntityType));
            sb.AppendLine($" WHERE 1= 1 AND {whereClause}");
            sb.AppendLine(" FOR JSON PATH  ");
            return sb.ToString();
        }

        private string GetTSQLSelectQueryForDataContractWithoutChild(Type type, ORMModelMetaInfo modelMetaInfo)
        {
            var tableName = GetTableNameByType(type);
            var childProperties = GetAllTableColumnProperties(type).ToList();
            var columnsSeparatedByComma = string.Join(",",childProperties.Select(c => $"[{tableName}].[{c.ColumnName}]"));
            var tableSelectQuery = $"SELECT {columnsSeparatedByComma} FROM {tableName}";
            return tableSelectQuery;
        }

        /// <summary>
        /// The get TSQL select query for data contract with child cached
        /// </summary>
        public static readonly Func<Tuple<Type, ORMModelMetaInfo>, string> GetTSQLSelectQueryForDataContractWithChildCached =
            DBUtility.Memonize<Tuple<Type, ORMModelMetaInfo>, string>(
                (_, tuple) =>
                {
                    QueryInfo GetTSQLSelectLocal(Type rootType,EntitySetPropertyInfo entitySetPropertyInfo,  ORMModelMetaInfo modelMetaInfolocal, QueryInfo queryInfo)
                    {
                        if (entitySetPropertyInfo != null)
                        {
                            var listOfAllColumns = modelMetaInfolocal.GetAllTableColumnProperties(entitySetPropertyInfo.EntitySetTableType).Select(c => $"[{entitySetPropertyInfo.EntitySetTableName}].[{c.ColumnName}]").ToList();
                            queryInfo.AllColumns.AddRange(listOfAllColumns);
                            var joinCondition = "\r\n LEFT JOIN {entitySetPropertyInfo.EntitySetTableName} ON " + string.Join(" \r\n AND ",entitySetPropertyInfo.ChildTableColumnNames.Zip(entitySetPropertyInfo.ParentTableColumnNames
                                                    ,(fs,snd) => $"[{entitySetPropertyInfo.EntitySetTableName}].[{fs}] = [{entitySetPropertyInfo.ParentTableName}].[snd]")) ;
                            queryInfo.AllJoins.Add(joinCondition);
                            modelMetaInfolocal.GetAllTableEntitySetProperties(entitySetPropertyInfo.EntitySetTableType).ForEach(c => GetTSQLSelectLocal(rootType,c,modelMetaInfolocal,queryInfo)); 
                        }
                        else
                        {
                            var tableName = modelMetaInfolocal.GetTableNameByType(rootType);
                            queryInfo.AllColumns.AddRange(modelMetaInfolocal.GetAllTableColumnProperties(rootType).Select(c => $"[{tableName}].[{c.ColumnName}]").ToList());
                            modelMetaInfolocal.GetAllTableEntitySetProperties(rootType).ForEach(c => GetTSQLSelectLocal(rootType,c,modelMetaInfolocal,queryInfo));
                        }

                        return queryInfo;
                    }

                    var resultQueryInfo = GetTSQLSelectLocal(tuple.Item1, null, tuple.Item2, new QueryInfo(tuple.Item2.GetTableNameByType(tuple.Item1)));
                    return
                        $"SELECT {string.Join(",", resultQueryInfo.AllColumns)} FROM [{resultQueryInfo.RootTableName}] {string.Join(" ", resultQueryInfo.AllJoins)}";
                });

        /// <summary>
        /// Gets the TSQL select query for data contract with child.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string GetTSQLSelectQueryForDataContractWithChild(Type type) 
            => GetTSQLSelectQueryForDataContractWithChildCached(new Tuple<Type, ORMModelMetaInfo>(type, this));
    }

    internal class QueryInfo
    {
        internal List<string> AllColumns { get; }
        internal List<string> AllJoins { get; }

        internal string RootTableName { get; }

        internal QueryInfo(string rootTableName)
        {
            AllColumns = new List<string>();
            AllJoins = new List<string>();
            RootTableName = rootTableName;
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