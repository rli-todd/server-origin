
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spGeoCityDistanceInitState]( @StateFips TINYINT, @MaxDistance INT=200, @InsertToTempTableOnly BIT=1) AS
  SET NOCOUNT ON
  DECLARE cFromCity CURSOR FOR 
    SELECT CityFips
    FROM GeoCity gc
    JOIN GeoState gs
      ON gs.StateFips=gc.StateFips
    WHERE gc.StateFips=@StateFips
    AND AvgLatitude IS NOT NULL
    AND AvgLongitude IS NOT NULL
    ORDER BY gc.StateFips,gc.CityFips

  DECLARE
    @FromCityFips INT

  OPEN cFromCity
  FETCH NEXT FROM cFromCity INTO @FromCityFips
  WHILE @@FETCH_STATUS=0
  BEGIN
    EXEC spGeoCityDistanceInitCity
      @FromStateFips=@StateFips,
      @FromCityFips=@FromCityFips,
      @MaxDistance=@MaxDistance,
      @InsertToTempTableOnly=@InsertTOTempTableOnly
    FETCH NEXT FROM cFromCity INTO @FromCityFips
  END
  CLOSE cFromCity
  DEALLOCATE cFromCity

GO
