SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Site] (
		[ID]                    [tinyint] IDENTITY(1, 1) NOT NULL,
		[SiteName]              [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[ShortName]             [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[SeoReferCode]          [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[SemReferCode]          [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[AlternateSiteName]     [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BaseUrl]               [varchar](100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Site]
	ADD
	CONSTRAINT [PK_Site]
	PRIMARY KEY
	CLUSTERED
	([ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Site] SET (LOCK_ESCALATION = TABLE)
GO
