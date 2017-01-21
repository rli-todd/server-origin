SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE FUNCTION [dbo].[fnGeoLocationIDFromIpAddrInt]( @IntIpAddr INT) RETURNS INT
AS BEGIN
	DECLARE	@GeoLocationID INT
	SELECT @GeoLocationID = GeoLocationID
		FROM GeoIP
		WHERE IntIpAddrStart<=@IntIpAddr
		AND IntIpAddrEnd>@IntIpAddr
	RETURN @GeoLocationID
END
GO
