SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spVisitGetV2]( @SiteID TINYINT, @VisitIDs ID_TABLE READONLY) AS
  SET NOCOUNT ON
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
			StorefrontUserToken
    FROM Visit v
    LEFT JOIN Users u
      ON u.ID=v.UserID
    LEFT JOIN sg_vGeoLocation gl
      ON gl.GeoLocationID=v.GeoLocationID
    WHERE v.SiteID=@SiteID
		AND EXISTS (
			SELECT 1
				FROM @VisitIDs vids
				WHERE vids.ID=v.ID
		)


GO
