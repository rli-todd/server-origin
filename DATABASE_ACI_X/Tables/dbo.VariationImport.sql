SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VariationImport] (
		[done]                  [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[PageCode]              [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BlockNum]              [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BlockName]             [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BlockType]             [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[SectionNum]            [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[SectionName]           [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[SectionType]           [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Description]           [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[MultirowPrefix]        [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[MultirowSuffix]        [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[MultirowDelimiter]     [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ViewName]              [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ViewFieldNames]        [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[HeaderTemplate]        [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[HeaderDefault]         [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BodyTemplate]          [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BodyDefault]           [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Valid]                 [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[VariationImport] SET (LOCK_ESCALATION = TABLE)
GO
