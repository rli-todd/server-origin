SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityCrimeStoppersTopNInit](@RowsPerCity INT=5) AS
	SET NOCOUNT ON;
		DROP TABLE GeoStateCityCrimeStoppers
		SELECT 
				CONVERT(VARCHAR(255),CrimeStopperName)'CrimeStopperName',
				Latitude,
				Longitude,
				CONVERT(VARCHAR(255),County)'CountyName',
				x.StateFips,
				x.CityFips,
				CONVERT(VARCHAR(20),Phone)'Phone'
			INTO GeoStateCityCrimeStoppers 
			FROM _import_CrimeStoppers i
			JOIN xvFips x
				ON x.StateAbbr=i.State
				AND x.CityName=i.City

	DROP TABLE GeoStateCityCrimeStoppersTopN;

	WITH cte AS
	(
		SELECT d.FromStateFips,d.FromCityFips,d.ToStateFips,d.ToCityFips,
				xt.StateName,xt.StateAbbr,xt.CityName,Miles,
				ROW_NUMBER() OVER(PARTITION BY d.FromStateFips,d.FromCityFips ORDER BY Miles )'RowNum',
				cs.*
			FROM xvFips xf
			JOIN GeoCityDistance d
				ON d.FromStateFips=xF.StateFips
				AND d.FromCityFips=xF.CityFips
			JOIN GeoStateCityCrimeStoppers cs
				ON d.ToStateFips=cs.StateFips
				AND d.ToCityFips=cs.CityFips
			JOIN xvFips xT
				ON xT.StateFips=ToStateFips
				AND xT.CityFips=ToCityFips
	)
		SELECT FromStateFips'StateFips',FromCityFips'CityFips',ToStateFips,ToCityFips,
						StateName,StateAbbr,CityName,Miles,RowNum,
						CrimeStopperName,Phone
			INTO GeoStateCityCrimeStoppersTopN
		FROM cte WHERE RowNum<=@RowsPerCity
		ORDER BY FromStateFips,FromCityFips,RowNum

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityCrimeStoppersTopN ON GeoStateCityCrimeStoppersTopN(StateFips,CityFips,ToStateFips,ToCityFips,RowNum)
GO
