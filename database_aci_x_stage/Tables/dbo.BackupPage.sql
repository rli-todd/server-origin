SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BackupPage] (
		[BackupDate]      [datetime] NOT NULL,
		[ID]              [int] NOT NULL,
		[PageCode]        [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Description]     [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[BackupPage] SET (LOCK_ESCALATION = TABLE)
GO
