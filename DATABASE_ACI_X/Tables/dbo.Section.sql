SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Section] (
		[ID]              [int] IDENTITY(1, 1) NOT NULL,
		[BlockID]         [int] NOT NULL,
		[SectionName]     [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[SectionType]     [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[IsEnabled]       [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Section]
	ADD
	CONSTRAINT [PK_ContentBlock]
	PRIMARY KEY
	CLUSTERED
	([ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Section]
	WITH CHECK
	ADD CONSTRAINT [FK_Section_Block]
	FOREIGN KEY ([BlockID]) REFERENCES [dbo].[Block] ([ID])
ALTER TABLE [dbo].[Section]
	CHECK CONSTRAINT [FK_Section_Block]

GO
ALTER TABLE [dbo].[Section] SET (LOCK_ESCALATION = TABLE)
GO
