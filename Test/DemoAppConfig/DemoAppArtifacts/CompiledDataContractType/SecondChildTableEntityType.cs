
            namespace DemoApp.Server.CompiledModels
            {
                internal partial class SecondChildTableEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "DemoApp.DataContract.SecondChildTable",
                typeof(DemoApp.DataContract.SecondChildTable),
                baseEntityType);

                var SecondChildID = runtimeEntityType.AddProperty("SecondChildID",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.SecondChildTable).GetProperty("SecondChildID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SecondChildTable).GetField("<SecondChildID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
valueGenerated: ValueGenerated.OnAdd,
afterSaveBehavior: PropertySaveBehavior.Throw);
SecondChildID.AddAnnotation("Relational:ColumnName","SecondChildID");
SecondChildID.AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.IdentityColumn);
var ParentID = runtimeEntityType.AddProperty("ParentID",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.SecondChildTable).GetProperty("ParentID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SecondChildTable).GetField("<ParentID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ParentID.AddAnnotation("Relational:ColumnName","ParentID");
var Name = runtimeEntityType.AddProperty("Name",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.SecondChildTable).GetProperty("Name",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SecondChildTable).GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50);
Name.AddAnnotation("Relational:ColumnName","Name");
                
                var key = runtimeEntityType.AddKey(new[] { SecondChildID });
key.AddAnnotation("Relational:Name","SecondChildTablePK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","SecondChildTable");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
            public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
            {
                
                var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("ParentID") },
                    principalEntityType.FindKey(new[] { principalEntityType.FindProperty("Id") }),
                    principalEntityType,
                    required: true);
            
            
                
                    var parentTable = declaringEntityType.AddNavigation("AutoTable",
                    runtimeForeignKey,
                    onDependent: true,
                    typeof(DemoApp.DataContract.AutoTable),
                    propertyInfo: typeof(DemoApp.DataContract.SecondChildTable).GetProperty("AutoTable",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(DemoApp.DataContract.SecondChildTable).GetField("<AutoTable>k__BackingField",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            
                
                    var childTableList = principalEntityType.AddNavigation("SecondChildTable",
                    runtimeForeignKey,
                    onDependent: false,
                    typeof(List<DemoApp.DataContract.SecondChildTable>),
                    propertyInfo: typeof(DemoApp.DataContract.AutoTable).GetProperty("SecondChildTable",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(DemoApp.DataContract.AutoTable).GetField("<SecondChildTable>k__BackingField",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            
                return runtimeForeignKey;
            }
            

                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            