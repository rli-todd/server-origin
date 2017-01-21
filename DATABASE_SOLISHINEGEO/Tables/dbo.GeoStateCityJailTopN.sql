CREATE TABLE [dbo].[GeoStateCityJailTopN]
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
[JailName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Zip] [float] NULL,
[ConstructionYear] [float] NULL,
[AvgPopulation] [float] NULL,
[Population] [float] NULL,
[AdultMaleInmates] [float] NULL,
[AdultFemaleInmates] [float] NULL,
[JuvenileMaleInmates] [float] NULL,
[JuvenileFemaleInmates] [float] NULL,
[FullTimeStaff] [float] NULL,
[PartTimeStaff] [float] NULL,
[RatedCapacity] [float] NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_GeoStateCityJailTopN] ON [dbo].[GeoStateCityJailTopN] ([StateFips], [CityFips], [ToStateFips], [ToCityFips], [RowNum]) ON [PRIMARY]
GO
