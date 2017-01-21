SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spCityLookupUpdateAll] AS 
  SET NOCOUNT ON
  DECLARE cState
    CURSOR FOR 
      SELECT ISNULL(gl.StateFips,gc.StateFips)
        FROM SolishineGeo..GeoLocation gl
        JOIN SolishineGeo..GeoCity gc
          ON gc.ID=gl.GeoCityID
        JOIN SolishineGeo..GeoRegion gr
          ON gr.ID=gc.GeoRegionID
        WHERE ISNULL(gl.StateFips,gc.StateFips) IS NOT NULL
        GROUP BY ISNULL(gl.StateFips,gc.StateFips)
        ORDER BY 1
  DECLARE 
    @StateFips TINYINT

  OPEN cState
  FETCH NEXT FROM cState INTO @StateFips
  WHILE @@FETCH_STATUS=0
  BEGIN
    EXEC spCityLookupUpdateByState @StateFips=@StateFips
    FETCH NEXT FROM cState INTO @StateFips
  END
  CLOSE cState
  DEALLOCATE cState
GO
