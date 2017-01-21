CREATE TABLE [dbo].[GeoStateCityLibraryTopN]
(
[StateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToStateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToCityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LibraryName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Zip] [float] NULL,
[Phone] [float] NULL,
[Miles] [float] NULL,
[RowNum] [bigint] NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_GeoStateCityLibraryTopN] ON [dbo].[GeoStateCityLibraryTopN] ([StateFips], [CityFips], [ToStateFips], [ToCityFips], [RowNum]) ON [PRIMARY]
GO
