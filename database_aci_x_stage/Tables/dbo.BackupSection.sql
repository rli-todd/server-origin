SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BackupSection] (
		[BackupDate]      [datetime] NOT NULL,
		[ID]              [int] NOT NULL,
		[BlockID]         [int] NULL,
		[SectionName]     [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[SectionType]     [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[IsEnabled]       [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BackupSection] SET (LOCK_ESCALATION = TABLE)
GO
