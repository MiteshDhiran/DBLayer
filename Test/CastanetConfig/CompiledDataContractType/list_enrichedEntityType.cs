
            namespace CastApp.Server.CompiledModels
            {
                internal partial class list_enrichedEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "CastApp.DataContract.list_enriched",
                typeof(CastApp.DataContract.list_enriched),
                baseEntityType);

                var CID = runtimeEntityType.AddProperty("CID",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("CID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<CID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
CID.AddAnnotation("Relational:ColumnName","CID");
var FN = runtimeEntityType.AddProperty("FN",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("FN",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<FN>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:100,
nullable: true);
FN.AddAnnotation("Relational:ColumnName","FN");
var LN = runtimeEntityType.AddProperty("LN",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("LN",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<LN>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:100,
nullable: true);
LN.AddAnnotation("Relational:ColumnName","LN");
var CN = runtimeEntityType.AddProperty("CN",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("CN",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<CN>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:500,
nullable: true);
CN.AddAnnotation("Relational:ColumnName","CN");
var Title = runtimeEntityType.AddProperty("Title",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("Title",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<Title>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:1000,
nullable: true);
Title.AddAnnotation("Relational:ColumnName","Title");
var P1 = runtimeEntityType.AddProperty("P1",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("P1",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<P1>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50,
nullable: true);
P1.AddAnnotation("Relational:ColumnName","P1");
var P2 = runtimeEntityType.AddProperty("P2",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("P2",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<P2>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50,
nullable: true);
P2.AddAnnotation("Relational:ColumnName","P2");
var P3 = runtimeEntityType.AddProperty("P3",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("P3",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<P3>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50,
nullable: true);
P3.AddAnnotation("Relational:ColumnName","P3");
var C = runtimeEntityType.AddProperty("C",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.list_enriched).GetProperty("C",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.list_enriched).GetField("<C>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50);
C.AddAnnotation("Relational:ColumnName","C");
                
                var key = runtimeEntityType.AddKey(new[] {  });
key.AddAnnotation("Relational:Name","list_enrichedPK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","list_enriched");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            