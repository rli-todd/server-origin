SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoLocationCreateWildcardLevels] AS
	INSERT INTO GeoRegion(GeoCountryID,RegionCode,RegionName)
		SELECT DISTINCT gc.ID,'*','Any'
			FROM GeoCountry gc
			WHERE NOT EXISTS (
				SELECT * FROM GeoRegion
					WHERE GeoCountryID=gc.ID
					AND RegionCode='*'
			)
	INSERT INTO GeoCity(CityName,GeoRegionID)
		SELECT DISTINCT 'Any',gr.ID
			FROM GeoRegion gr
			WHERE NOT EXISTS (
				SELECT * FROM GeoCity
					WHERE GeoRegionID=gr.ID
					AND CityName='Any'
			)
	INSERT INTO GeoLocation(GeoCityID,PostalCode)
		SELECT DISTINCT gc.ID,'City:'+CONVERT(VARCHAR,gc.ID)
			FROM GeoCity gc
			WHERE NOT EXISTS (
				SELECT * FROM GeoLocation 
					WHERE GeoCityID=gc.ID
					AND PostalCode='City:'+CONVERT(VARCHAR,gc.ID)
			)
GO
