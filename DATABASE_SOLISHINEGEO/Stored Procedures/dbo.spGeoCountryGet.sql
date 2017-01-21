SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spGeoCountryGet]( @GeoCountryCode CHAR(2)) AS
	SET NOCOUNT ON
	DECLARE @GeoCountryID INT
	SET @GeoCountryCode=ISNULL(@GeoCountryCode,'')
	SELECT @GeoCountryID=ID 
		FROM GeoCountry 
		WHERE CountryCode=@GeoCountryCode
	IF @GeoCountryID IS NULL BEGIN
		INSERT INTO GeoCountry(CountryCode,CountryName)VALUES(@GeoCountryCode,'Unknown')
		SET @GeoCountryID=SCOPE_IDENTITY()
	END
	RETURN @GeoCountryID
GO
