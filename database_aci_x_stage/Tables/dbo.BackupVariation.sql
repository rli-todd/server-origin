SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BackupVariation] (
		[BackupDate]            [datetime] NOT NULL,
		[ID]                    [int] NOT NULL,
		[SectionID]             [int] NULL,
		[IsEnabled]             [bit] NULL,
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
ALTER TABLE [dbo].[BackupVariation] SET (LOCK_ESCALATION = TABLE)
GO
