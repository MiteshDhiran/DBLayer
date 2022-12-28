

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace ConnectAndSell.DataAccessStandard.Common.DataContract
{
    /// <summary>
    ///  ORMMetadata class
    /// </summary>
    [DataContract]
    public class ORMMetadata
    {
        /// <summary>
        /// The root tables
        /// </summary>
        [DataMember] public readonly List<TreeNode<TableDataContractMetaInfo>> RootTables;

        /// <summary>
        /// The associate root entities
        /// </summary>
        [DataMember] public readonly List<AssociatedParentChildTableInfo> AssociateRootEntities;

        /// <summary>
        /// The enum information list
        /// </summary>
        [DataMember] public readonly List<EnumInfo> EnumInfoList;

        /// <summary>
        /// Gets the table name dictionary.
        /// </summary>
        /// <value>
        /// The table name dictionary.
        /// </value>
        public ReadOnlyDictionary<string, TableDataContractMetaInfo> TableNameDictionary { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ORMMetadata"/> class.
        /// </summary>
        /// <param name="rootTables">The root tables.</param>
        /// <param name="associateRootEntities">The associate root entities.</param>
        /// <param name="enumInfoList">The enum information list.</param>
        public ORMMetadata(List<TreeNode<TableDataContractMetaInfo>> rootTables, List<AssociatedParentChildTableInfo> associateRootEntities, List<EnumInfo> enumInfoList)
        {
            RootTables = rootTables ?? new List<TreeNode<TableDataContractMetaInfo>>();
            AssociateRootEntities = associateRootEntities ?? new List<AssociatedParentChildTableInfo>();
            //Changed the below code to handle recursive table
            /*
            TableNameDictionary = new ReadOnlyDictionary<string, TableDataContractMetaInfo>(RootTables.SelectMany(c => c.Flatten()).Select(c => c).ToList()
                .ToDictionary(c => c.TableName));
                */
            TableNameDictionary = new ReadOnlyDictionary<string, TableDataContractMetaInfo>(
                RootTables.SelectMany(c => c.Flatten()).Select(c => c).ToList()
                    .ToLookup(c => c.TableName).ToDictionary(k => k.Key, kv => kv.First()));
            EnumInfoList = enumInfoList ?? new List<EnumInfo>();
        }
        
        [OnDeserialized()]
        void OnDeserializedMethod(StreamingContext context)
        {
            //Changed the below code to handle recursive table
            /*
            TableNameDictionary = new ReadOnlyDictionary<string, TableDataContractMetaInfo>(RootTables.SelectMany(c => c.Flatten()).Select(c => c).ToList()
                .ToDictionary(c => c.TableName)); 
                */
            TableNameDictionary = new ReadOnlyDictionary<string, TableDataContractMetaInfo>(RootTables
                .SelectMany(c => c.Flatten()).Select(c => c).ToList()
                .ToLookup(c => c.TableName).ToDictionary(k => k.Key, kv => kv.First()));
        }

        private string GetLookupClassTypeForTable(string tableName)
        {
            if(TableNameDictionary.ContainsKey(tableName) == false) throw new InvalidOperationException($"Table :{tableName} does not exists");
            return TableNameDictionary[tableName].TablePrimaryKeyLookupResolveClassName;
        }

        /// <summary>
        /// Ges the name of the resolve class name or class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException">Table :{tableName} does not exists</exception>
        public string GeResolveClassNameOrClassName(string tableName)
        {
            if (string.IsNullOrEmpty(tableName))
            {
                throw new ArgumentNullException($"{nameof(tableName)}");
            }
            if(TableNameDictionary.ContainsKey(tableName) == false) throw new InvalidOperationException($"Table :{tableName} does not exists");
            return TableNameDictionary[tableName].ResolveClassNameOrClassName;
            
        }

        /// <summary>
        /// Gets the lookup type name based on lookup information.
        /// </summary>
        /// <param name="resolveLookupBase">The resolve lookup base.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Lookup type cannot be determined for LookupType:{resolveLookupBase.ResolveLookupType} Resolved PropertyName:{resolveLookupBase.ResolvedPropertyName}</exception>
        public string GetLookupTypeNameBasedOnLookupInfo(ResolveLookupBase resolveLookupBase)
        {
            ResolveTableLookUpInfo tableResolve = resolveLookupBase as ResolveTableLookUpInfo;
            ResolveEnumLookUpInfo enumResolve = resolveLookupBase as ResolveEnumLookUpInfo;
            if (tableResolve != null)
            {
                return GetLookupClassTypeForTable(tableResolve.TableName);
            }
            else if (enumResolve != null)
            {
                return enumResolve.EnumName;
            }
            else
            {
                throw new InvalidOperationException($"Lookup type cannot be determined for LookupType:{resolveLookupBase.ResolveLookupType} Resolved PropertyName:{resolveLookupBase.ResolvedPropertyName}");
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