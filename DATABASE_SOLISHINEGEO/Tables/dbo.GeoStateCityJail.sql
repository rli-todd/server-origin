CREATE TABLE [dbo].[GeoStateCityJail]
(
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
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
