SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApiLogTemplate] (
		[ID]              [smallint] IDENTITY(1, 1) NOT NULL,
		[Template]        [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[DateCreated]     [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiLogTemplate]
	ADD
	CONSTRAINT [PK_ApiLogTemplate]
	PRIMARY KEY
	CLUSTERED
	([ID])
	ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_ApiLogTemplate]
	ON [dbo].[ApiLogTemplate] ([Template])
	INCLUDE ([ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiLogTemplate] SET (LOCK_ESCALATION = TABLE)
GO
