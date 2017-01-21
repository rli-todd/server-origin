SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityCrimeInit] AS

INSERT INTO _import_WPCityCrime
	SELECT * 
		FROM _import_WPCityCrimeNoFips n
		WHERE NOT EXISTS (
			SELECT 1
				FROM _import_WPCityCrime f
				WHERE f.City=n.City
				AND f.State=n.State

		)

UPDATE _import_WPCityCrime SET fips=CityFips
	FROM _import_WPCityCrime i
	LEFT JOIN xvFips x
		ON x.StateAbbr=i.State
		AND x.CityName=i.City
	WHERE x.CityFips IS NOT NULL
	AND i.fips=0

INSERT INTO GeoCity(CityName,GeoRegionID,StateFips,CityFips)
	SELECT DISTINCT i.City,gr.ID,gr.StateFips,i.fips
		FROM _import_WPCityCrime i
		JOIN GeoRegion gr
			ON gr.RegionCode=i.State
	LEFT JOIN xvFips x
		ON x.StateAbbr=i.State
		AND x.CityName=i.City
	WHERE CityFips IS NULL AND fips<>0
	AND NOT EXISTS(
		SELECT 1
			FROM GeoCity gc
			WHERE gc.StateFips=gr.StateFips
			AND gc.CityFips=i.Fips
	)

UPDATE GeoLocation SET GeoCityID=n.ID
	FROM GeoLocation gl
	JOIN GeoCity o
		ON o.ID=gl.GeoCityID
	JOIN GeoCity n
		ON n.GeoRegionID=o.GeoRegionID
		AND n.CityName=o.CityName
	WHERE ISNULL(o.CityFips,0)=0
	AND n.CityFips<>0

DROP TABLE GeoStateCityCrime

SELECT 
		StateFips,CityFips,Year,StateAbbr,StateName,CityName,Agency,Population,violent_crime_rate'ViolentCrimeRate',
		Murder,Rape,Robbery,Assault,Property,Burglary,Larceny,Vehicular
	INTO GeoStateCityCrime
	FROM _import_WPCityCrime i
	JOIN xvFips x
		ON x.StateAbbr=i.State
		AND x.CityFips=i.fips
	WHERE i.Fips<>0

CREATE CLUSTERED INDEX IX_GeoStateCityCrime ON GeoStateCityCrime(StateFips,CityFips,Year)
GO
