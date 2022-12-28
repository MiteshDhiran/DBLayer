
            namespace DemoApp.Server.CompiledModels
            {
                internal partial class GrandChildEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "DemoApp.DataContract.GrandChild",
                typeof(DemoApp.DataContract.GrandChild),
                baseEntityType);

                var GrandchildID = runtimeEntityType.AddProperty("GrandchildID",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.GrandChild).GetProperty("GrandchildID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.GrandChild).GetField("<GrandchildID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
valueGenerated: ValueGenerated.OnAdd,
afterSaveBehavior: PropertySaveBehavior.Throw);
GrandchildID.AddAnnotation("Relational:ColumnName","GrandchildID");
GrandchildID.AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.IdentityColumn);
var GrandChildName = runtimeEntityType.AddProperty("GrandChildName",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.GrandChild).GetProperty("GrandChildName",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.GrandChild).GetField("<GrandChildName>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50);
GrandChildName.AddAnnotation("Relational:ColumnName","GrandChildName");
var ChildID = runtimeEntityType.AddProperty("ChildID",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.GrandChild).GetProperty("ChildID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.GrandChild).GetField("<ChildID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ChildID.AddAnnotation("Relational:ColumnName","ChildID");
                
                var key = runtimeEntityType.AddKey(new[] { GrandchildID });
key.AddAnnotation("Relational:Name","GrandChildPK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","GrandChild");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
            public static RuntimeForeignKey CreateForeignKey1(RuntimeEntityType declaringEntityType, RuntimeEntityType principalEntityType)
            {
                
                var runtimeForeignKey = declaringEntityType.AddForeignKey(new[] { declaringEntityType.FindProperty("ChildID") },
                    principalEntityType.FindKey(new[] { principalEntityType.FindProperty("ChildID") }),
                    principalEntityType,
                    required: true);
            
            
                
                    var parentTable = declaringEntityType.AddNavigation("ChildTable",
                    runtimeForeignKey,
                    onDependent: true,
                    typeof(DemoApp.DataContract.ChildTable),
                    propertyInfo: typeof(DemoApp.DataContract.GrandChild).GetProperty("ChildTable",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(DemoApp.DataContract.GrandChild).GetField("<ChildTable>k__BackingField",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            
                
                    var childTableList = principalEntityType.AddNavigation("GrandChild",
                    runtimeForeignKey,
                    onDependent: false,
                    typeof(List<DemoApp.DataContract.GrandChild>),
                    propertyInfo: typeof(DemoApp.DataContract.ChildTable).GetProperty("GrandChild",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(DemoApp.DataContract.ChildTable).GetField("<GrandChild>k__BackingField",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            
                return runtimeForeignKey;
            }
            

                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            