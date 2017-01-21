SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Variation] (
		[ID]                    [int] IDENTITY(1, 1) NOT NULL,
		[SectionID]             [int] NOT NULL,
		[IsEnabled]             [bit] NOT NULL,
		[Description]           [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[MultirowPrefix]        [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[MultirowSuffix]        [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[MultirowDelimiter]     [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[HeaderTemplate]        [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[HeaderDefault]         [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BodyTemplate]          [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BodyDefault]           [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ViewName]              [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ViewFieldNames]        [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Variation]
	ADD
	CONSTRAINT [PK_Variation]
	PRIMARY KEY
	NONCLUSTERED
	([ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Variation]
	WITH CHECK
	ADD CONSTRAINT [FK_Variation_Section]
	FOREIGN KEY ([SectionID]) REFERENCES [dbo].[Section] ([ID])
ALTER TABLE [dbo].[Variation]
	CHECK CONSTRAINT [FK_Variation_Section]

GO
CREATE UNIQUE CLUSTERED INDEX [CIX_Variation]
	ON [dbo].[Variation] ([SectionID], [IsEnabled], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Variation] SET (LOCK_ESCALATION = TABLE)
GO
