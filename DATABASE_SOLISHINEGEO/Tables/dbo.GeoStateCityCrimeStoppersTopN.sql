CREATE TABLE [dbo].[GeoStateCityCrimeStoppersTopN]
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
[CrimeStopperName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Phone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_GeoStateCityCrimeStoppersTopN] ON [dbo].[GeoStateCityCrimeStoppersTopN] ([StateFips], [CityFips], [ToStateFips], [ToCityFips], [RowNum]) ON [PRIMARY]
GO
