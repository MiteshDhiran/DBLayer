using ConnectAndSell.DataAccessStandard.Common.DataContract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ConnectAndSell.Dal.Generator.Common;

namespace ConnectAndSell.Dal.Generator
{
    internal static class CompiledTypeFileWritter
    {
       internal static void GenerateCompileTypeFiles(DataContextGeneratorParams dataContextGeneratorParams, 
                                        List<TableDataContractMetaInfo> tableDataContractMetaInfos, 
                                        string baseOutputFolderPath)
       {

            
            var dataContractTypeFolderPath = Path.Combine(baseOutputFolderPath,"CompiledDataContractType");
            if (!Directory.Exists(dataContractTypeFolderPath))
            {
                Directory.CreateDirectory(dataContractTypeFolderPath);
            }
            var compiledModelNameSpace =
                string.IsNullOrEmpty(dataContextGeneratorParams.ProjectDataContextNameSpace) 
                    ? "EFCompiledModels"
                    : dataContextGeneratorParams.ProjectDataContextNameSpace;
                    
            compiledModelNameSpace = $"{compiledModelNameSpace}.CompiledModels";
            
            foreach (var tableDataContractMetaInfo in tableDataContractMetaInfos)
            {
               var tableTypeText = GetTableCompileTypeClassTextContent(dataContextGeneratorParams
                                                                        ,tableDataContractMetaInfo
                                                                        ,compiledModelNameSpace
                                                                        );
               var tableDataContractTypeDirectoryPath =
                         string.IsNullOrEmpty(tableDataContractMetaInfo.DomainName) == false ?
                         Path.Combine(dataContractTypeFolderPath,tableDataContractMetaInfo.DomainName)
                         : dataContractTypeFolderPath;

                if (!Directory.Exists(tableDataContractTypeDirectoryPath))
                {
                    Directory.CreateDirectory(tableDataContractTypeDirectoryPath);
                }

                var tableTypeFilePath = Path.Combine(tableDataContractTypeDirectoryPath,
                    $"{tableDataContractMetaInfo.TableClassName}EntityType.cs");
                File.WriteAllText(tableTypeFilePath,tableTypeText);
                Console.WriteLine($"Generated Type file at:{tableTypeFilePath}");
            }
            var contextModelFilePath = Path.Combine(dataContractTypeFolderPath,
                        $"AppDbContextModel.cs");
            
            var dbContextModelClassName = $"{dataContextGeneratorParams.DataContextClassName}";//AppDbContextModel
            var domainContextModelClassText = GetCreateContextModelClassText(compiledModelNameSpace,dbContextModelClassName,tableDataContractMetaInfos);
            File.WriteAllText(contextModelFilePath,domainContextModelClassText);

        }

        static string ContextCompiledModel(DataContextGeneratorParams dataContextGeneratorParams
                     ,List<TableDataContractMetaInfo> tableDataContractMetaInfos)
        {
            var createTableTypeList = tableDataContractMetaInfos.Select(c => $"var {c.TableClassName.ToLower()} = {c.TableClassName}EntityType.Create(this);");
            var createText = string.Join("\r\n",createTableTypeList);

            var createTableAnnotationList = tableDataContractMetaInfos.Select(c => $"{c.TableClassName}EntityType.CreateAnnotations({c.TableClassName.ToLower()});");
            var createAnnotationText = string.Join("\r\n",createTableAnnotationList);
            return $"{ createText}\r\n{createAnnotationText}";
        }
        
        static string GetTableCompileTypeClassTextContent(DataContextGeneratorParams dataContextGeneratorParams
        ,TableDataContractMetaInfo tableDataContractMetaInfo, string compiledModelNameSpace)
        {
            var tableClassFullName = tableDataContractMetaInfo.TableClassFullName;
            var tableName = tableDataContractMetaInfo.TableClassName;
            //var compiledModelNameSpace =
            //string.IsNullOrEmpty(dataContextGeneratorParams.ProjectDataContextNameSpace) ? "EFCompiledModels" :
            //    dataContextGeneratorParams.ProjectDataContextNameSpace;
            var createForeignKeyMethodBody = GetCreateForeignKeyMethodBody(tableDataContractMetaInfo);
            var retVal =
            $@"
            namespace {compiledModelNameSpace}
            {{
                internal partial class {tableName}EntityType
                {{
                    {CreateTableText(dataContextGeneratorParams,tableDataContractMetaInfo)}
                    
                    {CreateTableAnnotationText(dataContextGeneratorParams,tableDataContractMetaInfo)}

                    {createForeignKeyMethodBody}
                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }}
            }}
            ";
            return retVal;
        }

        private static string GetCreateForeignKeyMethodBodyForSingleFK(TableDataContractMetaInfo tableDataContractMetaInfo,ParentPropertyMetaInfo parentPropertyMetaInfo, int index)
        {
            var parentPropertyNames = string.Join(" , ",
                    parentPropertyMetaInfo.ChildPropertyNames
                    .Select(p => $"declaringEntityType.FindProperty(\"{p}\")")
                );
            var principalPropertyNames = string.Join(" , ",
                    parentPropertyMetaInfo.ParentPropertyNames
                    .Select(p => $"principalEntityType.FindProperty(\"{p}\")")
                );
            string runtimeForeignKeyText =
            $@"
                var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] {{ {parentPropertyNames} }},
                    principalEntityType.FindKey(new[] {{ {principalPropertyNames} }}),
                    principalEntityType,
                    required: true);
            
            ";
            
            string parentTableNavigationEntityRefText =
            $@"
                    var parentTable = declaringEntityType.AddNavigation(""{parentPropertyMetaInfo.EntityRefPropertyName}"",
                    runtimeForeignKey,
                    onDependent: true,
                    typeof({parentPropertyMetaInfo.ParentClassFullName}),
                    propertyInfo: typeof({parentPropertyMetaInfo.ChildClassFullName}).GetProperty(""{parentPropertyMetaInfo.ParentClassName}"",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof({parentPropertyMetaInfo.ChildClassFullName}).GetField(""<{parentPropertyMetaInfo.ParentClassName}>k__BackingField"",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            ";

            string childEntitySetText =
            $@"
                    var childTableList = principalEntityType.AddNavigation(""{parentPropertyMetaInfo.EntitySetPropertyName}"",
                    runtimeForeignKey,
                    onDependent: false,
                    typeof(List<{parentPropertyMetaInfo.ChildClassFullName}>),
                    propertyInfo: typeof({parentPropertyMetaInfo.ParentClassFullName}).GetProperty(""{parentPropertyMetaInfo.EntitySetPropertyName}"",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof({parentPropertyMetaInfo.ParentClassFullName}).GetField(""<{parentPropertyMetaInfo.EntitySetPropertyName}>k__BackingField"",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            ";

            string retVal =
            $@"
            public static RuntimeForeignKey CreateForeignKey{index}(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
            {{
                {runtimeForeignKeyText}
                {parentTableNavigationEntityRefText}
                {childEntitySetText}
                return runtimeForeignKey;
            }}
            ";
            return retVal;
            
            /*
             * public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
            {
                var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("ParentID") },
                    principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id") }),
                    principalEntityType,
                    required: true);

                var autoTable = declaringEntityType.AddNavigation("AutoTable",
                    runtimeForeignKey,
                    onDependent: true,
                    typeof(AutoTable),
                    propertyInfo: typeof(ChildTable).GetProperty("AutoTable", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(ChildTable).GetField("<AutoTable>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

                var childTableList = principalEntityType.AddNavigation("ChildTableList",
                    runtimeForeignKey,
                    onDependent: false,
                    typeof(List<ChildTable>),
                    propertyInfo: typeof(AutoTable).GetProperty("ChildTableList", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(AutoTable).GetField("<ChildTableList>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));

                return runtimeForeignKey;
            }
             */

        }
        private static string GetCreateForeignKeyMethodBody(TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            if(tableDataContractMetaInfo.ParentPropertyMetaInfoList != null && tableDataContractMetaInfo.ParentPropertyMetaInfoList.Any())
            {
                var index = 1;
                var stringBuilder = new StringBuilder();
                foreach (var item in tableDataContractMetaInfo.ParentPropertyMetaInfoList)
                {
                    var txt = GetCreateForeignKeyMethodBodyForSingleFK(tableDataContractMetaInfo,item,index);
                    stringBuilder.AppendLine(txt);
                    index++;
                }
                return stringBuilder.ToString();
            }
            else
            {
                return "";
            }
        }

        static string CreateTableText(DataContextGeneratorParams dataContextGeneratorParams,TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            var columnTypeText = string.Join("\r\n",tableDataContractMetaInfo.ColumnPropertiesList.Select(c => GetColumnTypeText(tableDataContractMetaInfo,c)));
            var tableClassName = tableDataContractMetaInfo.TableClassFullName;
            var allKeysCommaSeparated = string.Join(",",tableDataContractMetaInfo.ColumnPropertiesList.Where(c => c.IsPKColumn).OrderBy(c => c.PKOrdinalPosition).Select(c => c.ColumnPropertyName));
            var keyDefinitionText = $"var key = runtimeEntityType.AddKey(new[] {{ {allKeysCommaSeparated} }});";
            var keyAnnotationText = $"key.AddAnnotation(\"Relational:Name\",\"{tableDataContractMetaInfo.TableName}PK\");";
            var addPrimaryKeyText = $"{keyDefinitionText}\r\n{keyAnnotationText}\r\nruntimeEntityType.SetPrimaryKey(key);";
            /*
             * 
             */
            var retVal =
            $@"
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {{
                var runtimeEntityType = model.AddEntityType(
                ""{tableClassName}"",
                typeof({tableClassName}),
                baseEntityType);

                {columnTypeText}
                
                {addPrimaryKeyText}
                
                return runtimeEntityType;
            }}
            ";

            return retVal;
        }

        static string CreateTableAnnotationText(DataContextGeneratorParams dataContextGeneratorParams,TableDataContractMetaInfo tableDataContractMetaInfo)
        {
            var retVal =
            $@"
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {{
                    runtimeEntityType.AddAnnotation(""Relational: FunctionName"",null);
                    runtimeEntityType.AddAnnotation(""Relational:Schema"",null);
                    runtimeEntityType.AddAnnotation(""Relational:SqlQuery"",null);
                    runtimeEntityType.AddAnnotation(""Relational:TableName"",""{tableDataContractMetaInfo.TableName}"");
                    runtimeEntityType.AddAnnotation(""Relational:ViewName"",null);
                    runtimeEntityType.AddAnnotation(""Relational:ViewSchema"",null);
                    Customize(runtimeEntityType);
            }}";
            return retVal;
        }

        static string GetColumnTypeText(TableDataContractMetaInfo tableDataContractMetaInfo,ColumnPropertyMetaInfo c)
        {
            /*
             * var id = runtimeEntityType.AddProperty(
                "Id",
                typeof(int),
                propertyInfo: typeof(AutoTable).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(AutoTable).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                valueGenerated: ValueGenerated.OnAdd,
                afterSaveBehavior: PropertySaveBehavior.Throw);
            id.AddAnnotation("Relational:ColumnName", "Id");
            id.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
             */

            /*
             *  var mSRowVersion = runtimeEntityType.AddProperty(
               "MSRowVersion",
               typeof(byte[]),
               propertyInfo: typeof(TableWithMSRowVersion).GetProperty("MSRowVersion", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
               fieldInfo: typeof(TableWithMSRowVersion).GetField("<MSRowVersion>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
               valueGenerated: ValueGenerated.OnUpdate,
               afterSaveBehavior: PropertySaveBehavior.Ignore);
           mSRowVersion.AddAnnotation("Relational:ColumnName", "MSRowVersion");
           mSRowVersion.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);
             */
            var backingFieldName = string.IsNullOrEmpty(c.BackingFieldName) == false
                ? c.BackingFieldName
                : $"\"<{c.ColumnPropertyName}>k__BackingField\""
                ;

            var runtimePropertyTypeVariableName = c.ColumnPropertyName;
            var propertyName = c.ColumnPropertyName;
            var propertyNameInQuote = $"\"{c.ColumnPropertyName}\"";
            var columnName = c.ColumnName;
            var propertyDataType = $"typeof({ c.NetDataType})";
            var propertyClassName = tableDataContractMetaInfo.TableClassFullName;
            var propertyInfoText = $"typeof({propertyClassName}).GetProperty(\"{propertyName}\",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)";
            var fieldInfoText = $"typeof({propertyClassName}).GetField({backingFieldName}, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)";
            var valueGeneratedText = c.IsGeneratedOnAdd ? "valueGenerated: ValueGenerated.OnAdd" : "";
            List<string> constructorParameters = new List<string>() { propertyNameInQuote
            ,propertyDataType
            ,$"propertyInfo:{ propertyInfoText}"
            ,$"fieldInfo:{ fieldInfoText}"
            };
            if (c.IsGeneratedOnAdd)
            {
                constructorParameters.Add("valueGenerated: ValueGenerated.OnAdd");
            }
            if (c.IsIdentity)
            {
                constructorParameters.Add("afterSaveBehavior: PropertySaveBehavior.Throw");
            }
            if (c.IsVersion)
            {
                constructorParameters.Add("afterSaveBehavior: PropertySaveBehavior.Ignore");
            }
            if (c.StringMaxLength != null && c.StringMaxLength > 0 && c.NetDataType.Equals("string",StringComparison.InvariantCultureIgnoreCase))
            {
                constructorParameters.Add($"maxLength:{c.StringMaxLength}");
            }
            if (c.IsGeneratedOnUpdate && c.IsGeneratedOnAdd == false)
            {
                constructorParameters.Add("valueGenerated: ValueGenerated.OnUpdate");
            }
            if (c.IsNullable)
            {
                constructorParameters.Add("nullable: true");
            }
            if (c.IsVersion)
            {
                constructorParameters.Add("concurrencyToken: true");
            }
            var commaSeparatedArgs = string.Join(",\r\n",constructorParameters);
            var columnNameAnnotationText = $"{runtimePropertyTypeVariableName}.AddAnnotation(\"Relational:ColumnName\",\"{columnName}\");";
            List<string> annotations = new List<string>() { columnNameAnnotationText };
            //annotations
            if (c.IsIdentity)
            {
                annotations.Add($"{runtimePropertyTypeVariableName}.AddAnnotation(\"SqlServer:ValueGenerationStrategy\",SqlServerValueGenerationStrategy.IdentityColumn);");
            }
            if (c.IsVersion)
            {
                annotations.Add($"{runtimePropertyTypeVariableName}.AddAnnotation(\"SqlServer:ValueGenerationStrategy\",SqlServerValueGenerationStrategy.None);");
            }
            string propertyCreation = $"var {runtimePropertyTypeVariableName} = runtimeEntityType.AddProperty({commaSeparatedArgs});";
            var allAnnotationStatements = string.Join("\r\n",annotations);
            var retVal = $"{propertyCreation}\r\n{allAnnotationStatements}";
            return retVal;
        }

        static string GetCreateContextModelClassText(string compiledModelNameSpace,string dbContextModelClassName,List<TableDataContractMetaInfo> tableDataContractMetaInfos)
        {
            //var entityTypeName = $"{c.TableClassName}EntityType";
            
              var createTableTypeList = tableDataContractMetaInfos.Select(c => $"var {c.TableClassName.ToLower()} = {c.TableClassName}EntityType.Create(this);");
            var createTableText = string.Join("\r\n",createTableTypeList);

            var createTableAnnotationList = tableDataContractMetaInfos.Select(c => $"{c.TableClassName}EntityType.CreateAnnotations({c.TableClassName.ToLower()});");
            var createAnnotationText = string.Join("\r\n",createTableAnnotationList);

            //get the fk info
            //tableDataContractMetaInfo.ParentPropertyMetaInfo.ParentClassFullName
            //ChildTableEntityType.CreateForeignKey1(childTable, autoTable);
            //t.ParentPropertyMetaInfo.ParentClassName
            var relations = tableDataContractMetaInfos
                                .Where(t => t.ParentPropertyMetaInfoList != null
                                        && t.ParentPropertyMetaInfoList.Any()
                                        ).SelectMany(t => t.ParentPropertyMetaInfoList.Select((p,index) => new { T= t, P = p, Index = index + 1}))
                                        .Select(x => x)
                                        .ToList();

            var createFKRelationText =
                            relations.Select(r =>
                            {
                                var childTableVariable = $"{r.P.ChildClassName.ToLower()}";
                                var parentTableVariable = $"{r.P.ParentClassName.ToLower()}";
                                var childEntityTypeClassName = $"{r.P.ChildClassName}EntityType";
                                return $"{childEntityTypeClassName}.CreateForeignKey{r.Index}({childTableVariable}, {parentTableVariable});";
                            });

           
            var relationText = string.Join("\r\n",createFKRelationText);
            
            var commonText =
            @"
                                      AddAnnotation(""ProductVersion"", ""6.0.3"");
                                      AddAnnotation(""Relational:MaxIdentifierLength"",128);
                                      AddAnnotation(""SqlServer:ValueGenerationStrategy"",SqlServerValueGenerationStrategy.IdentityColumn);

            ";


            var retVal = 
            $@"
                using {compiledModelNameSpace};
                
                public partial class {dbContextModelClassName}: RuntimeModel
                {{
                
                    private static {dbContextModelClassName} _instance;
                    public static IModel Instance => _instance;
                    
                    static {dbContextModelClassName}()
                    {{
                        var model = new {dbContextModelClassName}();
                        model.Initialize();
                        //model.Customize();
                        _instance = model;
                    }}
        
                    void Initialize()
                    {{
                          {createTableText}
                          {relationText}
                          {createAnnotationText}
                          {commonText}
                    }} 
                }}
            ";
            return retVal;
            /*
             * public partial class DemoAppDbContextModel
    {
        partial void Initialize()
        {
            var autoTable = AutoTableEntityType.Create(this);
            var childTable = ChildTableEntityType.Create(this);
            var grandChild = GrandChildEntityType.Create(this);
            var manualChildTable = ManualChildTableEntityType.Create(this);
            var manualTable = ManualTableEntityType.Create(this);
            var secondChildTable = SecondChildTableEntityType.Create(this);

            ChildTableEntityType.CreateForeignKey1(childTable, autoTable);
            GrandChildEntityType.CreateForeignKey1(grandChild, childTable);
            ManualChildTableEntityType.CreateForeignKey1(manualChildTable, manualTable);
            SecondChildTableEntityType.CreateForeignKey1(secondChildTable, autoTable);

            AutoTableEntityType.CreateAnnotations(autoTable);
            ChildTableEntityType.CreateAnnotations(childTable);
            GrandChildEntityType.CreateAnnotations(grandChild);
            ManualChildTableEntityType.CreateAnnotations(manualChildTable);
            ManualTableEntityType.CreateAnnotations(manualTable);
            SecondChildTableEntityType.CreateAnnotations(secondChildTable);

            AddAnnotation("ProductVersion", "6.0.3");
            AddAnnotation("Relational:MaxIdentifierLength", 128);
            AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
             */
        }


    }
}
