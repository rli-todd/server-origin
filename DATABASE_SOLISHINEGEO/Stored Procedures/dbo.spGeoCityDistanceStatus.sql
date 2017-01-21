SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoCityDistanceStatus] AS
SET NOCOUNT ON;
WITH city AS
(
  SELECT FromStateFips,FromCityFips,COUNT(*) 'NumNearCities'
    FROM GeoCityDistance
    GROUP BY FromStateFips,FromCityFips
)
  SELECT *
    INTO #city
    FROM city

  SELECT x.StateFips,StateName,
        COUNT(DISTINCT FromCityFips)'ProcessedCities',COUNT(*)'NumCities',
        100 * COUNT(DISTINCT FromCityFips) / COUNT(*)'PercentComplete',
        SUM(NumNearCities)'NearNearCities'
    FROM xvFips x WITH(NOEXPAND)
    LEFT JOIN #city c
      ON c.FromStateFips=x.StateFips
      AND c.FromCityFips=x.CityFips
    GROUP BY x.StateFips,StateName
    ORDER BY 5

  SELECT 100 * COUNT(DISTINCT FromCityFips) / COUNT(*)'PercentComplete',
        COUNT(*)'NumCities',COUNT(DISTINCT FromCityFips)'ProcessedCities',
        SUM(NumNearCities)'NearNearCities'
    FROM xvFips x WITH(NOEXPAND)
    LEFT JOIN #city c
      ON c.FromStateFips=x.StateFips
      AND c.FromCityFips=x.CityFips

  SELECT x.StateFips,StateName,x.CityFips,CityName,0'NumCities'
    FROM xvFips x WITH(NOEXPAND)
    WHERE NOT EXISTS (
      SELECT 1
        FROM GeoCityDistance gcd
        WHERE gcd.FromStateFips=x.StateFips
        AND gcd.FromCityFips=x.CityFips
    )
    ORDER BY StateName,CityName

DROP TABLE #city
GO
