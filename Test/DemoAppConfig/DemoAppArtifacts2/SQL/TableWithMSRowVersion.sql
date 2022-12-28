
IF TYPE_ID(N'TableWithMSRowVersionAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE TableWithMSRowVersionAppDtTbl
END

CREATE TYPE TableWithMSRowVersionAppDtTbl AS TABLE
(
[Id] int  NULL
,[Name] nvarchar (50) NULL
,[MSRowVersion] varchar(200)  NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)