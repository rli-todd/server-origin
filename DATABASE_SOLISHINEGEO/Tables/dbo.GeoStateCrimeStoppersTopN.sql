CREATE TABLE [dbo].[GeoStateCrimeStoppersTopN]
(
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RowNum] [bigint] NULL,
[CrimeStopperName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Phone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_GeoStateCrimeStoppersTopN] ON [dbo].[GeoStateCrimeStoppersTopN] ([StateFips], [RowNum]) ON [PRIMARY]
GO
