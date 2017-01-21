CREATE TABLE [dbo].[GeoStateCityCrimeStoppers]
(
[CrimeStopperName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Latitude] [float] NULL,
[Longitude] [float] NULL,
[CountyName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[Phone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
