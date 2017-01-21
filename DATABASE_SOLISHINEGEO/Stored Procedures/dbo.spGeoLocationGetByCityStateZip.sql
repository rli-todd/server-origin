SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spGeoLocationGetByCityStateZip]( @CityName VARCHAR(50), @StateCode CHAR(2), @Zip VARCHAR(10)) AS
  SET NOCOUNT ON
  DECLARE 
    @GeoCityID INT,
    @GeoLocationID INT

  EXEC @GeoCityID=spGeoCityGetByName @CityName=@CityName, @GeoRegionCode=@StateCode
  SELECT TOP 1 @GeoLocationID=ID
    FROM GeoLocation
    WHERE GeoCityID=@GeoCityID
    AND PostalCode=@Zip

  IF @GeoLocationID IS NULL
    SELECT TOP 1 @GeoLocationID=ID
      FROM GeoLocation
      WHERE GeoCityID=@GeoCityID

  IF @GeoLocationID IS NULL
  BEGIN
    INSERT INTO GeoLocation(GeoCityID,PostalCode) VALUES (@GeoCityID,@Zip)
    SET @GeoLocationID=SCOPE_IDENTITY()
  END
  RETURN @GeoLocationID
GO
