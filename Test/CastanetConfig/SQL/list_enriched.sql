
IF TYPE_ID(N'list_enrichedAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE list_enrichedAppDtTbl
END

CREATE TYPE list_enrichedAppDtTbl AS TABLE
(
[CID] int  NULL
,[FN] nvarchar (100) NULL
,[LN] nvarchar (100) NULL
,[CN] nvarchar (500) NULL
,[Title] nvarchar (1000) NULL
,[P1] varchar (50) NULL
,[P2] varchar (50) NULL
,[P3] varchar (50) NULL
,[C] varchar (50) NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)