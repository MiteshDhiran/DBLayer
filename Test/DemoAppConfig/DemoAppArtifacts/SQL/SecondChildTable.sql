
IF TYPE_ID(N'SecondChildTableAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE SecondChildTableAppDtTbl
END

CREATE TYPE SecondChildTableAppDtTbl AS TABLE
(
[SecondChildID] int  NULL
,[ParentID] int  NULL
,[Name] nvarchar (50) NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)