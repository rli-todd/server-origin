SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users] (
		[SiteID]                       [tinyint] NOT NULL,
		[ID]                           [int] IDENTITY(1, 1) NOT NULL,
		[UserGuid]                     [uniqueidentifier] NOT NULL,
		[FirstVisitID]                 [int] NOT NULL,
		[LastVisitID]                  [int] NOT NULL,
		[ExternalID]                   [int] NULL,
		[FirstName]                    [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[LastName]                     [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[EmailAddress]                 [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[HasAcceptedUserAgreement]     [bit] NOT NULL,
		[HasValidPaymentMethod]        [bit] NOT NULL,
		[CardCVV]                      [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardHash]                     [binary](20) NULL,
		[DateLastAuthenticated]        [datetime] NULL,
		[IsBackofficeWriter]           [bit] NOT NULL,
		[IsBackofficeReader]           [bit] NOT NULL,
		[CardholderName]               [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardLast4]                    [char](4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardExpiry]                   [char](4) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardAddress]                  [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardCity]                     [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardState]                    [char](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardCountry]                  [char](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardZip]                      [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[CardLastModified]             [datetime] NULL,
		[DateCreated]                  [datetime] NOT NULL,
		[MiddleName]                   [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users]
	ADD
	CONSTRAINT [PK_Users_1]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users]
	ADD
	CONSTRAINT [DF_Users_DateCreated]
	DEFAULT (getdate()) FOR [DateCreated]
GO
ALTER TABLE [dbo].[Users]
	ADD
	CONSTRAINT [DF_Users_HasValidPaymentMethod]
	DEFAULT ((0)) FOR [HasValidPaymentMethod]
GO
ALTER TABLE [dbo].[Users]
	ADD
	CONSTRAINT [DF_Users_IsBackofficeReader]
	DEFAULT ((0)) FOR [IsBackofficeReader]
GO
ALTER TABLE [dbo].[Users]
	ADD
	CONSTRAINT [DF_Users_IsBackofficeWriter]
	DEFAULT ((0)) FOR [IsBackofficeWriter]
GO
ALTER TABLE [dbo].[Users]
	ADD
	CONSTRAINT [DF_Users_UserGuid]
	DEFAULT (newid()) FOR [UserGuid]
GO
CREATE NONCLUSTERED INDEX [IX_Users_UserGuid]
	ON [dbo].[Users] ([UserGuid])
	INCLUDE ([ID], [ExternalID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Users] SET (LOCK_ESCALATION = TABLE)
GO
