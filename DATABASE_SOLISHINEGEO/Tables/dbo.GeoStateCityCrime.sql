CREATE TABLE [dbo].[GeoStateCityCrime]
(
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[Year] [float] NULL,
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Agency] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Population] [float] NULL,
[ViolentCrimeRate] [float] NULL,
[Murder] [float] NULL,
[Rape] [float] NULL,
[Robbery] [float] NULL,
[Assault] [float] NULL,
[Property] [float] NULL,
[Burglary] [float] NULL,
[Larceny] [float] NULL,
[Vehicular] [float] NULL
) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX [IX_GeoStateCityCrime] ON [dbo].[GeoStateCityCrime] ([StateFips], [CityFips], [Year]) ON [PRIMARY]
GO
