SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
	
CREATE VIEW [dbo].[vGeoRegion] AS
	SELECT gc.ID'GeoCountryID',CountryCode,CountryName,gr.ID'GeoRegionID',RegionCode,RegionName
	FROM GeoCountry gc
	JOIN GeoRegion gr ON gr.GeoCountryID=gc.ID
GO
