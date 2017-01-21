CREATE TABLE [dbo].[GeoStateCityLibrary]
(
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[LibraryName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Zip] [float] NULL,
[Phone] [float] NULL
) ON [PRIMARY]
GO
