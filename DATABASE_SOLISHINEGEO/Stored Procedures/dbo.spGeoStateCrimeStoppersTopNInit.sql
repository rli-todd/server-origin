SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCrimeStoppersTopNInit](@RowsPerState INT=10) AS
	SET NOCOUNT ON;
		DROP TABLE GeoStateCrimeStoppers
		SELECT 
				CONVERT(VARCHAR(255),CrimeStopperName)'CrimeStopperName',
				Latitude,
				Longitude,
				CONVERT(VARCHAR(255),County)'CountyName',
				x.StateFips,
				x.CityFips,
				CONVERT(VARCHAR(20),Phone)'Phone'
			INTO GeoStateCrimeStoppers 
			FROM _import_CrimeStoppers i
			JOIN xvFips x
				ON x.StateAbbr=i.State
				AND x.CityName=i.City

	DROP TABLE GeoStateCrimeStoppersTopN;

	WITH cte AS
	(
		SELECT xf.StateFips,xf.CityFips,
				xf.StateName,xf.StateAbbr,xf.CityName,
				ROW_NUMBER() OVER(PARTITION BY xf.StateFips ORDER BY Population DESC)'RowNum',
				cs.CrimeStopperName,CountyName,Phone,Population
			FROM xvFips xf
			JOIN xvStateCityPopulation xp
				ON xf.StateFips=xp.StateFips
				AND xf.CityFips=xp.CityFips
			JOIN GeoStateCityCrimeStoppers cs
				ON cs.StateFips=xf.StateFips
				AND cs.CityFips=xf.CityFIps
	)
		SELECT StateFips,CityFips,
						StateName,StateAbbr,CityName,RowNum,
						CrimeStopperName,Phone
			INTO GeoStateCrimeStoppersTopN
		FROM cte WHERE RowNum<=@RowsPerState
		ORDER BY stateFips,RowNum

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCrimeStoppersTopN ON GeoStateCrimeStoppersTopN(StateFips,RowNum)
GO
