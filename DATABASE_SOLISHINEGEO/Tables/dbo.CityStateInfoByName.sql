CREATE TABLE [dbo].[CityStateInfoByName]
(
[CityAndState] [varchar] (54) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateNormalized] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityNormalized] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountyName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[Population] [int] NULL,
[CourtName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtLevel] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Circuit] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MainOffice] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtAddress1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtAddress2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtCity] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtState] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtZip] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtPhone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CourtUrl] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MilesToCourt] [money] NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [CIX_CityStateInfoByName] ON [dbo].[CityStateInfoByName] ([CityAndState], [CountyName]) ON [PRIMARY]
GO
