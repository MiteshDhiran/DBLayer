
            namespace DemoApp.Server.CompiledModels
            {
                internal partial class ManualTableEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "DemoApp.DataContract.ManualTable",
                typeof(DemoApp.DataContract.ManualTable),
                baseEntityType);

                var Id = runtimeEntityType.AddProperty("Id",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.ManualTable).GetProperty("Id",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ManualTable).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
Id.AddAnnotation("Relational:ColumnName","Id");
var Name = runtimeEntityType.AddProperty("Name",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.ManualTable).GetProperty("Name",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ManualTable).GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50);
Name.AddAnnotation("Relational:ColumnName","Name");
var TouchedWhen = runtimeEntityType.AddProperty("TouchedWhen",
typeof(System.DateTimeOffset),
propertyInfo:typeof(DemoApp.DataContract.ManualTable).GetProperty("TouchedWhen",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.ManualTable).GetField("<TouchedWhen>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
TouchedWhen.AddAnnotation("Relational:ColumnName","TouchedWhen");
                
                var key = runtimeEntityType.AddKey(new[] { Id });
key.AddAnnotation("Relational:Name","ManualTablePK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","ManualTable");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            