CREATE TABLE [dbo].[GeoStateCountyCity]
(
[StateFips] [tinyint] NULL,
[CountyFips] [smallint] NULL,
[CityFips] [int] NULL,
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountyName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
