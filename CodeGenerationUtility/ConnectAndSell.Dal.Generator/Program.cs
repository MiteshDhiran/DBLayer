using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommandLine;
using ConnectAndSell.Dal.Generator.Common;
using ConnectAndSell.Dal.Generator.Templates;
using ConnectAndSell.DataAccessStandard.Common.DataContract;
using Microsoft.Data.SqlClient;

namespace ConnectAndSell.Dal.Generator
{
    class Program
    {
        
        static void Main(string[] args)
        {
                
            CommandLine.Parser.Default.ParseArguments<CommandOptions>(args).WithParsed<CommandOptions>(o =>
            {
                
                Console.WriteLine($"Running Program Using following Option projectName:{o.ProjectName} projectNameSpace:{o.ProjectNameSpace} outputFolerPath: {o.OutputFolderPath} configFolderPath: {o.ConfigFolderPath} connectionString: {o.ConnectionString} ");
                        
                        var localOutputFolderPath = Directory.Exists(o.OutputFolderPath)
                            ? o.OutputFolderPath
                            : Directory.CreateDirectory(o.OutputFolderPath).FullName;

                        var localConfigFolderPath = Directory.Exists(o.ConfigFolderPath)
                            ? o.ConfigFolderPath
                            : throw new ArgumentException($"Config Directory {o.ConfigFolderPath} does not exists");
                        
                        //create the directory for datacontext, datacontract and SPWrapper if it does not exists
                        var dataContractPath= Path.Combine(localOutputFolderPath, "DataContract");
                        var dataContextPath =Path.Combine(localOutputFolderPath, "DataContext");
                        var spWrapperPath = Path.Combine(localOutputFolderPath, "BusinessLayer");
                        var metadataFolderPath = Path.Combine(localOutputFolderPath, "Metadata");
                        var sqlFolderPath = Path.Combine(localOutputFolderPath, "SQL");
                        var enumFolderPath = Path.Combine(dataContractPath, "Enum");
                        
                        //Create Sub directories of it does not exists
                        if(Directory.Exists(dataContractPath) == false)
                        {
                            Directory.CreateDirectory(dataContractPath);
                        }
                        if(Directory.Exists(dataContextPath) == false)
                        {
                            Directory.CreateDirectory(dataContextPath);
                        }
                        if(Directory.Exists(spWrapperPath) == false)
                        {
                            Directory.CreateDirectory(spWrapperPath);
                        }

                        if (Directory.Exists(metadataFolderPath) == false)
                        {
                            Directory.CreateDirectory(metadataFolderPath);
                        }

                        if (Directory.Exists(sqlFolderPath) == false)
                        {
                            Directory.CreateDirectory(sqlFolderPath);
                        }
                        
                        if (Directory.Exists(enumFolderPath) == false)
                        {
                            Directory.CreateDirectory(enumFolderPath);
                        }
                        
                        var tableConfigFile = Path.Combine(localConfigFolderPath, "TableConfig.xml");
                        var spConfigFile = Path.Combine(localConfigFolderPath, "SPConfig.xml");
                        if (File.Exists(tableConfigFile) == false)
                            throw new ArgumentException($"Table Config file does not exists at location {tableConfigFile}");
                        if(File.Exists(spConfigFile) == false)
                            throw new ArgumentException($"SP Config file does not exists at location {spConfigFile}");

                        var dataContextName = $"{o.ProjectName}DbContext";
                        var dataContextClassFilePath = Path.Combine(dataContextPath, $"{dataContextName}.cs");
                        
                        var dataContextGeneratorParams = new DataContextGeneratorParams(dataContextName,
                            $"{o.ProjectNameSpace}.Server", $"{o.ProjectNameSpace}.DataContract", "CoreEntityBase"
                            , dataContractPath, dataContextPath ,
                            tableConfigFile, o.ConnectionString,
                            spConfigFile,spWrapperPath, $"{o.ProjectName}DALHelper"
                            ,"DataEntityState"
                            ,"MSReplrowguid,MSRowVersion"
                        );
                        
                        //check whether database connection can be made
                        if(string.IsNullOrEmpty(o.ConnectionString)) throw new ArgumentNullException($"{nameof(o.ConnectionString)} not provided");
                        var connection = new SqlConnection(o.ConnectionString);
                        try
                        {
                            connection.Open();
                            connection.Close();
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine($"Not able to establish connection string with connection string {o.ConnectionString}. Following exception occured {ex.Message}");
                            throw;
                        }
                            
                        
                        Console.WriteLine("Starting generation");
                        
                        var ormMetadata = ORMMetadataGenerator.GetORMMetadata(dataContextGeneratorParams);
                        var allTableContractsParticipatinginCompositeRelationShip = ormMetadata.RootTables.SelectMany(c => c.Flatten()).ToList();
                        
                        var parentChildInputList = ParentChildConfigReaderHelper.GetParentChildTableInfos(dataContextGeneratorParams.ConfigFilePath,dataContextGeneratorParams.ProjectDataContractNameSpace);
                        var tableDataContractMetaInfoList =  SQLMetadataProcessor.GetTableDataContractMetaInfo(dataContextGeneratorParams, parentChildInputList, dataContextGeneratorParams.ProjectDataContractNameSpace);
                        var allTableConfigText = GenerateDataContext(dataContextGeneratorParams,tableDataContractMetaInfoList);
                        
                        File.WriteAllText(dataContextClassFilePath,allTableConfigText);
                        
                        //Bulk Insert User Defined Table Type
                        foreach (var tableDataContractMetaInfo in tableDataContractMetaInfoList)
                        {
                            var appTableGeneratorInfo = BulkInsertAppTableGeneratorUtility.GetBulkInsertAppTableGeneratorInfo(
                                tableDataContractMetaInfo);
                            var tableTypeTransformText = new BulkInsertAppTableTemplate(appTableGeneratorInfo.TableUDTName,
                                appTableGeneratorInfo.ColumnWithDataType).TransformText();
                             var tableUDTTypePath = Path.Combine(sqlFolderPath,
                                $"{tableDataContractMetaInfo.TableClassName}.sql");
                            File.WriteAllText(tableUDTTypePath,tableTypeTransformText);
                        }

                        var distinctTableDomainNames = tableDataContractMetaInfoList.Where(t => string.IsNullOrEmpty(t.DomainName) == false)
                            .Select(t => t.DomainName).Distinct().ToList();
                        
                        distinctTableDomainNames.ForEach(domainName =>
                        {
                            var domainPath = Path.Combine(dataContractPath, domainName);
                            if (Directory.Exists(domainPath) == false)
                            {
                                Directory.CreateDirectory(domainPath);
                            }
                        });
                        
                        foreach (var tableDataContractMetaInfo in tableDataContractMetaInfoList)
                        {
                            var tableDataContractDirectoryPath = string.IsNullOrEmpty(tableDataContractMetaInfo.DomainName) ==false ?   
                                Path.Combine(dataContractPath, tableDataContractMetaInfo.DomainName) : dataContractPath;
                            
                            var tableContractClassText = GetTableDataContractClassText(dataContextGeneratorParams, tableDataContractMetaInfo);
                            var dataContractFilePath = Path.Combine(tableDataContractDirectoryPath,
                                $"{tableDataContractMetaInfo.TableClassName}.cs");
                            File.WriteAllText(dataContractFilePath,tableContractClassText);
                            if (tableDataContractMetaInfo.IsCompositeRootTable)
                            {
                                var tablePrimaryContractClassText =
                                    new DataContractClassWithPrimaryKeyPropertyOnly(dataContextGeneratorParams,
                                        tableDataContractMetaInfo).TransformText();
                                var primaryDataContractFilePath = Path.Combine(tableDataContractDirectoryPath,
                                    $"{tableDataContractMetaInfo.TableClassName}PrimaryRecordInfo.cs");
                                File.WriteAllText(primaryDataContractFilePath, tablePrimaryContractClassText);
                            }
                        }

                        if (ormMetadata.EnumInfoList != null)
                        {
                            foreach (var enumMetaDataInfo in ormMetadata.EnumInfoList)
                            {
                                var enumFullText = GetEnumText(dataContextGeneratorParams, enumMetaDataInfo);
                                var enumPath = Path.Combine(enumFolderPath, $"{enumMetaDataInfo.Name}.cs");
                                File.WriteAllText(enumPath, enumFullText);
                            }
                        }

                        //Generate datacontract class for WithMeaningfullValue
                        //var tableWhichContainsResolvedProperties = tableDataContractMetaInfoList
                        //    .Where(c => c.ResolveLookUps?.ResolveLookups?.Any() == true).ToList();
                        foreach (var resolvedEntity in tableDataContractMetaInfoList)
                        {
                            var tableDataContractDirectoryPath = string.IsNullOrEmpty(resolvedEntity.DomainName) == false ?   
                                Path.Combine(dataContractPath, resolvedEntity.DomainName) : dataContractPath;
                            
                            var resolvedDataContractFilePath = Path.Combine(tableDataContractDirectoryPath,
                                $"{resolvedEntity.ClassNameWithResolvedProperties}.cs");
                            var resolvedClassText = GetTableDataContractWithResolvedLookupPropertiesClassText(ormMetadata,
                                dataContextGeneratorParams, resolvedEntity);
                            File.WriteAllText(resolvedDataContractFilePath, resolvedClassText);
                            
                        }

                        
                        CompiledTypeFileWritter.GenerateCompileTypeFiles(dataContextGeneratorParams
                                                ,tableDataContractMetaInfoList
                                                ,localOutputFolderPath
                                );
                        

                //GetTableDataContractWithResolvedLookupPropertiesClassText

                var tableDataContractMetaInfoListFilteredForLookupPrimaryRecordInfo = tableDataContractMetaInfoList
                            .Where(t => t.PrimaryIDLookupValueRecordInfo != null).ToList();
                        
                        foreach (var tableInfoForPrimaryLookup in tableDataContractMetaInfoListFilteredForLookupPrimaryRecordInfo)
                        {
                            var tableDataContractDirectoryPath = string.IsNullOrEmpty(tableInfoForPrimaryLookup.DomainName) == false ?   
                                Path.Combine(dataContractPath, tableInfoForPrimaryLookup.DomainName) : dataContractPath;
                            
                            var lookResolveTemplate = new TableLookupResolveDataContract(tableInfoForPrimaryLookup,
                                dataContextGeneratorParams);
                            var lookResolveTemplateText = lookResolveTemplate.TransformText();
                            var primaryLookupResolveDataContractFilePath = Path.Combine(tableDataContractDirectoryPath,
                                $"{tableInfoForPrimaryLookup.TableClassName}PrimaryRecordLookupResolveInfo.cs");
                            File.WriteAllText(primaryLookupResolveDataContractFilePath,lookResolveTemplateText);
                        }

                        var associatedEntitiesDomainNames = ormMetadata.AssociateRootEntities.Where(c => string.IsNullOrEmpty(c.DomainName) == false)
                            .Select(c => c.DomainName).Distinct().ToList();
                        associatedEntitiesDomainNames.ForEach(domainName =>
                        {
                            var domainPath = Path.Combine(dataContractPath, domainName);
                            if (Directory.Exists(domainPath) == false)
                            {
                                Directory.CreateDirectory(domainPath);
                            }
                        });
                        foreach (var associateRoot in ormMetadata.AssociateRootEntities)
                        {
                            var associateChildrenPropertiesText = string.Join("\r\n",associateRoot.AssociateChildrenInfo.Select(c => $"[DataMember]\r\n public List<{c.AssociateTableName}> {c.AssociatePropertyName} " + "{get;set;}").ToList()) ;
                            var associateChildrenPropertiesInitializationText = "";
                            var associateRootTableDataContractMetaInfo =  tableDataContractMetaInfoList.First(
                                c => c.TableName == associateRoot.AssociateRootTableName);
                            var rootColumnPropertiesText = string.Join("\r\n",associateRootTableDataContractMetaInfo.ColumnPropertiesList.Select(c => $"[DataMember]\r\n public {c.NetDataTypeForDataContract} {c.ColumnPropertyName} " + "{get;set;}").ToList()) ;
                            var associateRootEntityTemplate = new AssociateRootEntityTemplate(dataContextGeneratorParams,associateRoot,associateChildrenPropertiesText,associateChildrenPropertiesInitializationText,rootColumnPropertiesText);
                            var associateClassDefinitionText = associateRootEntityTemplate.TransformText();
                            var associatedRootEntitiesFolderPath =
                                string.IsNullOrEmpty(associateRoot.DomainName) == false
                                    ? Path.Combine(dataContractPath, associateRoot.DomainName)
                                    : dataContractPath;
                            var associateDataContractFilePath = Path.Combine(associatedRootEntitiesFolderPath,
                                $"{associateRoot.AssociateRootClassName}.cs");
                            File.WriteAllText(associateDataContractFilePath,associateClassDefinitionText);
                        }


                        

                        GenerateWrapperForAllSP(dataContextGeneratorParams);
                        
                        var ormMetadataSerializedText = DataContractSerializerHelper.DataContractSerialize(ormMetadata);
                        var metaDataFilePath = Path.Combine(metadataFolderPath, "ORMMetadata.xml"); 
                        var metaDataDataContractFilePath = Path.Combine(dataContractPath, "ORMMetadata.xml"); 
                        File.WriteAllText(metaDataFilePath,ormMetadataSerializedText);
                        File.WriteAllText(metaDataDataContractFilePath,ormMetadataSerializedText);
                        Console.WriteLine("Done");                        
                        Console.ReadLine();
                
            });
                
        }


        static string GetEnumText(DataContextGeneratorParams dataContextGeneratorParams,EnumInfo enumInfo)
        {
            StringBuilder sbEnum = new StringBuilder();
            var enumText1 = string.Join("\r\n", enumInfo.EnumMemberList.Select(c => (!string.IsNullOrEmpty(c.Value)) ?
                ($"[EnumMember(Value = \"{c.Value}\")]\r\n{c.Name} = {c.Value}" + ",") : ($"[EnumMember()]\r\n{c.Name}" + ",")).ToList());
            var enumText2 = enumText1.ToString().TrimEnd(',');
            sbEnum.Append(enumText2);
            
            var enumMemberList = sbEnum.ToString();
            var enumTemplate = new EnumTemplate(dataContextGeneratorParams,enumInfo.Name, enumMemberList,enumInfo.EnumType);

            var enumTemplateText = enumTemplate.TransformText();
            return enumTemplateText;
        }
        

        public static void GenerateWrapperForAllSP(DataContextGeneratorParams contextGeneratorParam )
        {
            if (string.IsNullOrEmpty(contextGeneratorParam.SPExecutorWrapperFolderPath) == false &&
                string.IsNullOrEmpty(contextGeneratorParam.SPConfigFilePath) == false)
            {
                var spMetadataForCodeGeneration = SPConfigReaderHelper
                    .GetSPConfigData(contextGeneratorParam.SPConfigFilePath).Select(x =>
                        SQLMetadataProcessor.GetSPInputParameterInfo(
                            contextGeneratorParam.ConnectionString, x.SPName
                            , x.ResultSetInfoList.Select(rconfig =>
                                new SPResultSetMetaInfo(rconfig.ClassName, rconfig.Namespace)).ToList()
                            , contextGeneratorParam.DALHelperName)
                    ).ToList();

                var body = spMetadataForCodeGeneration.Aggregate(new StringBuilder(),
                    (sb, spInfo) => sb.AppendLine(SPGeneratorHelper.GenereateCode(spInfo).ToString())).ToString();

                var spWrapperClassTemplate = new SPWrapperClassTemplate()
                {
                    ClassBody = body, DALHelperName = contextGeneratorParam.DALHelperName, UsingBlock = "",
                    WrapperClassSuffix = "SPWrapper"
                };
                var res = spWrapperClassTemplate.TransformText();
                File.WriteAllText($"{contextGeneratorParam .SPExecutorWrapperFolderPath}\\{contextGeneratorParam.DALHelperName + "SP"}.cs",res);
            }
        }
        

        static string GetTableDataContractClassText(DataContextGeneratorParams dataContextGeneratorParams,TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            var columnPropertiesText = string.Join("\r\n",tableDataContractMetaInfo.ColumnPropertiesList.Select(c => $"[DataMember]\r\n public {c.NetDataTypeForDataContract} {c.ColumnPropertyName} " + "{get;set;}").ToList()) ;
            var entitySetPropertiesText = string.Join("\r\n",tableDataContractMetaInfo.ChildEntitySetPropertyInfoList.Select(c => $"[DataMember]\r\n public {c.ChildEntitySetPropertyType} {c.ChildEntitySetPropertyName} " + "{get;set;}"));
            var entitySetPropertyInitializationText =
                string.Join("\r\n",tableDataContractMetaInfo.ChildEntitySetPropertyInfoList.Select(c => $"{c.ChildEntitySetPropertyName} = new {c.ChildEntitySetPropertyType}();"));
            var entityRefPropertyText = tableDataContractMetaInfo.ParentPropertyMetaInfo != null 
                ? $"[IgnoreDataMember]\r\n[JsonIgnore]\r\n public {tableDataContractMetaInfo.ParentPropertyMetaInfo.ParentClassName} {tableDataContractMetaInfo.ParentPropertyMetaInfo.ParentClassName}" + "{get;set;}"
                : "";
            var ondeserializeText = string.Join("\r\n",
                tableDataContractMetaInfo.ChildEntitySetPropertyInfoList.Select(c =>
                    $"{c.ChildEntitySetPropertyName}?.ForEach(c => c.{c.EntityRefPropertyName} = this);")
            );
            var completeDeserializedMethodText =  tableDataContractMetaInfo.ChildEntitySetPropertyInfoList.Any()
                ? $@"[OnDeserialized()]
void OnDeserializedMethod(StreamingContext context)
{{
{ondeserializeText}
}}"
: string.Empty;

            var baseClassText = tableDataContractMetaInfo.IsCompositeRootTable
                ? $"{dataContextGeneratorParams.BaseDataContractClassName},ICompositeRoot"
                : $"{dataContextGeneratorParams.BaseDataContractClassName}";
            var dataContractClassTemplate = new Templates.DataContractClassTemplate(
                tableDataContractMetaInfo.TableClassName
                ,tableDataContractMetaInfo
                ,dataContextGeneratorParams
                , columnPropertiesText
                , entitySetPropertiesText
                , entitySetPropertyInitializationText
                , entityRefPropertyText
                , completeDeserializedMethodText
                , baseClassText
            );
            var retVal = dataContractClassTemplate.TransformText();
            return retVal;
        }
        
        static string GetTableDataContractWithResolvedLookupPropertiesClassText(ORMMetadata ormMetadata,DataContextGeneratorParams dataContextGeneratorParams,TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            var resolvedTableDataContractMetaInfo = ResolvedTableDataContractMetaInfo.GetResolvedTableDataContractMetaInfo(ormMetadata,
                tableDataContractMetaInfo);
            var columnPropertiesText = string.Join("\r\n",resolvedTableDataContractMetaInfo.PrimitiveColumns.Select(c => $"[DataMember]\r\n public {c.NetDataTypeForDataContract} {c.ColumnPropertyName} " + "{get;set;}").ToList()) ;
            var resolvedProperties = resolvedTableDataContractMetaInfo.ResolvedTableLookupPropertiesInfoList?.Select(c =>
                new {ResolvedPropertyName = c.ResolveTableLookUpInfoBeingResolved.ResolvedPropertyName, PropertyTypeName = c.LookupResolveClassName}).ToList();
            var lookupResolvePropertiesText =
                resolvedProperties?.Any() == true
                    ? string.Join("\r\n",
                        resolvedProperties.Select(c =>
                                $"[DataMember]\r\n public {c.PropertyTypeName} {c.ResolvedPropertyName} " +
                                "{get;set;}")
                            .ToList())
                    : string.Empty;

            var primitiveAndResolvedPropertiesText = string.IsNullOrEmpty(lookupResolvePropertiesText) == false
                ? columnPropertiesText + "\r\n" + lookupResolvePropertiesText
                : columnPropertiesText;
            
            var entitySetPropertiesText = string.Join("\r\n",resolvedTableDataContractMetaInfo.ResolvedChildEntitySetPropertyInfoList.Select(c => $"[DataMember]\r\n public {c.ResolvedChildEntitySetPropertyTypeName} {c.ResolvedChildEntitySetPropertyName} " + "{get;set;}"));
            var entitySetPropertyInitializationText =
                string.Join("\r\n",resolvedTableDataContractMetaInfo.ResolvedChildEntitySetPropertyInfoList.Select(c => $"{c.ResolvedChildEntitySetPropertyName} = new {c.ResolvedChildEntitySetPropertyTypeName}();"));
            var entityRefPropertyText = resolvedTableDataContractMetaInfo.ResolvedEntityRefPropertyInfo != null 
                ? $"[IgnoreDataMember]\r\n[JsonIgnore]\r\n public {resolvedTableDataContractMetaInfo.ResolvedEntityRefPropertyInfo.ResolvedEntityRefPropertyClassName} {resolvedTableDataContractMetaInfo.ResolvedEntityRefPropertyInfo.ResolvedEntityRefPropertyName}" + "{get;set;}"
                : "";
            var ondeserializeText = string.Join("\r\n",
                resolvedTableDataContractMetaInfo.ResolvedChildEntitySetPropertyInfoList.Select(c =>
                    $"{c.ResolvedChildEntitySetPropertyName}?.ForEach(c => c.{c.ResolvedEntityRefPropertyName} = this);")
            );
            var completeDeserializedMethodText =  resolvedTableDataContractMetaInfo.ResolvedChildEntitySetPropertyInfoList.Any()
                ? $@"[OnDeserialized()]
void OnDeserializedMethod(StreamingContext context)
{{
{ondeserializeText}
}}"
: string.Empty;

            //var baseClassText = tableDataContractMetaInfo.IsCompositeRootTable
            //    ? $"{dataContextGeneratorParams.BaseDataContractClassName}"
            //    : $"{dataContextGeneratorParams.BaseDataContractClassName}";
            var baseClassText = $"{dataContextGeneratorParams.BaseDataContractClassName}";
            var dataContractClassTemplate = new DataContractClassTemplate(
                tableDataContractMetaInfo.ClassNameWithResolvedProperties
                ,tableDataContractMetaInfo
                ,dataContextGeneratorParams
                , primitiveAndResolvedPropertiesText
                , entitySetPropertiesText
                , entitySetPropertyInitializationText
                , entityRefPropertyText
                , completeDeserializedMethodText
                , baseClassText
            );
            var retVal = dataContractClassTemplate.TransformText();
            return retVal;
        }

        
        static string GenerateDataContext(DataContextGeneratorParams dataContextGeneratorParams, List<TableDataContractMetaInfo> tableDataContractMetaInfoList)
        {
            var allTableTextList = tableDataContractMetaInfoList.Select(t => GetConfigureTableInDataContext(t, new List<string>(){"DataEntityState"} )).ToList();
            var allTableText = string.Join("\r\n",allTableTextList) ;
            var dataContextClassTemplate = new DataContextClassTemplate()
            {
                DataContextClassName = dataContextGeneratorParams.DataContextClassName
                , ModelBuilderConfigurationBody = allTableText
                , ProjectDataContextNameSpace = dataContextGeneratorParams.ProjectDataContextNameSpace
                , ProjectDataContractUsingStatement = "using " + dataContextGeneratorParams.ProjectDataContractNameSpace
            };
            var dbContextClassContent = dataContextClassTemplate.TransformText();
            return dbContextClassContent;
        }

        static string GetConfigureTableInDataContext(TableDataContractMetaInfo t, List<string> ignoreProperties)
        {
            var columnPropertiesTemplateConfiguration = t.ColumnPropertiesList.Select(c => new ContextTablePropertyTemplate()
            {
                ColumnPropertyMetaInfo = c
            }).ToList();
            var propertiesConfigurationText =
                columnPropertiesTemplateConfiguration.Select(c => c.TransformText()).ToList();
            var allPropertiesText = string.Join("\r\n", propertiesConfigurationText);
            var relationConfigurationText = t.ParentPropertyMetaInfo != null 
                ? (new RelationConfiguration(){ParentPropertyMetaInfo = t.ParentPropertyMetaInfo}).TransformText()
                :  String.Empty;
            var dataContextForTable = new DataContextTableConfiguration
            {
                TableName = t.TableName,
                TableClassName = t.TableClassName,
                CommaseperatedkeyColumns = string.Join(",", t.TablePrimaryKeyColumns.Select( c => $"e.{c}")), 
                PKName = $"{t.TableName}PK",
                IgnorePropertyNames = ignoreProperties,
                TablePropertiesConfiguration = allPropertiesText,
                RelationConfiguration = relationConfigurationText
            };
            var tableText = dataContextForTable.TransformText();
            return tableText;
        }
    }

    
}
