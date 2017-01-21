SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoCountyGet]( @StateFips TINYINT) AS
  SET NOCOUNT ON
  SELECT * 
    FROM GeoStateCounty
    WHERE StateFips=@StateFips
    ORDER BY COuntyName
GO
