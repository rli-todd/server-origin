SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
	

CREATE PROC [dbo].[spGeoLocationGetUnknown]( @GeoCountryCode CHAR(2)='US', @GeoRegionCode CHAR(2), @CityName VARCHAR(30)) AS
	SET NOCOUNT ON
	DECLARE 
		@GeoCityID INT,
		@GeoLocationID INT
		
	SET @GeoRegionCode=ISNULL(@GeoRegionCode,'')
	SET @GeoCountryCode=ISNULL(@GeoCountryCode,'')
	SET @CityName=ISNULL(@CityName,'')
	
	EXEC @GeoCityID=spGeoCityGetByName
		@CityName=@CityName,
		@GeoRegionCode=@GeoRegionCode,
		@GeoCountryCode=@GeoCountryCode

	SELECT @GeoLocationID=ID
		FROM GeoLocation
		WHERE GeoCityID=@GeoCityID
		AND Latitude IS NULL
		AND Longitude IS NULL
		AND AreaCode IS NULL
		AND DmaCode IS NULL
		AND postalCode='City:'+CONVERT(VARCHAR,@GeoCityID)
	
	IF @GeoLocationID IS NULL BEGIN
		INSERT INTO GeoLocation(GeoCityID,Latitude,Longitude,AreaCode,DmaCode,PostalCode) VALUES
			(@GeoCityID,NULL,NULL,NULL,NULL,'City:'+CONVERT(VARCHAR,@GeoCityID))
		SET @GeoLocationID=SCOPE_IDENTITY()
	END
	SELECT * FROM vGeoLocationSelector WHERE GeoLocationID=@GeoLocationID
GO
