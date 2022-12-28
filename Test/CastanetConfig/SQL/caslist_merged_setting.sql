
IF TYPE_ID(N'caslist_merged_settingAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE caslist_merged_settingAppDtTbl
END

CREATE TYPE caslist_merged_settingAppDtTbl AS TABLE
(
[ListID] int  NULL
,[LeadPoolSize] int  NULL
,[IsConnectOnHello] bit  NULL
,[Delay] int  NULL
,[RestrictedAttemptsEnabled] bit  NULL
,[MaxAttemptsRestricted] bit  NULL
,[MaxCallPerContact] int  NULL
,[LeadPoolCheck] bit  NULL
,[ManagerCanEditLeadPoolSize] bit  NULL
,[ViewCallSummary] int  NULL
,[hasDialerMode] int  NULL
,[hadLightningMode] bit  NULL
,[SalesforceCRMUsersError] int  NULL
,[OODEnabled] bit  NULL
,[hasCOLVMode] bit  NULL
,[CallerID] varchar (20) NULL
,[UseSystemCallerId] bit  NULL
,[ListAssignmentType] tinyint  NULL
,[IsConnectOnHelloSetting] bit  NULL
,[ListType] tinyint  NULL
,[DilaerModeInt] int  NULL
,[ListCompanyID] int  NULL
,[AllowNumberToDial] bit  NULL
,[phoneNumberFieldString] varchar (50) NULL
,[IS_applyMaxAttemptsPerDayFilter] int  NULL
,[IS_applyMaxAttemptsPerListFilter] int  NULL
,[CallFutureCallBackContacts] bit  NULL
,[NextCallReferenceDateTime] datetime  NULL
,[EnableCompanytoRestrictCallingonEmergencyAreas] bit  NULL
,[BusinessStartHours] time  NULL
,[BusinessEndhours] time  NULL
,[BusinessLunchStartHours] time  NULL
,[BusinessLunchEndhours] time  NULL
,[AutomaticTimeZoneDetectionEnabled] bit  NULL
,[LunchTimeZoneDetectionEnabled] bit  NULL
,[IsBusinessHours] int  NULL
,[IsLunchHours] int  NULL
,[MaxAttempts] int  NULL
,[MaxAttemptsList] int  NULL
,[BadNumberFiltering] bit  NULL
,[WrongNumberFiltering] bit  NULL
,[DncFiltering] bit  NULL
,[UseProbableDirectNumberEnabled] bit  NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)