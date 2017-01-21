SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cache] (
		[HashKey]         [binary](20) NOT NULL,
		[Value]           [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[DateCreated]     [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cache]
	ADD
	CONSTRAINT [PK_Cache]
	PRIMARY KEY
	CLUSTERED
	([HashKey])
	ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Cache_CacheDate]
	ON [dbo].[Cache] ([DateCreated])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cache] SET (LOCK_ESCALATION = TABLE)
GO
