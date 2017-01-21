SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BackupBlock] (
		[BackupDate]      [datetime] NOT NULL,
		[ID]              [int] NOT NULL,
		[BlockName]       [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[BlockType]       [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[IsEnabled]       [bit] NULL,
		[KeyTemplate]     [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BackupBlock] SET (LOCK_ESCALATION = TABLE)
GO
