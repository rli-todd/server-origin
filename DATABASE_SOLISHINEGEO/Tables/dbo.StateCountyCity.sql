CREATE TABLE [dbo].[StateCountyCity]
(
[StateAbbr] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateFips] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityFips] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountyFips] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountyName] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
