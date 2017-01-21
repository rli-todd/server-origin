SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ReportType] (
		[SiteID]                [tinyint] NOT NULL,
		[TypeCode]              [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[Title]                 [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ProfileAttributes]     [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ReportType]
	ADD
	CONSTRAINT [PK_ReportType_1]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [TypeCode])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[ReportType] SET (LOCK_ESCALATION = TABLE)
GO
