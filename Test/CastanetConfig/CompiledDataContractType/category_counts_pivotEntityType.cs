
            namespace CastApp.Server.CompiledModels
            {
                internal partial class category_counts_pivotEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "CastApp.DataContract.category_counts_pivot",
                typeof(CastApp.DataContract.category_counts_pivot),
                baseEntityType);

                var Checkout = runtimeEntityType.AddProperty("Checkout",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("Checkout",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<Checkout>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
Checkout.AddAnnotation("Relational:ColumnName","Checkout");
var RestrictedForNextCall = runtimeEntityType.AddProperty("RestrictedForNextCall",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("RestrictedForNextCall",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<RestrictedForNextCall>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
RestrictedForNextCall.AddAnnotation("Relational:ColumnName","RestrictedForNextCall");
var DNC = runtimeEntityType.AddProperty("DNC",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("DNC",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<DNC>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
DNC.AddAnnotation("Relational:ColumnName","DNC");
var IsDisqualified = runtimeEntityType.AddProperty("IsDisqualified",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("IsDisqualified",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<IsDisqualified>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
IsDisqualified.AddAnnotation("Relational:ColumnName","IsDisqualified");
var IsQualifying = runtimeEntityType.AddProperty("IsQualifying",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("IsQualifying",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<IsQualifying>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
IsQualifying.AddAnnotation("Relational:ColumnName","IsQualifying");
var PhonePenalized = runtimeEntityType.AddProperty("PhonePenalized",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("PhonePenalized",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<PhonePenalized>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
PhonePenalized.AddAnnotation("Relational:ColumnName","PhonePenalized");
var EmergencyArea = runtimeEntityType.AddProperty("EmergencyArea",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("EmergencyArea",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<EmergencyArea>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
EmergencyArea.AddAnnotation("Relational:ColumnName","EmergencyArea");
var GDPR = runtimeEntityType.AddProperty("GDPR",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("GDPR",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<GDPR>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
GDPR.AddAnnotation("Relational:ColumnName","GDPR");
var Trigger = runtimeEntityType.AddProperty("Trigger",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("Trigger",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<Trigger>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
Trigger.AddAnnotation("Relational:ColumnName","Trigger");
var MultiTouch = runtimeEntityType.AddProperty("MultiTouch",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("MultiTouch",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<MultiTouch>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
MultiTouch.AddAnnotation("Relational:ColumnName","MultiTouch");
var MaxAttemptsPerDayReached = runtimeEntityType.AddProperty("MaxAttemptsPerDayReached",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("MaxAttemptsPerDayReached",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<MaxAttemptsPerDayReached>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
MaxAttemptsPerDayReached.AddAnnotation("Relational:ColumnName","MaxAttemptsPerDayReached");
var MaxAttemptsPerListReached = runtimeEntityType.AddProperty("MaxAttemptsPerListReached",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("MaxAttemptsPerListReached",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<MaxAttemptsPerListReached>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
MaxAttemptsPerListReached.AddAnnotation("Relational:ColumnName","MaxAttemptsPerListReached");
var Callable = runtimeEntityType.AddProperty("Callable",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("Callable",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<Callable>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
Callable.AddAnnotation("Relational:ColumnName","Callable");
var NotInBusinessHours = runtimeEntityType.AddProperty("NotInBusinessHours",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("NotInBusinessHours",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<NotInBusinessHours>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
NotInBusinessHours.AddAnnotation("Relational:ColumnName","NotInBusinessHours");
var InLunchHours = runtimeEntityType.AddProperty("InLunchHours",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.category_counts_pivot).GetProperty("InLunchHours",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.category_counts_pivot).GetField("<InLunchHours>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
InLunchHours.AddAnnotation("Relational:ColumnName","InLunchHours");
                
                var key = runtimeEntityType.AddKey(new[] {  });
key.AddAnnotation("Relational:Name","category_counts_pivotPK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","category_counts_pivot");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            