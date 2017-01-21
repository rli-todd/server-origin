SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityJailTopNInit](@RowsPerCity INT=10) AS
SET NOCOUNT ON
DROP TABLE GeoStateCityJailTopN;
WITH cte AS
(
  SELECT d.FromStateFips,d.FromCityFips,d.ToStateFips,d.ToCityFips,
      xt.StateName,xt.StateAbbr,xt.CityName,Miles,
      ROW_NUMBER() OVER(PARTITION BY d.FromStateFips,d.FromCityFips ORDER BY Miles )'RowNum',
      j.*
    FROM xvFips xf
    JOIN GeoCityDistance d
      ON d.FromStateFips=xF.StateFips
      AND d.FromCityFips=xF.CityFips
    JOIN GeoStateCityJail j
      ON d.ToStateFips=j.StateFips
      AND d.ToCityFips=j.CityFips
    JOIN xvFips xT
      ON xT.StateFips=ToStateFips
      AND xT.CityFips=ToCityFips
)
    SELECT FromStateFips'StateFips',FromCityFips'CityFips',ToStateFips,ToCityFips,
          StateName,StateAbbr,CityName,Miles,RowNum,
          JailName,Address,Zip,ConstructionYear,AvgPopulation,Population,
          AdultMaleInmates,AdultFemaleInmates,JuvenileMaleInmates,JuvenileFemaleInmates,
          FullTimeStaff,PartTimeStaff,RatedCapacity
			INTO GeoStateCityJailTopN
  FROM cte WHERE RowNum<=@RowsPerCity
	ORDER BY FromStateFips,FromCityFips,RowNUm

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityJailTopN ON GeoStateCityJailTopN(StateFips,CityFips,ToStateFips,ToCityFips,RowNum)

GO
