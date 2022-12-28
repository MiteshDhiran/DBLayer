
IF TYPE_ID(N'category_counts_pivotAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE category_counts_pivotAppDtTbl
END

CREATE TYPE category_counts_pivotAppDtTbl AS TABLE
(
[Checkout] int  NULL
,[RestrictedForNextCall] int  NULL
,[DNC] int  NULL
,[IsDisqualified] int  NULL
,[IsQualifying] int  NULL
,[PhonePenalized] int  NULL
,[EmergencyArea] int  NULL
,[GDPR] int  NULL
,[Trigger] int  NULL
,[MultiTouch] int  NULL
,[MaxAttemptsPerDayReached] int  NULL
,[MaxAttemptsPerListReached] int  NULL
,[Callable] int  NULL
,[NotInBusinessHours] int  NULL
,[InLunchHours] int  NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)