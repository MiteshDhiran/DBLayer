
            namespace DemoApp.Server.CompiledModels
            {
                internal partial class AutoTableEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "DemoApp.DataContract.AutoTable",
                typeof(DemoApp.DataContract.AutoTable),
                baseEntityType);

                var Id = runtimeEntityType.AddProperty("Id",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.AutoTable).GetProperty("Id",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.AutoTable).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
valueGenerated: ValueGenerated.OnAdd,
afterSaveBehavior: PropertySaveBehavior.Throw);
Id.AddAnnotation("Relational:ColumnName","Id");
Id.AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.IdentityColumn);
var Name = runtimeEntityType.AddProperty("Name",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.AutoTable).GetProperty("Name",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.AutoTable).GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:10);
Name.AddAnnotation("Relational:ColumnName","Name");
var C = runtimeEntityType.AddProperty("C",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.AutoTable).GetProperty("C",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.AutoTable).GetField("<C>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:1,
nullable: true);
C.AddAnnotation("Relational:ColumnName","C");
                
                var key = runtimeEntityType.AddKey(new[] { Id });
key.AddAnnotation("Relational:Name","AutoTablePK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","AutoTable");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            