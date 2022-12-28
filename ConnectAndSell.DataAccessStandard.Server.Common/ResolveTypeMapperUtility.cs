using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    public static class ResolveTypeMapperUtility
    {
        private static object PopulateResolveInfo(ResolverProvider resolveProvider,IApplicationRepositoryBase repoBase,ORMModelMetaInfo metaModelInfo,object source,Type sourceType,Type resolveType)
        {
            var sourceTypeTableMetaInfo = metaModelInfo.TableTypeMetaInfoDic[sourceType];
            if (source == null)
            {
                return null;
            }
            var retVal = resolveType.GetConstructor(new Type[] { })?.Invoke(null);//TODO - Store the constructor in SourceTypeResolveTypeMappingInfo
            if (retVal == null)
            {
                throw new InvalidOperationException($"Cant create a instance of type {resolveType.FullName} using default constructor");
            }
            foreach (var primitiveSourceResolvePropertyMappingInfo in sourceTypeTableMetaInfo.SourceTypeResolveTypeMappingInfo.PrimitiveSourceResolvePropertyMappingInfoList)
            {
                primitiveSourceResolvePropertyMappingInfo.ResolvePropertyInfo.SetValue(retVal,primitiveSourceResolvePropertyMappingInfo.SourcePropertyInfo.GetValue(source));
            }

            foreach (var entitySetResolvePropertyMappingInfo in sourceTypeTableMetaInfo.SourceTypeResolveTypeMappingInfo.EntitySetResolvePropertyMappingInfoList)
            {
                if (entitySetResolvePropertyMappingInfo.SourceEntitySetPropertyInfo.GetValue(source) is IEnumerable sourceChildList)
                {
                    IList destinationChildList = typeof(List<>)
                        .MakeGenericType(entitySetResolvePropertyMappingInfo.ResolveEntitySetClassType)
                        ?.GetConstructor(new Type[] { })?.Invoke(null) as IList;
                    if (destinationChildList == null)
                    {
                        throw new InvalidOperationException($"Cant create an instance of List<{entitySetResolvePropertyMappingInfo.ResolveEntitySetClassType.FullName}>");
                    }
                    foreach (var sourceChildObject in sourceChildList)
                    {
                        if (sourceChildObject != null)
                        {
                            var destinationChildObject = PopulateResolveInfo(resolveProvider,repoBase,metaModelInfo,sourceChildObject,
                                                            entitySetResolvePropertyMappingInfo.SourceEntitySetClassType,
                                                            entitySetResolvePropertyMappingInfo.ResolveEntitySetClassType);
                            destinationChildList.Add(destinationChildObject);
                        }
                    }
                    entitySetResolvePropertyMappingInfo.ResolveEntitySetPropertyInfo.SetValue(retVal,destinationChildList);
                }
            }


            foreach (var resolvedPropertyWithTypeInfo in sourceTypeTableMetaInfo.SourceTypeResolveTypeMappingInfo.ResolvedPropertyWithTypeInfoList)
            {
                var mainLookupTableClassType = resolvedPropertyWithTypeInfo.MainLookupTable;
                var mainLookupTableMetaInfoWithType = metaModelInfo.TableTypeMetaInfoDic[mainLookupTableClassType];
                var valuesToBeReplaced = resolvedPropertyWithTypeInfo.PropertiesBeingResolvedList
                    .Select(pr => pr.GetValue(source)).ToArray();
                resolveProvider.AddPropertiesThatNeedToBeResolved(new PropertiesToBeSetWithResolvedValues(resolvedPropertyWithTypeInfo.ResolvePropertyInfo,retVal,valuesToBeReplaced,mainLookupTableMetaInfoWithType));
            }

            return retVal;
        }

        internal static List<object> PopulateResolveObjectList(IApplicationRepositoryBase repoBase,ORMModelMetaInfo metaModelInfo,
            List<object> sourceList,Type sourceType,Type resolveType)
        {
            var resolveProvider = new ResolverProvider();
            var retVal = sourceList.Select(s =>
                    PopulateResolveInfo(resolveProvider,repoBase,metaModelInfo,s,sourceType,resolveType))
                .ToList();
            resolveProvider.ResolveAllProviders(repoBase);
            return retVal;
        }

        public static object GetResolvedObject(IApplicationRepositoryBase repoBase, ORMModelMetaInfo metaModelInfo,object source, Type sourceType, Type resolveType)
        {
            var sourceTypeTableMetaInfo = metaModelInfo.TableTypeMetaInfoDic[sourceType];
            if (source == null)
            {
                return null;
            }
            var retVal =  resolveType.GetConstructor(new Type[] { })?.Invoke(null);//TODO - Store the constructor in SourceTypeResolveTypeMappingInfo
            if (retVal == null)
            {
                throw new InvalidOperationException($"Cant create a instance of type {resolveType.FullName} using default constructor");
            }
            foreach (var primitiveSourceResolvePropertyMappingInfo in sourceTypeTableMetaInfo.SourceTypeResolveTypeMappingInfo.PrimitiveSourceResolvePropertyMappingInfoList)
            {
                primitiveSourceResolvePropertyMappingInfo.ResolvePropertyInfo.SetValue(retVal,primitiveSourceResolvePropertyMappingInfo.SourcePropertyInfo.GetValue(source));  
            }

            foreach (var entitySetResolvePropertyMappingInfo in sourceTypeTableMetaInfo.SourceTypeResolveTypeMappingInfo.EntitySetResolvePropertyMappingInfoList)
            {
                if (entitySetResolvePropertyMappingInfo.SourceEntitySetPropertyInfo.GetValue(source) is IEnumerable sourceChildList)
                {
                    IList destinationChildList = typeof(List<>)
                        .MakeGenericType(entitySetResolvePropertyMappingInfo.ResolveEntitySetClassType)
                        ?.GetConstructor(new Type[] { })?.Invoke(null) as IList;
                    if (destinationChildList == null)
                    {
                        throw new InvalidOperationException($"Cant create an instance of List<{entitySetResolvePropertyMappingInfo.ResolveEntitySetClassType.FullName}>");
                    }
                    foreach (var sourceChildObject in sourceChildList)
                    {
                        if (sourceChildObject != null)
                        {
                            var destinationChildObject = GetResolvedObject(repoBase,metaModelInfo, sourceChildObject,
                                                            entitySetResolvePropertyMappingInfo.SourceEntitySetClassType,
                                                            entitySetResolvePropertyMappingInfo.ResolveEntitySetClassType);
                            destinationChildList.Add(destinationChildObject);
                        }
                    }
                    entitySetResolvePropertyMappingInfo.ResolveEntitySetPropertyInfo.SetValue(retVal,destinationChildList);
                }
            }

            
            var resolverProvider = new ResolverProvider();
            foreach (var resolvedPropertyWithTypeInfo in sourceTypeTableMetaInfo.SourceTypeResolveTypeMappingInfo.ResolvedPropertyWithTypeInfoList)
            {
                var mainLookupTableClassType = resolvedPropertyWithTypeInfo.MainLookupTable;
                var mainLookupTableMetaInfoWithType = metaModelInfo.TableTypeMetaInfoDic[mainLookupTableClassType];
                var valuesToBeReplaced = resolvedPropertyWithTypeInfo.PropertiesBeingResolvedList
                    .Select(pr => pr.GetValue(source)).ToArray();
                resolverProvider.AddPropertiesThatNeedToBeResolved(new PropertiesToBeSetWithResolvedValues(resolvedPropertyWithTypeInfo.ResolvePropertyInfo,retVal,valuesToBeReplaced,mainLookupTableMetaInfoWithType));
            }
            resolverProvider.ResolveAllProviders(repoBase);
            
            return retVal;
        }
        
        
    }

    public class PropertiesToBeSetWithResolvedValues
    {
        public readonly PropertyInfo PropertyInfoToBeSetWithResolvedValue;
        public readonly object ObjectThatContainsPropertyToBeResolved;
        public readonly object PKObjectForWhichResolvedValueNeedToBeObtained;
        public readonly TableDataContractMetaInfoWithType MainLookUpTableDataContractMetainfoWithType;       

        public PropertiesToBeSetWithResolvedValues(PropertyInfo propertyInfoToBeSetWithResolvedValue, object objectThatContainsPropertyToBeResolved, object[] valuesToBeReplaced, TableDataContractMetaInfoWithType mainLookUpTableDataContractMetainfoWithType)
        {
            if (valuesToBeReplaced == null || valuesToBeReplaced.Length == 0)
            {
                throw new ArgumentException($"the number of elements in argument is  zero {nameof(valuesToBeReplaced)}");
            }

            if ((propertyInfoToBeSetWithResolvedValue != null && objectThatContainsPropertyToBeResolved == null) ||
                (propertyInfoToBeSetWithResolvedValue == null && objectThatContainsPropertyToBeResolved != null))
            {
                throw new ArgumentException($"Either the PropertyInfo that need to be set with resolved value is null or the object that contains the PropertyInfo that need to be set with resolved value is null");
            }
            this.PropertyInfoToBeSetWithResolvedValue = propertyInfoToBeSetWithResolvedValue ;
            this.ObjectThatContainsPropertyToBeResolved = objectThatContainsPropertyToBeResolved ;
            this.MainLookUpTableDataContractMetainfoWithType =
                mainLookUpTableDataContractMetainfoWithType ??
                throw new ArgumentNullException($"{nameof(mainLookUpTableDataContractMetainfoWithType)}");
            this.PKObjectForWhichResolvedValueNeedToBeObtained = this.MainLookUpTableDataContractMetainfoWithType.PrimaryKeyClassConstructorInfo.Invoke(valuesToBeReplaced);
        }
    }

    public class ResolverProvider 
    {
        private readonly ConcurrentDictionary<Type, IResolver> _resolverDictionaryByPKType = new ConcurrentDictionary<Type, IResolver>();
        public readonly List<PropertiesToBeSetWithResolvedValues> PropertiesThatNeedToBeSetWithResolvedValues = new List<PropertiesToBeSetWithResolvedValues>();

        public void AddPropertiesThatNeedToBeResolved(PropertiesToBeSetWithResolvedValues propertiesToBeSetWithResolved)
        {
            var resolver = GetResolver(propertiesToBeSetWithResolved.MainLookUpTableDataContractMetainfoWithType.TableTypeClassInfo,
                propertiesToBeSetWithResolved.MainLookUpTableDataContractMetainfoWithType.TablePrimaryKeyClassType,
                propertiesToBeSetWithResolved.MainLookUpTableDataContractMetainfoWithType.PrimaryIDLookupClassType);
                
            resolver.Add(propertiesToBeSetWithResolved.PKObjectForWhichResolvedValueNeedToBeObtained);

            if (propertiesToBeSetWithResolved.PropertyInfoToBeSetWithResolvedValue != null)
            {
                PropertiesThatNeedToBeSetWithResolvedValues.Add(propertiesToBeSetWithResolved);
            }
        }
        
        public IResolver GetResolverByPKType(Type pkType)
        {
            return _resolverDictionaryByPKType[pkType];
        }

        public IResolver GetResolver(Type lookupClassType, Type lookupPrimaryClassType, Type lookupResolveClassType)
        {
            if (_resolverDictionaryByPKType.TryGetValue(lookupPrimaryClassType, out IResolver resolver))
            {
                return resolver;
            }
            else
            {
                Type resolverGenericType = typeof(Resolver<,,>).MakeGenericType(new Type[]
                    {lookupClassType, lookupPrimaryClassType, lookupResolveClassType});
                var defaultConstructor =  resolverGenericType.GetConstructor(new Type[]{});
                if (defaultConstructor == null)
                {
                    throw new InvalidOperationException($"Cannot find default constructor on type {resolverGenericType.FullName}");
                }
                if ((defaultConstructor?.Invoke(null) is IResolver createdResolver))
                {
                    if (_resolverDictionaryByPKType.TryAdd(lookupPrimaryClassType, createdResolver))
                    {
                        return createdResolver;
                    }
                    else
                    {
                        return _resolverDictionaryByPKType[lookupPrimaryClassType];
                    }
                }
                else
                {
                    throw new InvalidOperationException($"Cannot create instance of Resolver");
                }
            }
        }

        public void ResolveAllProviders(IApplicationRepositoryBase repoBase)
        {
            foreach (var keyValuePair in _resolverDictionaryByPKType)
            {
                keyValuePair.Value.Resolve(repoBase);
            }
            
            foreach (var propertyInfoToBeSetWithResolvedValues in PropertiesThatNeedToBeSetWithResolvedValues)
            {
                propertyInfoToBeSetWithResolvedValues.PropertyInfoToBeSetWithResolvedValue.SetValue(
                    propertyInfoToBeSetWithResolvedValues.ObjectThatContainsPropertyToBeResolved
                    ,GetResolverByPKType(propertyInfoToBeSetWithResolvedValues.PKObjectForWhichResolvedValueNeedToBeObtained.GetType())
                        .GetResolvedValue(propertyInfoToBeSetWithResolvedValues.PKObjectForWhichResolvedValueNeedToBeObtained));
            }

        }
        
    }
    
    public interface IResolver
    {
        void Add(object pkObject);
        object GetResolvedValue(object pkObject);
        void Resolve(IApplicationRepositoryBase repo);
    }

    public class Resolver<TK,TPK,TLk> : IResolver
        where TPK :class,IPrimaryKeyBase<TK> where TLk :class ,IPrimaryKeyResolveBase<TPK, TK> where TK : class
    {
        //private readonly SortedSet<TPK> _sortedSet = new SortedSet<TPK>();
        private readonly List<TPK> _sortedSet = new List<TPK>();
        private Dictionary<TPK,TLk> _dictionaryOfResolvedValue ;
        public void Add(object pkObject)
        {
            if (pkObject is TPK pkObj)
            {
                if (_sortedSet.Exists(p => p.Equals(pkObj)) == false)
                {
                    _sortedSet.Add(pkObj);
                }
            }
            else
            {
                throw new InvalidOperationException($"Could not case object to type {typeof(TPK).FullName}");
            }
        }

        public Resolver()
        {
            
        }
        public void Resolve(IApplicationRepositoryBase repo)
        {
            _dictionaryOfResolvedValue = repo.GetPKLookupResolvedDictionaryForSingleType<TK, TPK, TLk>(_sortedSet.ToList());
        }

        public TLk GetResolvedValue(TPK pkObject)
        {
            if (_dictionaryOfResolvedValue == null)
            {
                throw new InvalidOperationException($"Please call method Resolve before calling method GetResolvedValue");
            }
            if (pkObject is TPK pkObj)
            {
                return _dictionaryOfResolvedValue[pkObj];
            }
            else
            {
                throw new InvalidOperationException($"Could not case object to type {typeof(TPK).FullName}");
            }
            
        }

        object IResolver.GetResolvedValue(object pkObject)
        {
            if (pkObject is TPK pkObj)
            {
                return GetResolvedValue(pkObj);
            }
            else
            {
                throw new InvalidOperationException($"Could not case object to type {typeof(TPK).FullName}");
            }
        }
    }
    
}
