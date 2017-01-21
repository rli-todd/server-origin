SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateJailTopNInit](@RowsPerState INT=10) AS
SET NOCOUNT ON;
TRUNCATE TABLE GeoStateJailTopN;
WITH cte AS
(
  SELECT s.StateName,s.StateAbbr,CityName,
      ROW_NUMBER() OVER(PARTITION BY s.StateFips ORDER BY AvgPopulation DESC)'RowNum',
      j.*
    FROM GeoState s
    JOIN GeoStateCityJail j
      ON s.StateFips=j.StateFips
		JOIN GeoCity c
			ON c.StateFips=j.StateFips
			AND c.CityFips=j.CityFips
)
  INSERT INTO GeoStateJailTopN(
    StateFips, StateName,StateAbbr,CityName,
    RowNum,
    JailName,Address,Zip,ConstructionYear,AvgPopulation,Population,
    AdultMaleInmates,AdultFemaleInmates,JuvenileMaleInmates,JuvenileFemaleInmates,
    FullTimeStaff,PartTimeStaff,RatedCapacity)
    SELECT StateFips,StateName,StateAbbr,CityName,
          RowNum,
          JailName,Address,Zip,ConstructionYear,AvgPopulation,Population,
          AdultMaleInmates,AdultFemaleInmates,JuvenileMaleInmates,JuvenileFemaleInmates,
          FullTimeStaff,PartTimeStaff,RatedCapacity
  FROM cte WHERE RowNum<=@RowsPerState
GO
