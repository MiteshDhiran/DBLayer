USE [master]
GO
/****** Object:  Database [CDALDB]    Script Date: 9/5/2022 8:25:35 AM ******/
CREATE DATABASE [CDALDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CDALDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\CDALDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'CDALDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\CDALDB_log.ldf' , SIZE = 63424KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CDALDB] SET COMPATIBILITY_LEVEL = 130
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CDALDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CDALDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CDALDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CDALDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CDALDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CDALDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [CDALDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CDALDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CDALDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CDALDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CDALDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CDALDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CDALDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CDALDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CDALDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CDALDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CDALDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CDALDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CDALDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CDALDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CDALDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CDALDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CDALDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CDALDB] SET RECOVERY FULL 
GO
ALTER DATABASE [CDALDB] SET  MULTI_USER 
GO
ALTER DATABASE [CDALDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CDALDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CDALDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CDALDB] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [CDALDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CDALDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CDALDB', N'ON'
GO
ALTER DATABASE [CDALDB] SET QUERY_STORE = OFF
GO
USE [CDALDB]
GO
/****** Object:  UserDefinedTableType [dbo].[AutoTableDtTbl]    Script Date: 9/5/2022 8:25:35 AM ******/
CREATE TYPE [dbo].[AutoTableDtTbl] AS TABLE(
	[Id] [int] NULL,
	[Name] [nvarchar](50) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[ManualTableDtTbl]    Script Date: 9/5/2022 8:25:35 AM ******/
CREATE TYPE [dbo].[ManualTableDtTbl] AS TABLE(
	[Id] [int] NULL,
	[Name] [nvarchar](50) NULL,
	[TouchedWhen] [datetimeoffset](3) NULL
)
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AutoTable]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AutoTable](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nchar](10) NOT NULL,
	[C] [char](1) NULL,
 CONSTRAINT [PK_AutoTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChildTable]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChildTable](
	[ChildID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[ChildName] [nchar](10) NOT NULL,
 CONSTRAINT [PK_ChildTable] PRIMARY KEY CLUSTERED 
(
	[ChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GrandChild]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GrandChild](
	[GrandchildID] [int] IDENTITY(1,1) NOT NULL,
	[GrandChildName] [nvarchar](50) NOT NULL,
	[ChildID] [int] NOT NULL,
 CONSTRAINT [PK_GrandChild] PRIMARY KEY CLUSTERED 
(
	[GrandchildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ManualChildTable]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManualChildTable](
	[ChildID] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
	[ChildName] [nvarchar](50) NOT NULL,
	[TouchedWhen] [datetimeoffset](3) NOT NULL,
 CONSTRAINT [PK_ManualChildTable] PRIMARY KEY CLUSTERED 
(
	[ChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [IX_ManualChildTable] UNIQUE NONCLUSTERED 
(
	[ChildName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ManualTable]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManualTable](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[TouchedWhen] [datetimeoffset](3) NOT NULL,
 CONSTRAINT [PK_ManualTable] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SecondChildTable]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecondChildTable](
	[SecondChildID] [int] IDENTITY(1,1) NOT NULL,
	[ParentID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SecondChildTable] PRIMARY KEY CLUSTERED 
(
	[SecondChildID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SXARCMWithSysGeneratedColumns]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SXARCMWithSysGeneratedColumns](
	[Build] [int] NOT NULL,
	[TouchedBy] [varchar](50) NOT NULL,
	[TouchedWhenUTC] [datetimeoffset](3) NOT NULL,
	[CreatedBy] [varchar](50) NOT NULL,
	[CreatedWhenUTC] [datetimeoffset](3) NOT NULL,
	[MSRowVersion] [timestamp] NOT NULL,
	[ID] [bigint] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Amount] [int] NOT NULL,
 CONSTRAINT [PK_SXARCMWithSysGeneratedColumns] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TableWithMSRowVersion]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TableWithMSRowVersion](
	[Id] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[MSRowVersion] [timestamp] NOT NULL,
 CONSTRAINT [PK_TableWithMSRowVersion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ManualChildTable] ADD  CONSTRAINT [DF_ManualChildTable_TouchedWhen]  DEFAULT (sysdatetimeoffset()) FOR [TouchedWhen]
GO
ALTER TABLE [dbo].[ManualTable] ADD  CONSTRAINT [DF_ManualTable_TouchedWhen]  DEFAULT (sysdatetimeoffset()) FOR [TouchedWhen]
GO
ALTER TABLE [dbo].[ChildTable]  WITH CHECK ADD  CONSTRAINT [FK_ChildTable_AutoTable] FOREIGN KEY([ParentID])
REFERENCES [dbo].[AutoTable] ([Id])
GO
ALTER TABLE [dbo].[ChildTable] CHECK CONSTRAINT [FK_ChildTable_AutoTable]
GO
ALTER TABLE [dbo].[GrandChild]  WITH CHECK ADD  CONSTRAINT [FK_GrandChild_ChildTable] FOREIGN KEY([ChildID])
REFERENCES [dbo].[ChildTable] ([ChildID])
GO
ALTER TABLE [dbo].[GrandChild] CHECK CONSTRAINT [FK_GrandChild_ChildTable]
GO
ALTER TABLE [dbo].[ManualChildTable]  WITH CHECK ADD  CONSTRAINT [FK_ManualChildTable_ManualTable] FOREIGN KEY([ParentID])
REFERENCES [dbo].[ManualTable] ([Id])
GO
ALTER TABLE [dbo].[ManualChildTable] CHECK CONSTRAINT [FK_ManualChildTable_ManualTable]
GO
ALTER TABLE [dbo].[SecondChildTable]  WITH CHECK ADD  CONSTRAINT [FK_SecondChildTable_AutoTable] FOREIGN KEY([ParentID])
REFERENCES [dbo].[AutoTable] ([Id])
GO
ALTER TABLE [dbo].[SecondChildTable] CHECK CONSTRAINT [FK_SecondChildTable_AutoTable]
GO
/****** Object:  StoredProcedure [dbo].[AutoTableBulkInsPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[AutoTableBulkInsPr]
(
@AutoTableDtTbl AutoTableDtTbl READONLY
)
AS
INSERT INTO AutoTable([Name])
SELECT [Name] FROM @AutoTableDtTbl
GO
/****** Object:  StoredProcedure [dbo].[AutoTableChildTableCollSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[AutoTableChildTableCollSelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [ChildID],
           [ParentID],
           [ChildName]
    FROM   [ChildTable]
    WHERE  [ParentID] = @Id;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[AutoTableDelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[AutoTableDelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    DELETE [AutoTable]
    WHERE  [Id] = @Id;
    SET @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[AutoTableFromSP]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		AutoTableFromSP
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[AutoTableFromSP] 
	-- Add the parameters for the stored procedure here
	@id int 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id, Name, C FROM AutoTable WHERE Id = @id
END
GO
/****** Object:  StoredProcedure [dbo].[AutoTableInsPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[AutoTableInsPr]
@Name NCHAR (10) , @Id INT OUTPUT 

AS
BEGIN
    SET XACT_ABORT ON;
    SET NOCOUNT ON;
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;

    INSERT  INTO [AutoTable] ([Name])
    VALUES                  (@Name);
    SELECT @Id = scope_identity(),
           @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[AutoTableSecondChildTableCollSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[AutoTableSecondChildTableCollSelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [SecondChildID],
           [ParentID],
           [Name]
    FROM   [SecondChildTable]
    WHERE  [ParentID] = @Id;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[AutoTableSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[AutoTableSelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [Id],
           [Name]
    FROM   [AutoTable]
    WHERE  [Id] = @Id;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[AutoTableUpdPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[AutoTableUpdPr]
@Id INT, @Name NCHAR (10)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @ReturnStatus AS INT, @RowCount AS INT;
    SELECT @ReturnStatus = 0,
           @RowCount = 0;
    UPDATE  [AutoTable]
        SET [Name] = @Name
    WHERE   [Id] = @Id;
    SELECT @ReturnStatus = @@ERROR,
           @RowCount = @@ROWCOUNT;
    IF (@RowCount = 0)
        BEGIN
            DECLARE @ObjName AS sysname = OBJECT_NAME(@@PROCID);
            RAISERROR (60001, 16, 1, @ObjName);
            RETURN (60001);
        END
    RETURN @ReturnStatus;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[ChildTableDelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[ChildTableDelPr]
@ChildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    DELETE [ChildTable]
    WHERE  [ChildID] = @ChildID;
    SET @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[ChildTableGrandChildCollSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[ChildTableGrandChildCollSelPr]
@ChildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [GrandchildID],
           [GrandChildName],
           [ChildID]
    FROM   [GrandChild]
    WHERE  [ChildID] = @ChildID;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[ChildTableInsPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[ChildTableInsPr]
@ParentID INT, @ChildName NCHAR (10), @ChildID INT OUTPUT
AS
BEGIN
    SET XACT_ABORT ON;
    SET NOCOUNT ON;
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    INSERT  INTO [ChildTable] ([ParentID], [ChildName])
    VALUES                   (@ParentID, @ChildName);
    SELECT @ChildID = scope_identity(),
           @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[ChildTableSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[ChildTableSelPr]
@ChildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [ChildID],
           [ParentID],
           [ChildName]
    FROM   [ChildTable]
    WHERE  [ChildID] = @ChildID;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[ChildTableUpdPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[ChildTableUpdPr]
@ChildID INT, @ParentID INT, @ChildName NCHAR (10)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @ReturnStatus AS INT, @RowCount AS INT;
    SELECT @ReturnStatus = 0,
           @RowCount = 0;
    UPDATE  [ChildTable]
        SET [ParentID]  = @ParentID,
            [ChildName] = @ChildName
    WHERE   [ChildID] = @ChildID;
    SELECT @ReturnStatus = @@ERROR,
           @RowCount = @@ROWCOUNT;
    IF (@RowCount = 0)
        BEGIN
            DECLARE @ObjName AS sysname = OBJECT_NAME(@@PROCID);
            RAISERROR (60001, 16, 1, @ObjName);
            RETURN (60001);
        END
    RETURN @ReturnStatus;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/


GO
/****** Object:  StoredProcedure [dbo].[DeleteAllSampleData]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteAllSampleData]
	
AS
	Delete from GrandChild
	Delete from ChildTable
	Delete from SecondChildTable
	Delete from AutoTable
	Delete from ManualChildTable
	Delete from ManualTable
	

GO
/****** Object:  StoredProcedure [dbo].[ExecuteMultipleResultSetSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[ExecuteMultipleResultSetSelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
	SELECT [Id],
           [Name]
    FROM   [AutoTable]
    WHERE  [Id] = @Id;

    SELECT [ChildID],
           [ParentID],
           [ChildName]
    FROM   [ChildTable]
    WHERE  [ParentID] = @Id;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/



GO
/****** Object:  StoredProcedure [dbo].[GetAutoTableSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		AutoTableFromSP
-- Create date: 
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GetAutoTableSelPr]
	-- Add the parameters for the stored procedure here
	@id int 
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id, Name, C FROM AutoTable WHERE Id > @id
END
GO
/****** Object:  StoredProcedure [dbo].[GetAutoTableWithChildTableSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--sp_helptext 'GetAutoTableWithChildTableSelPr'

-- =============================================  
-- Author:  AutoTableFromSP  
-- Create date:   
-- Description:   
-- =============================================  
CREATE PROCEDURE [dbo].[GetAutoTableWithChildTableSelPr]  
 -- Add the parameters for the stored procedure here  
 @id int   
   
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
  IF (@id = 1)
  BEGIN
		SELECT Id, Name, C FROM AutoTable WHERE Id > 1
 END
 ELSE
 BEGIN
	SELECT  Name FROM AutoTable WHERE Id > 1
 END
 IF (@id = 1)
 BEGIN
	SELECT ChildID, ParentID, ChildName FROM ChildTable WHERE ChildID > 1
 END
 ELSE
 BEGIN
	SELECT ChildName,ParentID FROM ChildTable WHERE ChildID > 1
 END
 --SELECT Id, Name, C FROM AutoTable WHERE Id > @id  
 --SELECT ChildID, ParentID, ChildName FROM ChildTable WHERE ChildID > @id  
END  
GO
/****** Object:  StoredProcedure [dbo].[GrandChildDelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[GrandChildDelPr]
@GrandchildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    DELETE [GrandChild]
    WHERE  [GrandchildID] = @GrandchildID;
    SET @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[GrandChildInsPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[GrandChildInsPr]
@GrandChildName NVARCHAR (50), @ChildID INT, @GrandchildID INT OUTPUT
AS
BEGIN
    SET XACT_ABORT ON;
    SET NOCOUNT ON;
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    INSERT  INTO [GrandChild] ([GrandChildName], [ChildID])
    VALUES                   (@GrandChildName, @ChildID);
    SELECT @GrandchildID = scope_identity(),
           @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[GrandChildSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[GrandChildSelPr]
@GrandchildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [GrandchildID],
           [GrandChildName],
           [ChildID]
    FROM   [GrandChild]
    WHERE  [GrandchildID] = @GrandchildID;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[GrandChildUpdPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[GrandChildUpdPr]
@GrandchildID INT, @GrandChildName NVARCHAR (50), @ChildID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @ReturnStatus AS INT, @RowCount AS INT;
    SELECT @ReturnStatus = 0,
           @RowCount = 0;
    UPDATE  [GrandChild]
        SET [GrandChildName] = @GrandChildName,
            [ChildID]        = @ChildID
    WHERE   [GrandchildID] = @GrandchildID;
    SELECT @ReturnStatus = @@ERROR,
           @RowCount = @@ROWCOUNT;
    IF (@RowCount = 0)
        BEGIN
            DECLARE @ObjName AS sysname = OBJECT_NAME(@@PROCID);
            RAISERROR (60001, 16, 1, @ObjName);
            RETURN (60001);
        END
    RETURN @ReturnStatus;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[ManualChildTableDelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* PROPRIETARY NOTICE
¬© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
*/
CREATE PROCEDURE [dbo].[ManualChildTableDelPr]
@ChildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    DELETE [ManualChildTable]
    WHERE  [ChildID] = @ChildID;
    SET @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
Subject to full notice set forth herein.
*/
GO
/****** Object:  StoredProcedure [dbo].[ManualChildTableInsPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ManualChildTableInsPr]
@ChildID INT, @ParentID INT, @ChildName NVARCHAR (50), @TouchedWhen DATETIMEOFFSET (3)
AS
BEGIN
    SET XACT_ABORT ON;
    SET NOCOUNT ON;
	--WAITFOR DELAY '00:01:16'
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    SELECT @TouchedWhen = ISNULL(@TouchedWhen, GETDATE());
    INSERT  INTO [ManualChildTable] ([ChildID], [ParentID], [ChildName], [TouchedWhen])
    VALUES                         (@ChildID, @ParentID, @ChildName, @TouchedWhen);
    SELECT @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END
GO
/****** Object:  StoredProcedure [dbo].[ManualChildTableSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* PROPRIETARY NOTICE
¬© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
*/
CREATE PROCEDURE [dbo].[ManualChildTableSelPr]
@ChildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [ChildID],
           [ParentID],
           [ChildName],
           [TouchedWhen]
    FROM   [ManualChildTable]
    WHERE  [ChildID] = @ChildID;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
Subject to full notice set forth herein.
*/
GO
/****** Object:  StoredProcedure [dbo].[ManualChildTableUpdPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* PROPRIETARY NOTICE
¬© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
*/
CREATE PROCEDURE [dbo].[ManualChildTableUpdPr]
@ChildID INT, @ParentID INT, @ChildName NVARCHAR (50), @TouchedWhen DATETIMEOFFSET (3)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @ReturnStatus AS INT, @RowCount AS INT;
    SELECT @ReturnStatus = 0,
           @RowCount = 0;
    SELECT @TouchedWhen = ISNULL(@TouchedWhen, GETDATE());
    UPDATE  [ManualChildTable]
        SET [ParentID]    = @ParentID,
            [ChildName]   = @ChildName,
            [TouchedWhen] = @TouchedWhen
    WHERE   [ChildID] = @ChildID;
    SELECT @ReturnStatus = @@ERROR,
           @RowCount = @@ROWCOUNT;
    IF (@RowCount = 0)
        BEGIN
            DECLARE @ObjName AS sysname = OBJECT_NAME(@@PROCID);
            RAISERROR (60001, 16, 1, @ObjName);
            RETURN (60001);
        END
    RETURN @ReturnStatus;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
Subject to full notice set forth herein.
*/
GO
/****** Object:  StoredProcedure [dbo].[ManualTableDelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* PROPRIETARY NOTICE
¬© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
*/
CREATE PROCEDURE [dbo].[ManualTableDelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    DELETE [ManualTable]
    WHERE  [Id] = @Id;
    SET @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
Subject to full notice set forth herein.
*/
GO
/****** Object:  StoredProcedure [dbo].[ManualTableInsPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
¬© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
*/
CREATE PROCEDURE [dbo].[ManualTableInsPr]
@Id INT = NULL, @Name NVARCHAR (50) = NULL, @TouchedWhen DATETIMEOFFSET(3) = NULL, @ManualBulkInsertUDT ManualTableDtTbl READONLY 
AS
BEGIN
    SET XACT_ABORT ON;
    SET NOCOUNT ON;
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    SELECT @TouchedWhen = ISNULL(@TouchedWhen, GETDATE());
	IF EXISTS(SELECT 1 FROM @ManualBulkInsertUDT)
	BEGIN
		INSERT  INTO [ManualTable] ([Id], [Name], [TouchedWhen])
		SELECT [Id], [Name], [TouchedWhen]
		FROM	@ManualBulkInsertUDT
	END
	ELSE
	BEGIN
    INSERT  INTO [ManualTable] ([Id], [Name], [TouchedWhen])
    VALUES                    (@Id, @Name, @TouchedWhen);
	END
    SELECT @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END
GO
/****** Object:  StoredProcedure [dbo].[ManualTableManualChildTableCollSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* PROPRIETARY NOTICE
¬© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
*/
CREATE PROCEDURE [dbo].[ManualTableManualChildTableCollSelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [ChildID],
           [ParentID],
           [ChildName],
           [TouchedWhen]
    FROM   [ManualChildTable]
    WHERE  [ParentID] = @Id;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
Subject to full notice set forth herein.
*/
GO
/****** Object:  StoredProcedure [dbo].[ManualTableSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* PROPRIETARY NOTICE
¬© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
*/
CREATE PROCEDURE [dbo].[ManualTableSelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [Id],
           [Name],
           [TouchedWhen]
    FROM   [ManualTable]
    WHERE  [Id] = @Id;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
Subject to full notice set forth herein.
*/
GO
/****** Object:  StoredProcedure [dbo].[ManualTableUpdPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* PROPRIETARY NOTICE
¬© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
*/
CREATE PROCEDURE [dbo].[ManualTableUpdPr]
@Id INT, @Name NVARCHAR (50), @TouchedWhen DATETIMEOFFSET (3)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @ReturnStatus AS INT, @RowCount AS INT;
    SELECT @ReturnStatus = 0,
           @RowCount = 0;
    SELECT @TouchedWhen = ISNULL(@TouchedWhen, GETDATE());
    UPDATE  [ManualTable]
        SET [Name]        = @Name,
            [TouchedWhen] = @TouchedWhen
    WHERE   [Id] = @Id;
    SELECT @ReturnStatus = @@ERROR,
           @RowCount = @@ROWCOUNT;
    IF (@RowCount = 0)
        BEGIN
            DECLARE @ObjName AS sysname = OBJECT_NAME(@@PROCID);
            RAISERROR (60001, 16, 1, @ObjName);
            RETURN (60001);
        END
    RETURN @ReturnStatus;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ‚ÄúCommercial Computer Software.‚Äù
Subject to full notice set forth herein.
*/
GO
/****** Object:  StoredProcedure [dbo].[MultiOutputSample]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MultiOutputSample]
	@p1 int = 0, 
	@p2 int  OUTPUT,
	@p3 int  OUTPUT
AS
	SET NOCOUNT ON
	SELECT @p2 = @p1 + 1
	SELECT @p3 = @p1 + 2
	
GO
/****** Object:  StoredProcedure [dbo].[MultiResultSetTestSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MultiResultSetTestSelPr]
	-- Add the parameters for the stored procedure here
	@p1 int = 0

AS
BEGIN
	SET NOCOUNT ON;

	SELECT * FROM AutoTable
	SELECT * FROM ChildTable

    
	
END
GO
/****** Object:  StoredProcedure [dbo].[SecondChildTableDelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[SecondChildTableDelPr]
@SecondChildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    DELETE [SecondChildTable]
    WHERE  [SecondChildID] = @SecondChildID;
    SET @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[SecondChildTableInsPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[SecondChildTableInsPr]
@ParentID INT, @Name NVARCHAR (50), @SecondChildID INT OUTPUT
AS
BEGIN
    SET XACT_ABORT ON;
    SET NOCOUNT ON;
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    INSERT  INTO [SecondChildTable] ([ParentID], [Name])
    VALUES                         (@ParentID, @Name);
    SELECT @SecondChildID = scope_identity(),
           @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[SecondChildTableSelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[SecondChildTableSelPr]
@SecondChildID INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    SELECT [SecondChildID],
           [ParentID],
           [Name]
    FROM   [SecondChildTable]
    WHERE  [SecondChildID] = @SecondChildID;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[SecondChildTableUpdPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[SecondChildTableUpdPr]
@SecondChildID INT, @ParentID INT, @Name NVARCHAR (50)
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;
    DECLARE @ReturnStatus AS INT, @RowCount AS INT;
    SELECT @ReturnStatus = 0,
           @RowCount = 0;
    UPDATE  [SecondChildTable]
        SET [ParentID] = @ParentID,
            [Name]     = @Name
    WHERE   [SecondChildID] = @SecondChildID;
    SELECT @ReturnStatus = @@ERROR,
           @RowCount = @@ROWCOUNT;
    IF (@RowCount = 0)
        BEGIN
            DECLARE @ObjName AS sysname = OBJECT_NAME(@@PROCID);
            RAISERROR (60001, 16, 1, @ObjName);
            RETURN (60001);
        END
    RETURN @ReturnStatus;
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/

GO
/****** Object:  StoredProcedure [dbo].[SingleResultSetTest]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SingleResultSetTest]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT * FROM AutoTable
END
GO
/****** Object:  StoredProcedure [dbo].[TableWithMSRowVersionDelPr]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* PROPRIETARY NOTICE
© [2011 - 2012] Allscripts Healthcare Solutions, Inc. and/or its affiliates. All Rights Reserved.
This software has been provided pursuant to a License Agreement, with Allscripts Healthcare Solutions, Inc.
and/or its affiliates, containing restrictions on its use.
This software contains valuable trade secrets and proprietary information of Allscripts Healthcare Solutions, Inc.
and/or its affiliates and is protected by  trade secret and copyright law.
This software may not be copied or distributed in any form or medium, disclosed to any third parties,
or used in any manner not provided for in said License Agreement except with prior written authorization
from Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
*/
CREATE PROCEDURE [dbo].[TableWithMSRowVersionDelPr]
@Id INT
AS
SET XACT_ABORT ON;
SET NOCOUNT ON;
BEGIN
    DECLARE @ReturnStatus AS INT;
    SELECT @ReturnStatus = 0;
    DELETE [TableWithMSRowVersion]
    WHERE  [Id] = @Id;
    SET @ReturnStatus = @@error;
    IF @ReturnStatus <> 0
        BEGIN
            RETURN @ReturnStatus;
        END
END



/*PROPRIETARY NOTICE
Confidential and proprietary information of Allscripts Healthcare Solutions, Inc. and/or its affiliates.
Authorized users only. Notice to U.S. Government Users: This software is ìCommercial Computer Software.î
Subject to full notice set forth herein.
*/
GO
/****** Object:  StoredProcedure [dbo].[Test_ResultSetWithOutput]    Script Date: 9/5/2022 8:25:35 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Test_ResultSetWithOutput]
	@p1 int = 0, 
	@p2 int  OUTPUT,
	@p3 int  OUTPUT
AS
	SELECT @p2 = @p1 + 1
	SELECT @p3 = @p1 + 2
	Select TOP(2) * from AutoTable
	Select TOP(2) * from ChildTable
GO
USE [master]
GO
ALTER DATABASE [CDALDB] SET  READ_WRITE 
GO
