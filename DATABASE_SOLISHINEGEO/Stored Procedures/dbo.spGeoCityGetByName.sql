SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spGeoCityGetByName]( @CityName VARCHAR(30), @GeoRegionCode CHAR(2), @GeoCountryCode CHAR(2)='US') AS
	SET NOCOUNT ON
	DECLARE 
		@GeoRegionID INT,
		@GeoCityID INT
	
	SET @GeoRegionCode=ISNULL(@GeoRegionCode,'')
	SET @GeoCountryCode=ISNULL(@GeoCountryCode,'')
	SET @CityName=ISNULL(@CityName,'')
	
	EXEC @GeoRegionID=spGeoRegionGet
		@GeoRegionCode=@GeoRegionCode,
		@GeoCountryCode=@GeoCountryCode
	
	SELECT @GeoCityID=ID FROM GeoCity
		WHERE GeoRegionID=@GeoRegionID
		AND CityName=@CityName
	
	IF @GeoCityID IS NULL BEGIN
		INSERT INTO GeoCity(CityName,GeoRegionID) VALUES
			(@CityName,@GeoRegionID)
		SET @GeoCityID=SCOPE_IDENTITY()
	END
	RETURN @GeoCityID
GO
