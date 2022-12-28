
IF TYPE_ID(N'AutoTableAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE AutoTableAppDtTbl
END

CREATE TYPE AutoTableAppDtTbl AS TABLE
(
[Id] int  NULL
,[Name] nchar (10) NULL
,[C] char (1) NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)