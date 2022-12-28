
            namespace DemoApp.Server.CompiledModels
            {
                internal partial class TableWithMSRowVersionEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "DemoApp.DataContract.TableWithMSRowVersion",
                typeof(DemoApp.DataContract.TableWithMSRowVersion),
                baseEntityType);

                var Id = runtimeEntityType.AddProperty("Id",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.TableWithMSRowVersion).GetProperty("Id",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.TableWithMSRowVersion).GetField("<Id>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
Id.AddAnnotation("Relational:ColumnName","Id");
var Name = runtimeEntityType.AddProperty("Name",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.TableWithMSRowVersion).GetProperty("Name",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.TableWithMSRowVersion).GetField("<Name>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50);
Name.AddAnnotation("Relational:ColumnName","Name");
var MSRowVersion = runtimeEntityType.AddProperty("MSRowVersion",
typeof(byte[]),
propertyInfo:typeof(DemoApp.DataContract.TableWithMSRowVersion).GetProperty("MSRowVersion",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.TableWithMSRowVersion).GetField("<MSRowVersion>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
valueGenerated: ValueGenerated.OnAdd,
afterSaveBehavior: PropertySaveBehavior.Ignore,
concurrencyToken: true);
MSRowVersion.AddAnnotation("Relational:ColumnName","MSRowVersion");
MSRowVersion.AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.None);
                
                var key = runtimeEntityType.AddKey(new[] { Id });
key.AddAnnotation("Relational:Name","TableWithMSRowVersionPK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","TableWithMSRowVersion");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            