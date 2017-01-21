SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spCityLookupUpdateByState]( @StateFips TINYINT) AS
  SET NOCOUNT ON
  DECLARE cCity
    CURSOR FOR 
      SELECT CityFips
        FROM SolishineGeo..vGeoLocation gl
        WHERE CityFips IS NOT NULL
        AND StateFips = @StateFips
        GROUP BY CityFips
        ORDER BY 1
  DECLARE 
    @CityFips INT

  OPEN cCity
  FETCH NEXT FROM cCity INTO @CityFIps
  WHILE @@FETCH_STATUS=0
  BEGIN
    EXEC spCityLookupUpdate @StateFips=@StateFips, @CityFips=@CityFips
    FETCH NEXT FROM cCity INTO @CityFIps
  END
  CLOSE cCity
  DEALLOCATE cCity
GO
