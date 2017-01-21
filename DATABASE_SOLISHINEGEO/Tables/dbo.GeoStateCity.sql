CREATE TABLE [dbo].[GeoStateCity]
(
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateNormalized] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityNormalized] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
CREATE UNIQUE CLUSTERED INDEX [CIX_GeoStateCity] ON [dbo].[GeoStateCity] ([StateFips], [CityFips]) ON [PRIMARY]

GO
