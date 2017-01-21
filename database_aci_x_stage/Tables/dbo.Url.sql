SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Url] (
		[SiteID]          [tinyint] NOT NULL,
		[UrlTypeCode]     [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[ID]              [int] IDENTITY(1, 1) NOT NULL,
		[UrlHash]         [binary](20) NULL,
		[IsHttps]         [bit] NULL,
		[DomainID]        [int] NULL,
		[SubdomainID]     [int] NULL,
		[Path]            [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[QueryString]     [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Url]
	ADD
	CONSTRAINT [PK_Url]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	WITH (PAD_INDEX = ON, FILLFACTOR = 80)
	ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Url]
	ON [dbo].[Url] ([SiteID], [UrlTypeCode], [UrlHash])
	INCLUDE ([ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Url] SET (LOCK_ESCALATION = TABLE)
GO
