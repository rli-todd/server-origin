SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApiLog] (
		[SiteID]               [tinyint] NOT NULL,
		[LogDate]              [datetime] NOT NULL,
		[ID]                   [int] IDENTITY(1, 1) NOT NULL,
		[VisitID]              [int] NULL,
		[UserID]               [int] NULL,
		[ClientIP]             [int] NULL,
		[UserIP]               [int] NULL,
		[RequestMethod]        [char](1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[RequestSize]          [int] NULL,
		[RequestBody]          [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ResponseSize]         [int] NULL,
		[ResponseJson]         [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[HttpStatusCode]       [smallint] NULL,
		[DurationMsecs]        [int] NULL,
		[ErrorType]            [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ErrorMessage]         [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ApiLogPathID]         [int] NULL,
		[ApiLogTemplateID]     [smallint] NULL,
		[UserAgentID]          [int] NULL,
		[ServerID]             [smallint] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiLog]
	ADD
	CONSTRAINT [PK_ApiLog]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [LogDate], [ID])
	ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApiLog2014_ClientIP]
	ON [dbo].[ApiLog] ([ClientIP], [LogDate])
	INCLUDE ([UserID])
	ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApiLog2014_Path]
	ON [dbo].[ApiLog] ([ApiLogPathID])
	ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApiLog2014_User]
	ON [dbo].[ApiLog] ([UserID], [LogDate])
	INCLUDE ([ClientIP], [ApiLogPathID])
	ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApiLog2014_UserSession]
	ON [dbo].[ApiLog] ([VisitID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiLog] SET (LOCK_ESCALATION = TABLE)
GO
