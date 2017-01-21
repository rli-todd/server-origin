SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoCityGet]( @StateFips TINYINT, @CountyFips SMALLINT=NULL) AS
  SET NOCOUNT ON
  IF @CountyFips IS NULL
    SELECT *
      FROM GeoStateCity
      WHERE StateFips=@StateFips
      ORDER BY CityName
  ELSE
    SELECT *
      FROM GeoStateCountyCity
      WHERE StateFips=@StateFips
      AND CountyFips=@CountyFips
      ORDER BY CityName
GO
