SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Category] (
		[SiteID]                [tinyint] NOT NULL,
		[ID]                    [int] IDENTITY(1, 1) NOT NULL,
		[CategoryName]          [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[IsActive]              [bit] NOT NULL,
		[CategoryCode]          [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[RequireQueryID]        [bit] NULL,
		[RequireState]          [bit] NULL,
		[RequireProfileID]      [bit] NULL,
		[ReportTypeCode]        [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CategoryType]          [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ProductExternalID]     [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Category]
	ADD
	CONSTRAINT [PK_Category]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Category]
	ADD
	CONSTRAINT [DF_Category_IsActive]
	DEFAULT ((0)) FOR [IsActive]
GO
CREATE NONCLUSTERED INDEX [IX_Category_CategoryCode]
	ON [dbo].[Category] ([SiteID], [CategoryCode])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Category] SET (LOCK_ESCALATION = TABLE)
GO
