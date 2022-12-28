
IF TYPE_ID(N'ManualTableAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE ManualTableAppDtTbl
END

CREATE TYPE ManualTableAppDtTbl AS TABLE
(
[Id] int  NULL
,[Name] nvarchar (50) NULL
,[TouchedWhen] datetimeoffset  NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)