SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApiClient] (
		[SiteID]           [tinyint] NOT NULL,
		[ApiClientID]      [int] NOT NULL,
		[ClientSecret]     [uniqueidentifier] NOT NULL,
		[ClientName]       [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[IsEnabled]        [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiClient]
	ADD
	CONSTRAINT [PK_ApiClient]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ApiClientID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiClient] SET (LOCK_ESCALATION = TABLE)
GO
