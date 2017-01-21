
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spVisitInfoGet2]( 
  @StringIpAddress VARCHAR(20), 
  @UserAgent VARCHAR(255), 
	@IntIpAddress INT OUTPUT, 
  @RobotID INT OUTPUT, 
  @RobotName VARCHAR(50) OUTPUT,
	@IsBlocked BIT OUTPUT,
  @StateFips INT=NULL OUTPUT,
  @CityFips INT=NULL OUTPUT,
  @CountryName VARCHAR(50)=NULL OUTPUT, 
  @RegionName VARCHAR(50)=NULL OUTPUT, 
	@CityName VARCHAR(50)=NULL OUTPUT,
  @Longitude REAL=NULL OUTPUT,
  @Latitude REAL=NULL OUTPUT,
  @GeoLocationID INT=NULL OUTPUT) AS
	SET NOCOUNT ON
		
	SET @IntIpAddress = dbo.fnIpAddressToInt(@StringIpAddress)
	EXEC @RobotID=spRobotGet
		@UserAgent=@UserAgent,
		@IpAddress=@IntIpAddress,
		@RobotName=@RobotName OUTPUT,
		@IsBlocked=@IsBlocked OUTPUT

	--SET @RobotID = dbo.fnRobotGetID(@UserAgent,@IntIpAddress)
	--SELECT @IsBlocked=Block, @RobotName=RobotName
	--	FROM Robot
	--	WHERE ID=@RobotID
	/*
	** WE can turn this on later when we can optimize it better.
	DECLARE @GeoLocationID INT
	*/
	SELECT TOP 1 @GeoLocationID = GeoLocationID
		FROM GeoIP
		WHERE IntIpAddrStart<=@IntIpAddress
		ORDER BY IntIpAddrStart DESC
		
	SELECT 
      @CountryName=CountryName,
      @RegionName=RegionName,
      @CityName=CityName,
      @CityFips=CityFips,
      @StateFips=StateFips,
      @Longitude=Longitude,
      @Latitude=Latitude
	  FROM xvGeoLocation WITH (NOEXPAND)
	  WHERE GeoLocationID=@GeoLocationID
	RETURN
GO
