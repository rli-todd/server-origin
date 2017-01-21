SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[fnGeoLocationIDFromIpAddrString]( @StrIpAddr VARCHAR(20)) RETURNS INT
AS BEGIN
	RETURN dbo.fnGeoLocationIDFromIpAddrInt( dbo.fnIpAddressToInt( @StrIpAddr ) )
END

GO
GRANT EXECUTE ON  [dbo].[fnGeoLocationIDFromIpAddrString] TO [public]
GO
