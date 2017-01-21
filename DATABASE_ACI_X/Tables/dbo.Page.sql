SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Page] (
		[ID]              [int] IDENTITY(1, 1) NOT NULL,
		[PageCode]        [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Description]     [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Page]
	ADD
	CONSTRAINT [PK_Page]
	PRIMARY KEY
	CLUSTERED
	([ID])
	ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Page_PageCode]
	ON [dbo].[Page] ([PageCode])
	INCLUDE ([ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Page] SET (LOCK_ESCALATION = TABLE)
GO