
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO



CREATE VIEW [dbo].[vGeoLocationSelector]   AS
SELECT g.ID'GeoLocationID',
	CASE 
		WHEN RegionCode = '*'
			THEN RTRIM(CountryName)
		WHEN CityName='Any' 
			THEN RegionName + ' (State)'
		WHEN RegionName IS NOT NULL AND PostalCode='City:'+CONVERT(VARCHAR,GeoCityID)
			THEN CityName+ ', ' + RegionName
		WHEN RegionName IS NULL AND PostalCode='City:'+CONVERT(VARCHAR,GeoCityID)
			THEN CityName + ', ' + CountryName
		ELSE PostalCode+' ('+CityName+', '+RegionName+')'
	END'Location',CountryCode,RegionCode,PostalCode
	FROM dbo.GeoLocation g
	JOIN dbo.GeoCity c ON c.ID=g.GeoCityID
	JOIN dbo.GeoRegion r ON r.ID=c.GeoRegionID
	JOIN dbo.GeoCountry gc ON gc.ID=r.GeoCountryID






GO
