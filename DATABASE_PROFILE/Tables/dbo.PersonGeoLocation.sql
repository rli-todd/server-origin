CREATE TABLE [dbo].[PersonGeoLocation]
(
[PersonID] [int] NOT NULL,
[GeoLocationID] [int] NOT NULL,
[CityFips] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PersonGeoLocation] ADD CONSTRAINT [PK_PersonGeoLocation] PRIMARY KEY CLUSTERED  ([PersonID], [GeoLocationID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PersonGeoLocation] ON [dbo].[PersonGeoLocation] ([GeoLocationID], [PersonID]) ON [PRIMARY]
GO
