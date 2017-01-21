SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[BackupPageBlock] (
		[BackupDate]     [datetime] NOT NULL,
		[PageID]         [int] NOT NULL,
		[BlockID]        [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BackupPageBlock] SET (LOCK_ESCALATION = TABLE)
GO
