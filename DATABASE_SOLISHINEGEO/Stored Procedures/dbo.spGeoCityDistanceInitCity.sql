
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spGeoCityDistanceInitCity]( 
  @FromStateFips TINYINT, 
  @FromCityFips INT, 
  @MaxDistance INT=200) AS
  SET NOCOUNT ON
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

  DECLARE
    @FromPoint GEOGRAPHY,
    @CityName VARCHAR(MAX),
    @StateName VARCHAR(MAX)

  SELECT 
    @FromPoint=GEOGRAPHY::Point(AvgLatitude,AvgLongitude,4326),
    @StateName=StateName,
    @CityName=CityName
    FROM GeoCity gc
    JOIN GeoState gs
      ON gs.StateFips=gc.StateFips
    WHERE gc.StateFips=@FromStateFips
    AND gc.CityFips=@FromCityFips
    AND AvgLatitude IS NOT NULL
    AND AvgLongitude IS NOT NULL;

  WITH cte AS
  (
    SELECT 
      @FromStateFips'FromStateFips',
      @FromCityFips'FromCityFips',
      p2.StateFips'ToStateFips',
      p2.CityFips'ToCityFips',
      @FromPoint.STDistance(GEOGRAPHY::Point(p2.AvgLatitude,p2.AvgLongitude,4326))/1609.344'Miles'
      FROM GeoCity p2
      WHERE p2.AvgLatitude IS NOT NULL
      AND p2.AvgLongitude IS NOT NULL
  )
    SELECT FromStateFips,FromCityFips,ToStateFips,ToCityFips,Miles
      INTO #near_cities
      FROM cte
      WHERE Miles<@MaxDistance
      ORDER BY FromStateFips,FRomCityFips,ToStateFips,ToCityFips
  EXEC spPrint @@ROWCOUNT, ' #near_cities inserted for ', @CityName,', ',@StateName

  INSERT INTO GeoCityDistance(FromStateFips,FromCityFips,ToStateFips,ToCityFips,Miles)
    SELECT FromStateFips,FromCityFips,ToStateFips,ToCityFips,Miles
      FROM #near_cities
GO
