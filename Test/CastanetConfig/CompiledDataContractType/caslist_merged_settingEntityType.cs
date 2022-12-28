
            namespace CastApp.Server.CompiledModels
            {
                internal partial class caslist_merged_settingEntityType
                {
                    
            public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
            {
                var runtimeEntityType = model.AddEntityType(
                "CastApp.DataContract.caslist_merged_setting",
                typeof(CastApp.DataContract.caslist_merged_setting),
                baseEntityType);

                var ListID = runtimeEntityType.AddProperty("ListID",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("ListID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<ListID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ListID.AddAnnotation("Relational:ColumnName","ListID");
var LeadPoolSize = runtimeEntityType.AddProperty("LeadPoolSize",
typeof(Nullable<int>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("LeadPoolSize",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<LeadPoolSize>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
LeadPoolSize.AddAnnotation("Relational:ColumnName","LeadPoolSize");
var IsConnectOnHello = runtimeEntityType.AddProperty("IsConnectOnHello",
typeof(bool),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("IsConnectOnHello",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<IsConnectOnHello>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
IsConnectOnHello.AddAnnotation("Relational:ColumnName","IsConnectOnHello");
var Delay = runtimeEntityType.AddProperty("Delay",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("Delay",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<Delay>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
Delay.AddAnnotation("Relational:ColumnName","Delay");
var RestrictedAttemptsEnabled = runtimeEntityType.AddProperty("RestrictedAttemptsEnabled",
typeof(bool),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("RestrictedAttemptsEnabled",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<RestrictedAttemptsEnabled>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
RestrictedAttemptsEnabled.AddAnnotation("Relational:ColumnName","RestrictedAttemptsEnabled");
var MaxAttemptsRestricted = runtimeEntityType.AddProperty("MaxAttemptsRestricted",
typeof(bool),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("MaxAttemptsRestricted",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<MaxAttemptsRestricted>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
MaxAttemptsRestricted.AddAnnotation("Relational:ColumnName","MaxAttemptsRestricted");
var MaxCallPerContact = runtimeEntityType.AddProperty("MaxCallPerContact",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("MaxCallPerContact",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<MaxCallPerContact>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
MaxCallPerContact.AddAnnotation("Relational:ColumnName","MaxCallPerContact");
var LeadPoolCheck = runtimeEntityType.AddProperty("LeadPoolCheck",
typeof(bool),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("LeadPoolCheck",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<LeadPoolCheck>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
LeadPoolCheck.AddAnnotation("Relational:ColumnName","LeadPoolCheck");
var ManagerCanEditLeadPoolSize = runtimeEntityType.AddProperty("ManagerCanEditLeadPoolSize",
typeof(bool),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("ManagerCanEditLeadPoolSize",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<ManagerCanEditLeadPoolSize>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ManagerCanEditLeadPoolSize.AddAnnotation("Relational:ColumnName","ManagerCanEditLeadPoolSize");
var ViewCallSummary = runtimeEntityType.AddProperty("ViewCallSummary",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("ViewCallSummary",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<ViewCallSummary>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ViewCallSummary.AddAnnotation("Relational:ColumnName","ViewCallSummary");
var hasDialerMode = runtimeEntityType.AddProperty("hasDialerMode",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("hasDialerMode",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<hasDialerMode>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
hasDialerMode.AddAnnotation("Relational:ColumnName","hasDialerMode");
var hadLightningMode = runtimeEntityType.AddProperty("hadLightningMode",
typeof(bool),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("hadLightningMode",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<hadLightningMode>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
hadLightningMode.AddAnnotation("Relational:ColumnName","hadLightningMode");
var SalesforceCRMUsersError = runtimeEntityType.AddProperty("SalesforceCRMUsersError",
typeof(Nullable<int>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("SalesforceCRMUsersError",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<SalesforceCRMUsersError>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
SalesforceCRMUsersError.AddAnnotation("Relational:ColumnName","SalesforceCRMUsersError");
var OODEnabled = runtimeEntityType.AddProperty("OODEnabled",
typeof(bool),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("OODEnabled",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<OODEnabled>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
OODEnabled.AddAnnotation("Relational:ColumnName","OODEnabled");
var hasCOLVMode = runtimeEntityType.AddProperty("hasCOLVMode",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("hasCOLVMode",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<hasCOLVMode>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
hasCOLVMode.AddAnnotation("Relational:ColumnName","hasCOLVMode");
var CallerID = runtimeEntityType.AddProperty("CallerID",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("CallerID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<CallerID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:20,
nullable: true);
CallerID.AddAnnotation("Relational:ColumnName","CallerID");
var UseSystemCallerId = runtimeEntityType.AddProperty("UseSystemCallerId",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("UseSystemCallerId",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<UseSystemCallerId>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
UseSystemCallerId.AddAnnotation("Relational:ColumnName","UseSystemCallerId");
var ListAssignmentType = runtimeEntityType.AddProperty("ListAssignmentType",
typeof(byte),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("ListAssignmentType",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<ListAssignmentType>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ListAssignmentType.AddAnnotation("Relational:ColumnName","ListAssignmentType");
var IsConnectOnHelloSetting = runtimeEntityType.AddProperty("IsConnectOnHelloSetting",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("IsConnectOnHelloSetting",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<IsConnectOnHelloSetting>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
IsConnectOnHelloSetting.AddAnnotation("Relational:ColumnName","IsConnectOnHelloSetting");
var ListType = runtimeEntityType.AddProperty("ListType",
typeof(byte),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("ListType",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<ListType>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ListType.AddAnnotation("Relational:ColumnName","ListType");
var DilaerModeInt = runtimeEntityType.AddProperty("DilaerModeInt",
typeof(Nullable<int>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("DilaerModeInt",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<DilaerModeInt>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
DilaerModeInt.AddAnnotation("Relational:ColumnName","DilaerModeInt");
var ListCompanyID = runtimeEntityType.AddProperty("ListCompanyID",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("ListCompanyID",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<ListCompanyID>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
ListCompanyID.AddAnnotation("Relational:ColumnName","ListCompanyID");
var AllowNumberToDial = runtimeEntityType.AddProperty("AllowNumberToDial",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("AllowNumberToDial",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<AllowNumberToDial>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
AllowNumberToDial.AddAnnotation("Relational:ColumnName","AllowNumberToDial");
var phoneNumberFieldString = runtimeEntityType.AddProperty("phoneNumberFieldString",
typeof(string),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("phoneNumberFieldString",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<phoneNumberFieldString>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
maxLength:50,
nullable: true);
phoneNumberFieldString.AddAnnotation("Relational:ColumnName","phoneNumberFieldString");
var IS_applyMaxAttemptsPerDayFilter = runtimeEntityType.AddProperty("IS_applyMaxAttemptsPerDayFilter",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("IS_applyMaxAttemptsPerDayFilter",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<IS_applyMaxAttemptsPerDayFilter>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
IS_applyMaxAttemptsPerDayFilter.AddAnnotation("Relational:ColumnName","IS_applyMaxAttemptsPerDayFilter");
var IS_applyMaxAttemptsPerListFilter = runtimeEntityType.AddProperty("IS_applyMaxAttemptsPerListFilter",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("IS_applyMaxAttemptsPerListFilter",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<IS_applyMaxAttemptsPerListFilter>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
IS_applyMaxAttemptsPerListFilter.AddAnnotation("Relational:ColumnName","IS_applyMaxAttemptsPerListFilter");
var CallFutureCallBackContacts = runtimeEntityType.AddProperty("CallFutureCallBackContacts",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("CallFutureCallBackContacts",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<CallFutureCallBackContacts>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
CallFutureCallBackContacts.AddAnnotation("Relational:ColumnName","CallFutureCallBackContacts");
var NextCallReferenceDateTime = runtimeEntityType.AddProperty("NextCallReferenceDateTime",
typeof(Nullable<DateTime>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("NextCallReferenceDateTime",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<NextCallReferenceDateTime>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
NextCallReferenceDateTime.AddAnnotation("Relational:ColumnName","NextCallReferenceDateTime");
var EnableCompanytoRestrictCallingonEmergencyAreas = runtimeEntityType.AddProperty("EnableCompanytoRestrictCallingonEmergencyAreas",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("EnableCompanytoRestrictCallingonEmergencyAreas",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<EnableCompanytoRestrictCallingonEmergencyAreas>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
EnableCompanytoRestrictCallingonEmergencyAreas.AddAnnotation("Relational:ColumnName","EnableCompanytoRestrictCallingonEmergencyAreas");
var BusinessStartHours = runtimeEntityType.AddProperty("BusinessStartHours",
typeof(Nullable<TimeSpan>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("BusinessStartHours",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<BusinessStartHours>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
BusinessStartHours.AddAnnotation("Relational:ColumnName","BusinessStartHours");
var BusinessEndhours = runtimeEntityType.AddProperty("BusinessEndhours",
typeof(Nullable<TimeSpan>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("BusinessEndhours",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<BusinessEndhours>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
BusinessEndhours.AddAnnotation("Relational:ColumnName","BusinessEndhours");
var BusinessLunchStartHours = runtimeEntityType.AddProperty("BusinessLunchStartHours",
typeof(Nullable<TimeSpan>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("BusinessLunchStartHours",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<BusinessLunchStartHours>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
BusinessLunchStartHours.AddAnnotation("Relational:ColumnName","BusinessLunchStartHours");
var BusinessLunchEndhours = runtimeEntityType.AddProperty("BusinessLunchEndhours",
typeof(Nullable<TimeSpan>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("BusinessLunchEndhours",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<BusinessLunchEndhours>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
BusinessLunchEndhours.AddAnnotation("Relational:ColumnName","BusinessLunchEndhours");
var AutomaticTimeZoneDetectionEnabled = runtimeEntityType.AddProperty("AutomaticTimeZoneDetectionEnabled",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("AutomaticTimeZoneDetectionEnabled",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<AutomaticTimeZoneDetectionEnabled>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
AutomaticTimeZoneDetectionEnabled.AddAnnotation("Relational:ColumnName","AutomaticTimeZoneDetectionEnabled");
var LunchTimeZoneDetectionEnabled = runtimeEntityType.AddProperty("LunchTimeZoneDetectionEnabled",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("LunchTimeZoneDetectionEnabled",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<LunchTimeZoneDetectionEnabled>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
LunchTimeZoneDetectionEnabled.AddAnnotation("Relational:ColumnName","LunchTimeZoneDetectionEnabled");
var IsBusinessHours = runtimeEntityType.AddProperty("IsBusinessHours",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("IsBusinessHours",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<IsBusinessHours>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
IsBusinessHours.AddAnnotation("Relational:ColumnName","IsBusinessHours");
var IsLunchHours = runtimeEntityType.AddProperty("IsLunchHours",
typeof(int),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("IsLunchHours",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<IsLunchHours>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly));
IsLunchHours.AddAnnotation("Relational:ColumnName","IsLunchHours");
var MaxAttempts = runtimeEntityType.AddProperty("MaxAttempts",
typeof(Nullable<int>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("MaxAttempts",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<MaxAttempts>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
MaxAttempts.AddAnnotation("Relational:ColumnName","MaxAttempts");
var MaxAttemptsList = runtimeEntityType.AddProperty("MaxAttemptsList",
typeof(Nullable<int>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("MaxAttemptsList",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<MaxAttemptsList>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
MaxAttemptsList.AddAnnotation("Relational:ColumnName","MaxAttemptsList");
var BadNumberFiltering = runtimeEntityType.AddProperty("BadNumberFiltering",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("BadNumberFiltering",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<BadNumberFiltering>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
BadNumberFiltering.AddAnnotation("Relational:ColumnName","BadNumberFiltering");
var WrongNumberFiltering = runtimeEntityType.AddProperty("WrongNumberFiltering",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("WrongNumberFiltering",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<WrongNumberFiltering>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
WrongNumberFiltering.AddAnnotation("Relational:ColumnName","WrongNumberFiltering");
var DncFiltering = runtimeEntityType.AddProperty("DncFiltering",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("DncFiltering",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<DncFiltering>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
DncFiltering.AddAnnotation("Relational:ColumnName","DncFiltering");
var UseProbableDirectNumberEnabled = runtimeEntityType.AddProperty("UseProbableDirectNumberEnabled",
typeof(Nullable<bool>),
propertyInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetProperty("UseProbableDirectNumberEnabled",BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
fieldInfo:typeof(CastApp.DataContract.caslist_merged_setting).GetField("<UseProbableDirectNumberEnabled>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
nullable: true);
UseProbableDirectNumberEnabled.AddAnnotation("Relational:ColumnName","UseProbableDirectNumberEnabled");
                
                var key = runtimeEntityType.AddKey(new[] {  });
key.AddAnnotation("Relational:Name","caslist_merged_settingPK");
runtimeEntityType.SetPrimaryKey(key);
                
                return runtimeEntityType;
            }
            
                    
                    
            public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
                {
                    runtimeEntityType.AddAnnotation("Relational: FunctionName",null);
                    runtimeEntityType.AddAnnotation("Relational:Schema",null);
                    runtimeEntityType.AddAnnotation("Relational:SqlQuery",null);
                    runtimeEntityType.AddAnnotation("Relational:TableName","caslist_merged_setting");
                    runtimeEntityType.AddAnnotation("Relational:ViewName",null);
                    runtimeEntityType.AddAnnotation("Relational:ViewSchema",null);
                    Customize(runtimeEntityType);
            }

                    
                    
                    static partial void Customize(RuntimeEntityType runtimeEntityType);
                }
            }
            