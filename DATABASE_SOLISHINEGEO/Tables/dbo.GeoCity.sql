CREATE TABLE [dbo].[GeoCity]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GeoRegionID] [smallint] NULL,
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[AvgLatitude] [decimal] (10, 6) NULL,
[AvgLongitude] [decimal] (10, 6) NULL,
[LandArea] [real] NULL,
[WaterArea] [real] NULL,
[TotalArea] [real] NULL,
[LandAreaStateRank] [int] NULL,
[WaterAreaStateRank] [int] NULL,
[TotalAreaStateRank] [int] NULL,
[LandAreaNationalRank] [int] NULL,
[WaterAreaNationalRank] [int] NULL,
[TotalAreaNationalRank] [int] NULL
) ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_GeoCity_Lat_Long] ON [dbo].[GeoCity] ([AvgLatitude], [AvgLongitude]) INCLUDE ([CityFips], [CityName], [StateFips]) ON [PRIMARY]

GO
ALTER TABLE [dbo].[GeoCity] ADD CONSTRAINT [PK_GeoCity] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_GeoCity_Region_CityName] ON [dbo].[GeoCity] ([GeoRegionID], [CityName]) WITH (FILLFACTOR=70) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeoCity] ADD CONSTRAINT [FK_GeoCity_GeoRegion] FOREIGN KEY ([GeoRegionID]) REFERENCES [dbo].[GeoRegion] ([ID])
GO
GRANT SELECT ON  [dbo].[GeoCity] TO [public]
GO
