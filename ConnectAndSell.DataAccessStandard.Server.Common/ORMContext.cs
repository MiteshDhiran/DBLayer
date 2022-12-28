

using System;
using System.Reflection;
using ConnectAndSell.DataAccessStandard.Common.DataContract;

namespace ConnectAndSell.DataAccessStandard.Server.Common
{
    /// <summary>
    /// ORMContext class
    /// </summary>
    public class ORMContext
    {
        private ORMContext(ORMMetadata ormMetadata, Assembly dataContractAssembly, bool preCompile)
        {
            ORMMetadata = ormMetadata;
            DataContractAssembly = dataContractAssembly;
            PreCompile = preCompile;
            ORMModelMetaInfo = new ORMModelMetaInfo(ORMMetadata,DataContractAssembly);
            if (PreCompile)
            {
                GeneratePreCompiledQueries();
            }
        }

        /// <summary>
        /// The get orm context
        /// </summary>
        public static readonly Func<Tuple<string, Assembly>, ORMContext> GetORMContext = DBUtility.Memonize<Tuple<string, Assembly>, ORMContext>(
            (_, tuple) =>
            {
                var ormMetadata = DataContractSerializerHelper.DataContractDeserializer<ORMMetadata>(tuple.Item1);
                return new ORMContext(ormMetadata,tuple.Item2,true);
            });

        /// <summary>
        /// Gets the orm metadata.
        /// </summary>
        /// <value>
        /// The orm metadata.
        /// </value>
        public ORMMetadata ORMMetadata { get; private set; }

        /// <summary>
        /// Gets the data contract assembly.
        /// </summary>
        /// <value>
        /// The data contract assembly.
        /// </value>
        public Assembly DataContractAssembly { get; private set; }

        /// <summary>
        /// Gets a value indicating whether [pre compile].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pre compile]; otherwise, <c>false</c>.
        /// </value>
        public bool PreCompile { get; }

        /// <summary>
        /// Gets the orm model meta information.
        /// </summary>
        /// <value>
        /// The orm model meta information.
        /// </value>
        public ORMModelMetaInfo ORMModelMetaInfo { get; private set; }

        private void GeneratePreCompiledQueries()
        {
            foreach (var ormMetadataRootTable in ORMMetadata.RootTables)
            {
                if (ORMModelMetaInfo.TableTypeMetaInfoDicByClassFullName.ContainsKey(ormMetadataRootTable.Value
                    .TableClassFullName) == false)
                {
                    Console.WriteLine($"Class : {ormMetadataRootTable.Value.TableClassFullName} is missing from Dic ORMModelMetaInfo.TableTypeMetaInfoDicByClassFullName");
                }
                else
                {
                    ORMModelMetaInfo.GetTSQLByXMLForDataContract(ORMModelMetaInfo
                        .TableTypeMetaInfoDicByClassFullName[ormMetadataRootTable.Value.TableClassFullName]
                        .TableTypeClassInfo);
                    ORMModelMetaInfo.GetTSQLByJSONForDataContract(ORMModelMetaInfo
                        .TableTypeMetaInfoDicByClassFullName[ormMetadataRootTable.Value.TableClassFullName]
                        .TableTypeClassInfo);    
                }
                
            }

            foreach (var ormMetadataRootTable in ORMMetadata.RootTables)
            {
                if (ORMModelMetaInfo.TableTypeMetaInfoDicByClassFullName.ContainsKey(ormMetadataRootTable.Value
                    .TableClassFullName) == false)
                {
                    Console.WriteLine($"Class : {ormMetadataRootTable.Value.TableClassFullName} is missing from Dic ORMModelMetaInfo.TableTypeMetaInfoDicByClassFullName");
                }
                else
                {
                    var tableInfo = ORMModelMetaInfo
                        .TableTypeMetaInfoDicByClassFullName[ormMetadataRootTable.Value.TableClassFullName];
                    var allColumns = ORMModelMetaInfo.GetAllTableColumnProperties(tableInfo.TableTypeClassInfo);
                    if (allColumns.Exists(t => t.IsPrimaryKey && t.IsDbGeneratedOnAdd))
                    {
                        ORMModelMetaInfo.GetBulkInsertSQLForType(ORMModelMetaInfo
                            .TableTypeMetaInfoDicByClassFullName[ormMetadataRootTable.Value.TableClassFullName]
                            .TableTypeClassInfo);
                    }
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