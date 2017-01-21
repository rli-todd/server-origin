SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NLog] (
		[LogDate]      [datetime] NOT NULL,
		[ID]           [int] IDENTITY(1, 1) NOT NULL,
		[Level]        [nvarchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Logger]       [nvarchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Message]      [nvarchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ClientIP]     [int] NULL,
		[UserID]       [int] NULL,
		[ApiLogID]     [int] NULL,
		[Server]       [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[NLog]
	ADD
	CONSTRAINT [PK_NLog]
	PRIMARY KEY
	CLUSTERED
	([LogDate], [ID])
	ON [PRIMARY]
GO
GRANT INSERT
	ON [dbo].[NLog]
	TO [NLogWriter]
GO
ALTER TABLE [dbo].[NLog] SET (LOCK_ESCALATION = TABLE)
GO
