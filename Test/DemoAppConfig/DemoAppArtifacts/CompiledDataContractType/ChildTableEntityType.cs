
            namespace DemoApp.Server.CompiledModels
            {
                internal partial class ChildTableEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "DemoApp.DataContract.ChildTable",
                typeof(DemoApp.DataContract.ChildTable),
                baseEntityType);

                var ChildID = runtimeEntityType.AddProperty("ChildID",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.ChildTable).GetProperty("ChildID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ChildTable).GetField("<ChildID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
valueGenerated: ValueGenerated.OnAdd,
afterSaveBehavior: PropertySaveBehavior.Throw);
ChildID.AddAnnotation("Relational:ColumnName","ChildID");
ChildID.AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.IdentityColumn);
var ParentID = runtimeEntityType.AddProperty("ParentID",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.ChildTable).GetProperty("ParentID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ChildTable).GetField("<ParentID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ParentID.AddAnnotation("Relational:ColumnName","ParentID");
var ChildName = runtimeEntityType.AddProperty("ChildName",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.ChildTable).GetProperty("ChildName",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ChildTable).GetField("<ChildName>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:10);
ChildName.AddAnnotation("Relational:ColumnName","ChildName");
                
                var key = runtimeEntityType.AddKey(new[] { ChildID });
key.AddAnnotation("Relational:Name","ChildTablePK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","ChildTable");
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
                    propertyInfo: typeof(DemoApp.DataContract.ChildTable).GetProperty("AutoTable",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(DemoApp.DataContract.ChildTable).GetField("<AutoTable>k__BackingField",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            
                
                    var childTableList = principalEntityType.AddNavigation("ChildTableList",
                    runtimeForeignKey,
                    onDependent: false,
                    typeof(List<DemoApp.DataContract.ChildTable>),
                    propertyInfo: typeof(DemoApp.DataContract.AutoTable).GetProperty("ChildTableList",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(DemoApp.DataContract.AutoTable).GetField("<ChildTableList>k__BackingField",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            
                return runtimeForeignKey;
            }
            

                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            