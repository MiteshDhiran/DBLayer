
IF TYPE_ID(N'ChildTableAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE ChildTableAppDtTbl
END

CREATE TYPE ChildTableAppDtTbl AS TABLE
(
[ChildID] int  NULL
,[ParentID] int  NULL
,[ChildName] nchar (10) NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)