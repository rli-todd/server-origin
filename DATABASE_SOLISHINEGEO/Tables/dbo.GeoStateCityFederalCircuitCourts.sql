CREATE TABLE [dbo].[GeoStateCityFederalCircuitCourts]
(
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[CourtName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtLevel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Circuit] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MainOffice] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationAddress1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationAddress2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationCity] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationState] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationZip] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingAddress1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingAddress2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingCity] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingState] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingZip] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Url] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ECLLink] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
