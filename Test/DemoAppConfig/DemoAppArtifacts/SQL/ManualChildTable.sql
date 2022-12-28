
IF TYPE_ID(N'ManualChildTableAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE ManualChildTableAppDtTbl
END

CREATE TYPE ManualChildTableAppDtTbl AS TABLE
(
[ChildID] int  NULL
,[ParentID] int  NULL
,[ChildName] nvarchar (50) NULL
,[TouchedWhen] datetimeoffset  NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)