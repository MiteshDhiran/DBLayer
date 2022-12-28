

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using ConnectAndSell.DataAccessStandard.Server.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace ConnectAndSell.DataAccess.Repository.EFCore1
{
    /// <summary>
    /// Core1ModelBuilder
    /// </summary>
    public class Core1ModelBuilder
    {
        private static readonly ConcurrentDictionary<ORMMetadata, IModel> MetadataEFModelDictionary = new ConcurrentDictionary<ORMMetadata, IModel>();
        /// <summary>
        /// Provides the EF Model based on the given ORMContext.
        /// </summary>
        /// <param name="ormContext"></param>
        /// <returns></returns>
        public static IModel GetEFModel(ORMContext ormContext)
        {
            if (MetadataEFModelDictionary.ContainsKey(ormContext.ORMMetadata) == false)
            {
                var efModel = CreateEFModel(ormContext, new ModelBuilder(SqlServerConventionSetBuilder.Build()));
                MetadataEFModelDictionary.TryAdd(ormContext.ORMMetadata,efModel);
            }
            return MetadataEFModelDictionary[ormContext.ORMMetadata];
        }

        /// <summary>
        /// Used to extract the Classes Bases types
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        private static List<Type> GetBaseClasses(List<Type> types)
        {
            void PopulateBaseClassInternal(Type type, List<Type> existingBaseClasses )
            {
                if(type != null && type.BaseType != null && type.BaseType.IsClass && type.BaseType != typeof(object)  && existingBaseClasses.Contains(type.BaseType) == false)
                {
                    existingBaseClasses.Add(type.BaseType);
                    PopulateBaseClassInternal(type.BaseType,existingBaseClasses);
                }
            }
            List<Type> retVal = new List<Type>();
            types.ForEach(t => PopulateBaseClassInternal(t,retVal));
            return retVal;
        }

        /// <summary>
        /// Provides the list of properties which are not meant to be used entity type that were added by convention.
        /// For example DomainEntityState, this is not an column in Database table.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="tableDataContract"></param>
        /// <returns></returns>
        private static List<IColumnNetInfo> GetAllPropertyToIgnore(Type type, TableDataContractMetaInfoWithType tableDataContract)
        {
            var listOfAllProperties =type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy).Select(p => new ColumnNetInfo(p)).Cast<IColumnNetInfo>().ToList();
            var listOfValidProperties = new List<IColumnNetInfo>();
            var columnPropertyNames = tableDataContract.ColumnPropertiesWithTypeList.Select(c => c.ColumnPropertyInfo.Name).ToList();
            var columnProperties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public ).ToList().Where(p => columnPropertyNames.Contains(p.Name)).Select(p => new ColumnNetInfo(p)).Cast<IColumnNetInfo>().ToList();
            if (columnProperties.Any())
            {
                listOfValidProperties.AddRange(columnProperties);
            }
            var childEntitySetProperties = tableDataContract.ChildPropertyMetaInfoWithTypeList.Select(t => t.ChildEntitySetPropertyInfo).ToList();
            listOfValidProperties.AddRange(childEntitySetProperties);
            // Commented the below code as we dont want to ignore the Parent Property.--Need not ignore Parent Property Info
            if (tableDataContract.ParentPropertyMetaInfoWithType != null &&
                tableDataContract.ParentPropertyMetaInfoWithType.Length != 0)
            {
                listOfValidProperties.AddRange(tableDataContract.ParentPropertyMetaInfoWithType.Select(x => x.EntityRefPropertyInfo));
            }
            var listOfIgnorableProperties = listOfAllProperties.Except(listOfValidProperties,  ColumnNetInfo.ColumnNetInfoNamePropertyTypeEqualityComparer).ToList();
            return listOfIgnorableProperties;
        }

        /// <summary>
        /// Creates EFModel based on the ORM Context Provided
        /// The ORMContext is build by the ORMmetadata provided by the application
        /// </summary>
        /// <param name="ormContext"></param>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static IModel CreateEFModel(ORMContext ormContext, ModelBuilder modelBuilder)
        {
            var typeWithTableContractTupleList = ormContext.ORMModelMetaInfo.TableTypeMetaInfoDic.Select(k => k.Value).ToList();
            var allTypes = typeWithTableContractTupleList.Select(t => t.TableTypeClassInfo).ToList();
            var baseClasses = GetBaseClasses(allTypes);
            baseClasses.ForEach(t => modelBuilder.Ignore(t));

            var entityTypeBuilderList = typeWithTableContractTupleList.Select(t =>
            {
                var entityBuilder = modelBuilder.Entity(t.TableTypeClassInfo);
                var pkPropertyNames = t.ColumnPropertiesWithTypeList.Where(p => p.ColumnPropertyMetaInfo.IsPKColumn)
                    .Select(p => p.ColumnPropertyMetaInfo.ColumnPropertyName).ToArray();
                if (pkPropertyNames.Any())
                {
                    entityBuilder.HasKey(pkPropertyNames.ToArray());
                }
                var allIgnorableProperties = GetAllPropertyToIgnore(t.TableTypeClassInfo, t);
                allIgnorableProperties.ForEach(prop => entityBuilder.Ignore(prop.Name));

                if (t.IgnoreColumnsPropertyInfoList != null && t.IgnoreColumnsPropertyInfoList.Any())
                {
                    foreach (var propertyInfo in t.IgnoreColumnsPropertyInfoList)
                    {
                        entityBuilder.Ignore(propertyInfo.Name);
                    }
                }
                if (t.ParentPropertyMetaInfoWithType != null && t.ParentPropertyMetaInfoWithType.Length > 0)
                {
                        foreach (var parentPropertyMetaInfoWithType in t.ParentPropertyMetaInfoWithType)
                        {
                                try
                                {
                                    entityBuilder.HasOne(parentPropertyMetaInfoWithType.ParentClassType,
                                                                parentPropertyMetaInfoWithType.EntityRefPropertyInfo.Name)
                                                            .WithMany(parentPropertyMetaInfoWithType.EntitySetPropertyInfo.Name)
                                                            .HasForeignKey(parentPropertyMetaInfoWithType.ChildParticipatingPropertyInfoList
                                                                .Select(cp => cp.Name).ToArray())
                                                            .HasPrincipalKey(parentPropertyMetaInfoWithType.ParentParticipatingPropertyInfoList
                                                                .Select(cp => cp.Name).ToArray())
                                                            .OnDelete(DeleteBehavior.ClientSetNull)
                                                            ;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                        }
                }
                var propertyBuilderList = t.ColumnPropertiesWithTypeList.Select(c =>
                {
                    var propertyBuilder = entityBuilder.Property(c.ColumnPropertyMetaInfo.ColumnPropertyName);
                    if (string.IsNullOrEmpty(c.ColumnPropertyMetaInfo.BackingFieldName) == false )
                    {
                        propertyBuilder.HasField(c.ColumnPropertyMetaInfo.BackingFieldName);
                    }
                    propertyBuilder.HasColumnName(c.ColumnPropertyMetaInfo.ColumnName);

                    
                    if (c.ColumnPropertyMetaInfo.IsRequired && c.ColumnPropertyInfo.PropertyType != typeof(string) && (!(c.ColumnPropertyInfo.PropertyType.IsGenericType && c.ColumnPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))))
                    {
                        propertyBuilder.IsRequired();
                    }
                    else if (c.ColumnPropertyInfo.PropertyType.IsGenericType )
                    {
                        if (c.ColumnPropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyBuilder.IsRequired(false);
                        }
                    }

                    if (c.ColumnPropertyMetaInfo.IsGeneratedOnAdd && c.ColumnPropertyMetaInfo.IsGeneratedOnUpdate == false)
                    {
                        propertyBuilder.ValueGeneratedOnAdd();
                    }
                    else if (c.ColumnPropertyMetaInfo.IsGeneratedOnAdd && c.ColumnPropertyMetaInfo.IsGeneratedOnUpdate == false)
                    {
                        propertyBuilder.ValueGeneratedOnAdd();
                    }
                    else if (c.ColumnPropertyMetaInfo.IsGeneratedOnUpdate && c.ColumnPropertyMetaInfo.IsGeneratedOnAdd == false)
                    {
                        propertyBuilder.ValueGeneratedOnUpdate();
                    }
                    else if (c.ColumnPropertyMetaInfo.IsGeneratedOnUpdate == true &&
                             c.ColumnPropertyMetaInfo.IsGeneratedOnAdd == true)
                    {
                        propertyBuilder.ValueGeneratedOnAddOrUpdate();
                    }

                    if (c.ColumnPropertyMetaInfo.IsVersion && (c.ColumnPropertyMetaInfo.IsPKColumn == false))
                    {
                        propertyBuilder.IsRowVersion();
                        propertyBuilder.ValueGeneratedOnAddOrUpdate();
                        propertyBuilder.IsConcurrencyToken();
                    }
                    
                    
                    if (c.ColumnPropertyMetaInfo.StringMaxLength != null && c.ColumnPropertyMetaInfo.StringMaxLength.Value > 0)
                    {
                        propertyBuilder.HasMaxLength(c.ColumnPropertyMetaInfo.StringMaxLength.Value);
                    }
                    return propertyBuilder;
                }).ToList();
                return entityBuilder;
            }).ToList();
            var retVal = modelBuilder.Model;
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