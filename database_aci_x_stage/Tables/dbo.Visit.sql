SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Visit] (
		[SiteID]                  [tinyint] NOT NULL,
		[ID]                      [int] NOT NULL,
		[WebServerID]             [tinyint] NOT NULL,
		[ApiServerID]             [tinyint] NOT NULL,
		[DateCreated]             [datetime] NOT NULL,
		[DateModified]            [datetime] NOT NULL,
		[VisitGuid]               [uniqueidentifier] NOT NULL,
		[UserAgentID]             [int] NOT NULL,
		[LandingUrlID]            [int] NOT NULL,
		[RefererUrlID]            [int] NOT NULL,
		[IpAddress]               [int] NOT NULL,
		[UseCount]                [int] NOT NULL,
		[UserID]                  [int] NULL,
		[RobotID]                 [int] NULL,
		[IsBlocked]               [bit] NOT NULL,
		[GeoLocationID]           [int] NULL,
		[ConversionPathID]        [int] NULL,
		[IwsReferCode]            [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[IwsUserToken]            [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[IwsUserTokenExpiry]      [datetime] NULL,
		[IwsUserID]               [int] NULL,
		[StorefrontUserToken]     [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[UtcOffsetMins]           [smallint] NULL,
		[AcceptLanguage]          [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[IsRecorded]              [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Visit]
	ADD
	CONSTRAINT [PK_Visit]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Visit]
	ADD
	CONSTRAINT [DF_IsRecorded]
	DEFAULT ((0)) FOR [IsRecorded]
GO
ALTER TABLE [dbo].[Visit]
	ADD
	CONSTRAINT [DF_Visit_UserAccessToken]
	DEFAULT (newid()) FOR [VisitGuid]
GO
CREATE NONCLUSTERED INDEX [IX_Visit]
	ON [dbo].[Visit] ([SiteID], [UserAgentID], [IpAddress], [DateCreated])
	ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Visit_ID]
	ON [dbo].[Visit] ([ID])
	ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Visit_IsRecorded]
	ON [dbo].[Visit] ([IsRecorded], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Visit] SET (LOCK_ESCALATION = TABLE)
GO
