SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConversionPath] (
		[SiteID]                 [tinyint] NOT NULL,
		[ID]                     [int] IDENTITY(1, 1) NOT NULL,
		[ConversionPathCode]     [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConversionPath]
	ADD
	CONSTRAINT [PK_ConversionPath]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ConversionPath]
	ON [dbo].[ConversionPath] ([SiteID], [ConversionPathCode])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[ConversionPath] SET (LOCK_ESCALATION = TABLE)
GO
