SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoLocationGetForCountryOnly]( @GeoLocationID INT) AS 
		SET NOCOUNT ON
		DECLARE
			@GeoCountryID SMALLINT,
			@RegionCode CHAR(2)
			
		SELECT @GeoCountryID=gc.ID,@RegionCode=RegionCode
			FROM GeoLocation gl
			JOIN GeoCity c ON c.ID=gl.GeoCityID
			JOIN GeoRegion gr ON gr.ID=c.GeoRegionID
			JOIN GeoCountry gc ON gc.ID=gr.GeoCountryID
			WHERE gl.ID=@GeoLocationID
			
		IF @RegionCode<>'*' BEGIN
			SELECT @GeoLocationID=gl.ID
				FROM GeoLocation gl
				JOIN GeoCity c ON c.ID=gl.GeoCityID
				JOIN GeoRegion gr ON gr.ID=c.GeoRegionID
				WHERE c.CityName='Any'
				AND gr.RegionCode='*'
				AND gr.GeoCountryID=@GeoCountryID
		END
		RETURN @GeoLocationID

GO
