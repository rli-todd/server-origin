SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[xvGeoLocation] WITH SCHEMABINDING AS
	SELECT 
		CountryName,CountryCode,
    RegionName,RegionCode,
    CityName,PostalCode,AreaCode,DmaCode,
    TimeZone,DaylightSavings,Latitude,Longitude,
    gl.ID'GeoLocationID',
    GeoCityID,GeoRegionID,GeoCountryID,
    CONVERT(TINYINT,ISNULL(gr.StateFips,ISNULL(gc.StateFips,ISNULL(gl.StateFips,0))))'StateFips',
    CONVERT(INT,ISNULL(gc.CityFips,ISNULL(gl.CityFips,0)))'CityFips'
		FROM dbo.GeoLocation gl
		JOIN dbo.GeoCity gc ON gc.ID=gl.GeoCityID
		JOIN dbo.GeoRegion gr ON gr.ID=gc.GeoRegionID
		JOIN dbo.GeoCountry c ON c.ID=gr.GeoCountryID	


GO
CREATE UNIQUE CLUSTERED INDEX [IX_xvGeoLocation] ON [dbo].[xvGeoLocation] ([GeoLocationID]) ON [PRIMARY]
GO
