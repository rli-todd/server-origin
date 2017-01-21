SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spGeoRegionGet]( @GeoRegionCode CHAR(2), @GeoCountryCode CHAR(2)='US') AS
	SET NOCOUNT ON
	DECLARE 
		@GeoCountryID INT,
		@GeoRegionID INT
		
	SET @GeoRegionCode=ISNULL(@GeoRegionCode,'')
	SET @GeoCountryCode=ISNULL(@GeoCountryCode,'')
	EXEC @GeoCountryID=spGeoCountryGet @GeoCountryCode=@GeoCountryCode
	
	SELECT @GeoRegionID=ID 
		FROM GeoRegion 
		WHERE GeoCountryID=@GeoCountryID
		AND RegionCode=@GeoRegionCode
	
	IF @GeoRegionID IS NULL BEGIN
		INSERT INTO GeoRegion(GeoCountryID,RegionCode,RegionName) VALUES
			(@GeoCountryID,@GeoRegionCode,'Unknown')
		SET @GeoRegionID=SCOPE_IDENTITY()
	END
	RETURN @GeoRegionID
GO
