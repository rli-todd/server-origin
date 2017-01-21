SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityGetContentData]( @StateFips TINYINT, @CityFips INT) AS
SELECT 'GeoStateCityJail' 'TableName',*
  FROM GeoStateCityJail
    WHERE StateFips=@StateFips
    AND CityFips=@CityFips

SELECT 'GeoStateCityLawEnforcement' 'TableName',*
  FROM GeoStateCityLawENforcement
    WHERE StateFips=@StateFips
    AND CityFips=@CityFips

SELECT 'GeoStateCityLibrary' 'TableName',*
  FROM GeoStateCityLibrary
    WHERE StateFips=@StateFips
    AND CityFips=@CityFips

SELECT 'GeoStateCityOffenses2012' 'TableName',*
  FROM GeoStateCityOffenses2012
    WHERE StateFips=@StateFips
    AND CityFips=@CityFips
GO
