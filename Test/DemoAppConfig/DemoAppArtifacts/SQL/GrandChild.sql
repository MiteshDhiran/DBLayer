
IF TYPE_ID(N'GrandChildAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE GrandChildAppDtTbl
END

CREATE TYPE GrandChildAppDtTbl AS TABLE
(
[GrandchildID] int  NULL
,[GrandChildName] nvarchar (50) NULL
,[ChildID] int  NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)