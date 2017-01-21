CREATE TABLE [dbo].[GeoStateCityFederalAppealsCourtsTopN]
(
[StateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToStateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToCityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Miles] [float] NULL,
[RowNum] [bigint] NULL,
[CourtName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[COurtLevel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Circuit] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MainOffice] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationAddress1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationAddress2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationCity] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationState] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LocationZip] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingAddress1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingAddress2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingAddress3] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingCity] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingState] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MailingZip] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Url] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_GeoStateCityFederalAppealsCourtsTopN] ON [dbo].[GeoStateCityFederalAppealsCourtsTopN] ([StateFips], [CityFips], [ToStateFips], [ToCityFips], [RowNum]) ON [PRIMARY]
GO
