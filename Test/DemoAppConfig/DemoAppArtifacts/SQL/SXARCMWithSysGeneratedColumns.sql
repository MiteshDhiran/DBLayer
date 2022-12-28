
IF TYPE_ID(N'SXARCMWithSysGeneratedColumnsAppDtTbl') IS NOT NULL
BEGIN
	DROP TYPE SXARCMWithSysGeneratedColumnsAppDtTbl
END

CREATE TYPE SXARCMWithSysGeneratedColumnsAppDtTbl AS TABLE
(
[Build] int  NULL
,[TouchedBy] varchar (50) NULL
,[TouchedWhenUTC] datetimeoffset  NULL
,[CreatedBy] varchar (50) NULL
,[CreatedWhenUTC] datetimeoffset  NULL
,[MSRowVersion] varchar(200)  NULL
,[ID] bigint  NULL
,[Amount] int  NULL
,[RID] [bigint] NULL
,[PRID] [bigint] NULL
)