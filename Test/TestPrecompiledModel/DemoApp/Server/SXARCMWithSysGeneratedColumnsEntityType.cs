
            namespace DemoApp.Server.CompiledModels
            {
                internal partial class SXARCMWithSysGeneratedColumnsEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "DemoApp.DataContract.SXARCMWithSysGeneratedColumns",
                typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns),
                baseEntityType);

                var Build = runtimeEntityType.AddProperty("Build",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetProperty("Build",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetField("<Build>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
Build.AddAnnotation("Relational:ColumnName","Build");
var TouchedBy = runtimeEntityType.AddProperty("TouchedBy",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetProperty("TouchedBy",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetField("<TouchedBy>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50);
TouchedBy.AddAnnotation("Relational:ColumnName","TouchedBy");
var TouchedWhenUTC = runtimeEntityType.AddProperty("TouchedWhenUTC",
typeof(System.DateTimeOffset),
propertyInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetProperty("TouchedWhenUTC",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetField("<TouchedWhenUTC>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
TouchedWhenUTC.AddAnnotation("Relational:ColumnName","TouchedWhenUTC");
var CreatedBy = runtimeEntityType.AddProperty("CreatedBy",
typeof(string),
propertyInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetProperty("CreatedBy",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetField("<CreatedBy>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50);
CreatedBy.AddAnnotation("Relational:ColumnName","CreatedBy");
var CreatedWhenUTC = runtimeEntityType.AddProperty("CreatedWhenUTC",
typeof(System.DateTimeOffset),
propertyInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetProperty("CreatedWhenUTC",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetField("<CreatedWhenUTC>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
CreatedWhenUTC.AddAnnotation("Relational:ColumnName","CreatedWhenUTC");
var MSRowVersion = runtimeEntityType.AddProperty("MSRowVersion",
typeof(byte[]),
propertyInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetProperty("MSRowVersion",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetField("<MSRowVersion>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
valueGenerated: ValueGenerated.OnAdd,
afterSaveBehavior: PropertySaveBehavior.Ignore,
concurrencyToken: true);
MSRowVersion.AddAnnotation("Relational:ColumnName","MSRowVersion");
MSRowVersion.AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.None);
var ID = runtimeEntityType.AddProperty("ID",
typeof(long),
propertyInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetProperty("ID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetField("<ID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
valueGenerated: ValueGenerated.OnAdd,
afterSaveBehavior: PropertySaveBehavior.Throw);
ID.AddAnnotation("Relational:ColumnName","ID");
ID.AddAnnotation("SqlServer:ValueGenerationStrategy",SqlServerValueGenerationStrategy.IdentityColumn);
var Amount = runtimeEntityType.AddProperty("Amount",
typeof(int),
propertyInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetProperty("Amount",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(DemoApp.DataContract.SXARCMWithSysGeneratedColumns).GetField("<Amount>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
Amount.AddAnnotation("Relational:ColumnName","Amount");
                
                var key = runtimeEntityType.AddKey(new[] { ID });
key.AddAnnotation("Relational:Name","SXARCMWithSysGeneratedColumnsPK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","SXARCMWithSysGeneratedColumns");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            