
            namespace DemoApp.Server.CompiledModels
            {
                internal partial class ManualChildTableEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "DemoApp.DataContract.ManualChildTable",
                typeof(DemoApp.DataContract.ManualChildTable),
                baseEntityType);

                var ChildID = runtimeEntityType.AddProperty("ChildID",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.ManualChildTable).GetProperty("ChildID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ManualChildTable).GetField("<ChildID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ChildID.AddAnnotation("Relational:ColumnName","ChildID");
var ParentID = runtimeEntityType.AddProperty("ParentID",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.ManualChildTable).GetProperty("ParentID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ManualChildTable).GetField("<ParentID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ParentID.AddAnnotation("Relational:ColumnName","ParentID");
var ChildName = runtimeEntityType.AddProperty("ChildName",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.ManualChildTable).GetProperty("ChildName",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ManualChildTable).GetField("<ChildName>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50);
ChildName.AddAnnotation("Relational:ColumnName","ChildName");
var TouchedWhen = runtimeEntityType.AddProperty("TouchedWhen",
typeof(System.DateTimeOffset),
propertyInfo:typeof(DemoApp.DataContract.ManualChildTable).GetProperty("TouchedWhen",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ManualChildTable).GetField("<TouchedWhen>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
TouchedWhen.AddAnnotation("Relational:ColumnName","TouchedWhen");
                
                var key = runtimeEntityType.AddKey(new[] { ChildID });
key.AddAnnotation("Relational:Name","ManualChildTablePK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","ManualChildTable");
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
            
            
                
                    var parentTable = declaringEntityType.AddNavigation("ManualTable",
                    runtimeForeignKey,
                    onDependent: true,
                    typeof(DemoApp.DataContract.ManualTable),
                    propertyInfo: typeof(DemoApp.DataContract.ManualChildTable).GetProperty("ManualTable",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(DemoApp.DataContract.ManualChildTable).GetField("<ManualTable>k__BackingField",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            
                
                    var childTableList = principalEntityType.AddNavigation("ManualChildTable",
                    runtimeForeignKey,
                    onDependent: false,
                    typeof(List<DemoApp.DataContract.ManualChildTable>),
                    propertyInfo: typeof(DemoApp.DataContract.ManualTable).GetProperty("ManualChildTable",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                    fieldInfo: typeof(DemoApp.DataContract.ManualTable).GetField("<ManualChildTable>k__BackingField",BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
            
                return runtimeForeignKey;
            }
            

                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            