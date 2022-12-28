using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    public class ResolvedTableDataContractMetaInfo
    {
        public readonly TableDataContractMetaInfo TableDataContractBeingResolved;
        public readonly List<ColumnPropertyMetaInfo> PrimitiveColumns;
        public readonly List<ResolveTableLookUpWithTypeNameInfo> ResolvedTableLookupPropertiesInfoList;
        public readonly List<ResolvedChildEntitySetPropertyInfo> ResolvedChildEntitySetPropertyInfoList;
        public readonly ResolvedEntityRefPropertyInfo ResolvedEntityRefPropertyInfo;

        public ResolvedTableDataContractMetaInfo(TableDataContractMetaInfo tableDataContractBeingResolved, List<ColumnPropertyMetaInfo> primitiveColumns, List<ResolveTableLookUpWithTypeNameInfo> resolvedPropertiesInfoList, List<ResolvedChildEntitySetPropertyInfo> resolvedChildEntitySetPropertyInfoList, ResolvedEntityRefPropertyInfo resolvedEntityRefPropertyInfo)
        {
            TableDataContractBeingResolved = tableDataContractBeingResolved ?? throw new ArgumentNullException($"{nameof(tableDataContractBeingResolved)}");
            PrimitiveColumns = primitiveColumns;
            ResolvedTableLookupPropertiesInfoList = resolvedPropertiesInfoList;
            ResolvedChildEntitySetPropertyInfoList = resolvedChildEntitySetPropertyInfoList;
            ResolvedEntityRefPropertyInfo = resolvedEntityRefPropertyInfo;
        }

        public static ResolvedTableDataContractMetaInfo GetResolvedTableDataContractMetaInfo(ORMMetadata ormMetadata, TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            return new ResolvedTableDataContractMetaInfo(tableDataContractMetaInfo
                ,tableDataContractMetaInfo.WithoutLookupColumnProperties
                ,tableDataContractMetaInfo.ResolveLookUps?.ResolveLookups?.
                                    Where(c => c is ResolveTableLookUpInfo)?.
                                        Cast<ResolveTableLookUpInfo>().Select(d => new ResolveTableLookUpWithTypeNameInfo(d
                                        ,ormMetadata.TableNameDictionary[d.TableName].TablePrimaryKeyLookupResolveClassName
                                        ,ormMetadata.TableNameDictionary[d.TableName].TablePrimaryKeyClassName))
                                    ?.ToList()
                ,tableDataContractMetaInfo.ChildEntitySetPropertyInfoList?.Select(c => 
                    new ResolvedChildEntitySetPropertyInfo(c,c.ChildEntitySetPropertyName
                        ,$"List<{ormMetadata.GeResolveClassNameOrClassName(c.ChildTableName)}>"
                        , ormMetadata.GeResolveClassNameOrClassName(c.ChildTableName)
                        , c.ChildClassNamespace
                        , c.EntityRefPropertyName
                        )
                ).ToList()
                , tableDataContractMetaInfo.ParentPropertyMetaInfo != null 
                    ? new ResolvedEntityRefPropertyInfo(
                        tableDataContractMetaInfo.ParentPropertyMetaInfo
                        ,tableDataContractMetaInfo.ParentPropertyMetaInfo.EntityRefPropertyName
                        ,ormMetadata.GeResolveClassNameOrClassName(tableDataContractMetaInfo.ParentPropertyMetaInfo.ParentTableName)
                        ,tableDataContractMetaInfo.ParentPropertyMetaInfo.ParentClassNamespace) 
                    : null
                );
        }
    }

    public class ResolveTableLookUpWithTypeNameInfo
    {
        public readonly ResolveTableLookUpInfo ResolveTableLookUpInfoBeingResolved;
        public readonly string LookupResolveClassName;
        public readonly string LookupResolvePrimaryKeyClassName;


        public ResolveTableLookUpWithTypeNameInfo(ResolveTableLookUpInfo resolveTableLookUpInfoBeingResolved,
            string lookupResolveClassName, string lookupResolvePrimaryKeyClassName)
        {
            ResolveTableLookUpInfoBeingResolved = resolveTableLookUpInfoBeingResolved ??
                                                  throw new ArgumentNullException(
                                                      nameof(resolveTableLookUpInfoBeingResolved));
            LookupResolveClassName =
                lookupResolveClassName ?? throw new ArgumentNullException(nameof(lookupResolveClassName));
            LookupResolvePrimaryKeyClassName = lookupResolvePrimaryKeyClassName ??
                                               throw new ArgumentNullException(
                                                   nameof(lookupResolvePrimaryKeyClassName));
        }
    }
    
    public class ResolvedChildEntitySetPropertyInfo
    {
        public readonly ChildPropertyMetaInfo ChildPropertyInfoBeingResolved;
        public readonly string ResolvedChildEntitySetPropertyName;
        public readonly string ResolvedChildEntitySetPropertyTypeName;
        public readonly string ResolvedChildEntitySetPropertyClassName;
        public readonly string ResolvedChildEntitySetPropertyClassNamespace;
        public readonly string ResolvedEntityRefPropertyName;

        public ResolvedChildEntitySetPropertyInfo(ChildPropertyMetaInfo childPropertyInfoBeingResolved, string resolvedChildEntitySetPropertyName,string resolvedChildEntitySetPropertyTypeName ,string resolvedChildEntitySetPropertyClassName, string resolvedChildEntitySetPropertyClassNamespace, string resolvedEntityRefPropertyName)
        {
            ChildPropertyInfoBeingResolved = childPropertyInfoBeingResolved ?? throw new ArgumentNullException($"{nameof(childPropertyInfoBeingResolved)}");
            ResolvedChildEntitySetPropertyName = resolvedChildEntitySetPropertyName;
            ResolvedChildEntitySetPropertyTypeName = resolvedChildEntitySetPropertyTypeName;
            ResolvedChildEntitySetPropertyClassName = resolvedChildEntitySetPropertyClassName;
            ResolvedChildEntitySetPropertyClassNamespace = resolvedChildEntitySetPropertyClassNamespace;
            ResolvedEntityRefPropertyName = resolvedEntityRefPropertyName;
        }
    }
    
    public class ResolvedChildEntitySetPropertyInfoWithType
    {
        public readonly ResolvedChildEntitySetPropertyInfo ResolvedChildEntitySetPropertyInfoWithoutType; 
        public readonly ChildPropertyMetaInfoWithType ChildPropertyInfoBeingResolvedWithType;
        public readonly PropertyInfo ResolvedChildEntitySetPropertyInfo;
        public readonly PropertyInfo ResolvedEntityRefPropertyInfo;

        public ResolvedChildEntitySetPropertyInfoWithType(ResolvedChildEntitySetPropertyInfo resolvedChildEntitySetPropertyInfoWithoutType, ChildPropertyMetaInfoWithType childPropertyInfoBeingResolvedWithType, PropertyInfo resolvedChildEntitySetPropertyInfo,string resolvedChildEntitySetPropertyTypeName ,PropertyInfo resolvedEntityRefPropertyInfo)
        {
            ResolvedChildEntitySetPropertyInfoWithoutType = resolvedChildEntitySetPropertyInfoWithoutType;
            ChildPropertyInfoBeingResolvedWithType = childPropertyInfoBeingResolvedWithType ?? throw new ArgumentNullException($"{nameof(childPropertyInfoBeingResolvedWithType)}");
            ResolvedChildEntitySetPropertyInfo = resolvedChildEntitySetPropertyInfo;
            ResolvedEntityRefPropertyInfo = resolvedEntityRefPropertyInfo;
        }
    }
    
    public class ResolvedEntityRefPropertyInfo      
    {
        public readonly ParentPropertyMetaInfo ParentPropertyMetaInfoBeingResolved;
        public readonly string ResolvedEntityRefPropertyName;
        public readonly string ResolvedEntityRefPropertyClassName;
        public readonly string ResolvedEntityRefPropertyClassNamespace;

        public ResolvedEntityRefPropertyInfo(ParentPropertyMetaInfo parentPropertyMetaInfoBeingResolved, string resolvedEntityRefPropertyName, string resolvedEntityRefPropertyClassName, string resolvedEntityRefPropertyClassNamespace)
        {
            ParentPropertyMetaInfoBeingResolved = parentPropertyMetaInfoBeingResolved ?? throw new ArgumentNullException($"{nameof(parentPropertyMetaInfoBeingResolved)}");
            ResolvedEntityRefPropertyName = resolvedEntityRefPropertyName;
            ResolvedEntityRefPropertyClassName = resolvedEntityRefPropertyClassName;
            ResolvedEntityRefPropertyClassNamespace = resolvedEntityRefPropertyClassNamespace;
        }
    }

    public class SourceTypeResolveTypeMappingInfo
    {
        public readonly Type SourceType;
        public readonly Type ResolveType;
        public readonly List<PrimitiveSourceResolvePropertyMappingInfo> PrimitiveSourceResolvePropertyMappingInfoList;
        public readonly List<ResolvedPropertyWithTypeInfo> ResolvedPropertyWithTypeInfoList;
        public readonly List<EntitySetResolvePropertyMappingInfo> EntitySetResolvePropertyMappingInfoList;

        public SourceTypeResolveTypeMappingInfo(Type sourceType, Type resolveType, List<PrimitiveSourceResolvePropertyMappingInfo> primitiveSourceResolvePropertyMappingInfoList, List<ResolvedPropertyWithTypeInfo> resolvedPropertyWithTypeInfoList, List<EntitySetResolvePropertyMappingInfo> entitySetResolvePropertyMappingInfoList)
        {
            SourceType = sourceType;
            ResolveType = resolveType;
            PrimitiveSourceResolvePropertyMappingInfoList = primitiveSourceResolvePropertyMappingInfoList;
            ResolvedPropertyWithTypeInfoList = resolvedPropertyWithTypeInfoList ?? new List<ResolvedPropertyWithTypeInfo>();
            EntitySetResolvePropertyMappingInfoList = entitySetResolvePropertyMappingInfoList;
        }

        public static SourceTypeResolveTypeMappingInfo GetSourceTypeResolveTypeMappingInfo(
            Assembly dataContractAssembly, ORMMetadata ormMetadata, TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            var classType = dataContractAssembly.GetType($"{tableDataContractMetaInfo.TableClassNameSpace}.{tableDataContractMetaInfo.TableClassName}");
            var tableClassTypeWithResolvedProperties = string.IsNullOrEmpty(tableDataContractMetaInfo.ClassNameWithResolvedProperties) == false ?
                dataContractAssembly.GetType($"{tableDataContractMetaInfo.TableClassNameSpace}.{tableDataContractMetaInfo.ClassNameWithResolvedProperties}") : null;
            
            if (tableClassTypeWithResolvedProperties != null )
            {
                var resolvedTableDataContractMetaInfo = ResolvedTableDataContractMetaInfo.GetResolvedTableDataContractMetaInfo(ormMetadata, tableDataContractMetaInfo);
                
                var primitiveSourceResolvePrimitivePropertyMappingList = resolvedTableDataContractMetaInfo
                    .PrimitiveColumns.Select(c => 
                            new PrimitiveSourceResolvePropertyMappingInfo(classType.GetProperty(c.ColumnPropertyName)
                                                                            ,tableClassTypeWithResolvedProperties.GetProperty(c.ColumnPropertyName)))
                    .ToList();
                
                var resolvedPropertyWithTypeInfoList = resolvedTableDataContractMetaInfo.ResolvedTableLookupPropertiesInfoList?.Select(r =>
                          new ResolvedPropertyWithTypeInfo(dataContractAssembly.GetType(ormMetadata.TableNameDictionary[r.ResolveTableLookUpInfoBeingResolved.TableName].TableClassFullName), tableClassTypeWithResolvedProperties.GetProperty(r.ResolveTableLookUpInfoBeingResolved.ResolvedPropertyName)
                              ,dataContractAssembly.GetType($"{tableDataContractMetaInfo.TableClassNameSpace}.{r.LookupResolveClassName}")
                        , r.ResolveTableLookUpInfoBeingResolved.ColumnMaps.Select(c => classType.GetProperty(c.ColumnName)).ToList()
                        , r.ResolveTableLookUpInfoBeingResolved.ColumnMaps.Select(c => dataContractAssembly.GetType($"{tableDataContractMetaInfo.TableClassNameSpace}.{r.LookupResolvePrimaryKeyClassName}").GetProperty(c.PrimaryKeyColumnName)).ToList()
                        , dataContractAssembly.GetType($"{tableDataContractMetaInfo.TableClassNameSpace}.{r.LookupResolvePrimaryKeyClassName}"
                              )
                    ))
                    .ToList();
                var entitySetResolvePropertyMappingInfoList =
                    resolvedTableDataContractMetaInfo.ResolvedChildEntitySetPropertyInfoList?.Select(r =>
                        new EntitySetResolvePropertyMappingInfo(classType.GetProperty(r.ChildPropertyInfoBeingResolved.ChildEntitySetPropertyName)
                            ,tableClassTypeWithResolvedProperties.GetProperty(r.ResolvedChildEntitySetPropertyName) 
                            ,dataContractAssembly.GetType( r.ChildPropertyInfoBeingResolved.ChildClassFullName)
                            ,dataContractAssembly.GetType( $"{r.ResolvedChildEntitySetPropertyClassNamespace}.{r.ResolvedChildEntitySetPropertyClassName}")
                            )).ToList();
                return new SourceTypeResolveTypeMappingInfo(classType,tableClassTypeWithResolvedProperties,primitiveSourceResolvePrimitivePropertyMappingList,resolvedPropertyWithTypeInfoList,entitySetResolvePropertyMappingInfoList);
            }
            else
            {
                return null;
            }

        }
        
    }
    
    public class PrimitiveSourceResolvePropertyMappingInfo
    {
        public readonly PropertyInfo SourcePropertyInfo;
        public readonly PropertyInfo ResolvePropertyInfo;

        public PrimitiveSourceResolvePropertyMappingInfo(PropertyInfo sourcePropertyInfo, PropertyInfo resolvePropertyInfo)
        {
            SourcePropertyInfo = sourcePropertyInfo;
            ResolvePropertyInfo = resolvePropertyInfo;
        }
    }

    public class ResolvedPropertyWithTypeInfo
    {
        public readonly PropertyInfo ResolvePropertyInfo;
        public readonly Type PrimaryIDLookupClassType;
        public readonly Type MainLookupTable;
        public readonly List<PropertyInfo> PropertiesBeingResolvedList;
        public readonly List<PropertyInfo> PrimaryKeyClassPropertyList;
        public readonly Type PrimaryKeyClassType;
        public readonly ConstructorInfo PrimaryKeyClassConstructor;
        public readonly Type ListTypeOfPrimaryIDLookupClassType;
        public readonly Func<string, object> DeserializePrimaryIDLookupJSONToListOfPrimaryIDLookup;
        //public readonly TableDataContractMetaInfoWithType MainLookupTableTableDataContractMetaInfoWithType;

        public ResolvedPropertyWithTypeInfo(Type mainLookupClassType, PropertyInfo resolvePropertyInfo,Type primaryIDLookupClassType, List<PropertyInfo> propertiesBeingResolvedList, List<PropertyInfo> primaryKeyClassPropertyList, Type primaryKeyClassType)
        {
            //MainLookupTableTableDataContractMetaInfoWithType = mainLookupTableTableDataContractMetaInfoWithType;
            MainLookupTable = mainLookupClassType; //MainLookupTableTableDataContractMetaInfoWithType.TableTypeClassInfo;
            ResolvePropertyInfo = resolvePropertyInfo;
            PrimaryIDLookupClassType = primaryIDLookupClassType;
            PrimaryKeyClassType = primaryKeyClassType;
            //MainLookupTable = PrimaryKeyClassType.GetInterface(typeof(IPrimaryKeyBase<>).FullName).GenericTypeArguments.ToList().First();//table type to which this PK type belongs to
            PropertiesBeingResolvedList = propertiesBeingResolvedList;
            PrimaryKeyClassPropertyList = primaryKeyClassPropertyList;
            PrimaryKeyClassConstructor = primaryKeyClassType != null
                ? primaryKeyClassType.GetConstructor(PrimaryKeyClassPropertyList.Select(c => c.PropertyType).ToArray())
                : null;
            if (PrimaryKeyClassType != null && PrimaryKeyClassConstructor == null && PrimaryKeyClassPropertyList.Any() == false)
            {
                throw new InvalidOperationException($"Could not find primary key class: {PrimaryKeyClassType.FullName} constructor which takes {PrimaryKeyClassPropertyList?.Count} parameters");
            }

            ListTypeOfPrimaryIDLookupClassType = typeof(List<>).MakeGenericType(new Type[] {PrimaryIDLookupClassType});
            DeserializePrimaryIDLookupJSONToListOfPrimaryIDLookup = (string jsonText) =>
                typeof(DataContractSerializerHelper)
                    .GetMethod("DataContractJSONDeserializer", new Type[] {typeof(string)})
                    ?.MakeGenericMethod(new Type[] {ListTypeOfPrimaryIDLookupClassType})
                    .Invoke(null, new object[] {jsonText});
        }

        public object GetPrimaryKeyValues(object source)
        {
            if(source == null) throw new ArgumentNullException($"{nameof(source)}");
            var sourcePropertyValuesBeingResolved = PropertiesBeingResolvedList.Select(pr => pr.GetValue(source)).ToList();
            var primaryKeyClassObject = this.PrimaryKeyClassConstructor.Invoke(sourcePropertyValuesBeingResolved.ToArray()) as IPrimaryKeyBase;
            return primaryKeyClassObject;
        }

        public List<object> GetPrimaryKeyListValues(List<object> sourceList)
        {
            return sourceList?.Where(c => c != null)?.Select(GetPrimaryKeyValues).ToList();
        }
    }

    public class EntitySetResolvePropertyMappingInfo
    {
        public readonly PropertyInfo SourceEntitySetPropertyInfo;
        public readonly PropertyInfo ResolveEntitySetPropertyInfo;
        public readonly Type SourceEntitySetClassType;		
        public readonly Type ResolveEntitySetClassType;

        public EntitySetResolvePropertyMappingInfo(PropertyInfo sourceEntitySetPropertyInfo, PropertyInfo resolveEntitySetPropertyInfo, Type sourceEntitySetClassType, Type resolveEntitySetClassType)
        {
            SourceEntitySetPropertyInfo = sourceEntitySetPropertyInfo;
            ResolveEntitySetPropertyInfo = resolveEntitySetPropertyInfo;
            SourceEntitySetClassType = sourceEntitySetClassType;
            ResolveEntitySetClassType = resolveEntitySetClassType;
        }
    }


}
