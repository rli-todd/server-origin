CREATE TABLE [dbo].[GeoRegion]
(
[ID] [smallint] NOT NULL IDENTITY(1, 1),
[GeoCountryID] [tinyint] NULL,
[RegionCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[RegionName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateFips] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeoRegion] ADD CONSTRAINT [PK_GeoRegion] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_GeoRegion] ON [dbo].[GeoRegion] ([GeoCountryID], [RegionCode]) INCLUDE ([ID]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeoRegion] ADD CONSTRAINT [FK_GeoRegion_GeoCountry] FOREIGN KEY ([GeoCountryID]) REFERENCES [dbo].[GeoCountry] ([ID])
GO
