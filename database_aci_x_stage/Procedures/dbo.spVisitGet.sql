SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spVisitGet]( @SiteID TINYINT, @VisitID INT) AS
  SET NOCOUNT ON
	/*
	** Note that the geo location values will be inaccurate, because they are
	** being pulled from vGeoLocation rather than the newer, more accurate MMLocation table.
	** This needs to be reconciled and the GeoLocation table needs to be deprecated or updated
	** with the latest MMLocation data.
	*/
  SELECT 
      v.ID'VisitID',
      VisitGuid,
      u.ID'UserID',
      dbo.fnIpAddressToString(v.IpAddress)'ClientIP',
      UserGuid,
      IsBlocked,
      IwsReferCode,
      v.SiteID,
      CountryName,
      RegionName,
      CityName,
      StateFips,
      CityFips,
      Longitude,
      Latitude,
      gl.GeoLocationID,
      CONVERT(BIT,CASE WHEN ISNULL(IwsUserTokenExpiry,'1/1/2000')>GETDATE() THEN 1 ELSE 0 END)'IsAuthenticated',
      IwsUserToken,
      IwsUserTokenExpiry,
      IwsUserID,
			StorefrontUserToken,
			AcceptLanguage,
			CONVERT(INT,UtcOffsetMins)'UtcOffsetMins'
    FROM Visit v
    LEFT JOIN Users u
      ON u.ID=v.UserID
    LEFT JOIN sg_vGeoLocation gl
      ON gl.GeoLocationID=v.GeoLocationID
    WHERE v.SiteID=@SiteID
		AND v.ID=@VisitID


GO
