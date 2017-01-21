SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[spVisitCreate]( 
	@UserAgent VARCHAR(255)=NULL, 
	@IpAddress VARCHAR(20)=NULL,
	@AcceptLanguage VARCHAR(10)=NULL,
	@LandingUrl VARCHAR(512)=NULL,
	@RefererUrl VARCHAR(512)=NULL,
  @WebServerName VARCHAR(50)=NULL,
  @ApiServerName VARCHAR(50)=NULL,
  @UserGuid UNIQUEIDENTIFIER=NULL,
	@ReadOnly BIT=0)
AS
	DECLARE 
    @VisitDate DATETIME=GETDATE(),
    @VisitGuid UNIQUEIDENTIFIER,
		@IntIpAddress INT=dbo.fnIpAddresstoInt(@IpAddress),
    @SiteID TINYINT,
    @VisitID INT,
    @UserID INT,
		@GeoLocationID INT,
		@RobotID INT,
    @WebServerID TINYINT,
    @ApiServerID TINYINT,
    @ExistingRobotID INT,
		@RobotName VARCHAR(50),
		@IsBlocked BIT,
		@CountryName VARCHAR(50),
		@RegionName VARCHAR(50),
		@CityName VARCHAR(50),
    @UserAgentID INT,
    @LandingUrlID INT,
    @RefererUrlID INT,
    @ReferCode VARCHAR(20),
    @StateFips INT,
    @CityFips INT,
    @Longitude REAL,
    @Latitude REAL,
		@IwsUserToken VARCHAR(50),
		@IwsUserTokenExpiry DATETIME,
		@IwsUserID INT,
		@StoreFrontUserToken VARCHAR(50),
		@UtcOffsetMins INT
		
	SET NOCOUNT ON
	
	IF @ReadOnly=0
	BEGIN
		EXEC @LandingUrlID=spUrlGet 
			@Url=@LandingUrl, 
			@UrlTypeCode='L', 
			@SiteID=@SiteID OUTPUT, 
			@ReferCode=@ReferCode OUTPUT
		EXEC @RefererUrlID=spUrlGet 
			@Url=@RefererUrl, 
			@UrlTypeCOde='R', 
			@SiteID=@SiteID
		EXEC @UserAgentID=spUserAgentGet @UserAgent
		EXEC @WebServerID=spServerGet @WebServerName
		EXEC @ApiServerID=spServerGet @ApiServerName
	END

	EXEC sg_spVisitInfoGet2 
    @StringIpAddress=@IpAddress, 
    @UserAgent=@UserAgent,
		@IntIpAddress=@IntIpAddress OUTPUT, 
    @RobotID=@RobotID OUTPUT, 
    @RobotName=@RobotName OUTPUT,
    @GeoLocationID=@GeoLocationID OUTPUT,
		@IsBlocked=@IsBlocked OUTPUT, 
    @CountryName=@CountryName OUTPUT,
    @RegionName=@RegionName OUTPUT, 
		@CityName=@CityName OUTPUT,
    @StateFips=@StateFips OUTPUT,
    @CityFips=@CityFips OUTPUT,
		@UtcOffsetMins=@UtcOffsetMins OUTPUT

		/*
		** Let's see if there's already a visit in there for this user
    */

		SELECT 
				@VisitID=ID,
				@ExistingRobotID=RobotID,
				@VisitGuid=VisitGuid,
				@IwsUserToken=IwsUserToken,
				@IwsUserTokenExpiry=IwsUserTokenExpiry,
				@IwsUserID=IwsUserID,
				@UserID=UserID,
				@StorefrontUserToken=StorefrontUserToken
			FROM Visit
			WHERE SiteID=@SiteID
			AND IpAddress=@IntIpAddress
			AND UserAgentID=@UserAgentID
			AND DateCreated>=DATEADD(minute,-30,@VisitDate)

    /*
    ** If a UserGuid is specified, grab the user
    */
    IF @UserGuid IS NOT NULL
      SELECT @UserID=ID,@IwsUserID=ExternalID
        FROM Users
        WHERE UserGuid=@UserGuid
			
		IF @ReadOnly=0
		BEGIN
			IF @VisitID IS NOT NULL
			BEGIN
				IF ISNULL(@RobotID,0)=ISNULL(@ExistingRobotID,0)
					UPDATE Visit SET 
						UseCount=ISNULL(UseCount,0)+1,
						UserID=ISNULL(@UserID,UserID),
						DateModified=GETDATE() 
						WHERE SiteID=@SiteID
						AND ID=@VisitID
				ELSE
					/* Same IP address and UserAgent, but different RobotID?
					*/
					SET @VisitID=NULL
			END
			
			IF @VisitID IS NULL
			BEGIN
				SET @VisitGuid=NEWID();
				BEGIN TRAN
				SET @VisitID=NEXT VALUE FOR VisitID
				WHILE EXISTS (SELECT 1 FROM Visit WHERE SiteID=@SiteID AND ID=@VisitID)
				BEGIN
					INSERT INTO NLog(LogDate,Level,Logger,Message,ClientIP,UserID,Server)
						VALUES (
							GETDATE(),
							'Error',
							'spVisitCreate',
							'VisitID '+CONVERT(VARCHAR(MAX),@VisitID)+' obtained from Sequence already exists for SiteID ' + CONVERT(VARCHAR(MAX),@SiteID),
							@IntIpAddress,
							@UserID,
							@@ServerName)
						SET @VisitID=NEXT VALUE FOR VisitID
				END
				INSERT INTO Visit(
						ID,SiteID,WebServerID,APiServerID,DateCreated,DateModified,
						VisitGuid,UserAgentID,LandingUrlID,RefererUrlID,
						IpAddress,IsBlocked,UseCount,UserID,RobotID,ConversionPathID,
						IwsReferCode,UtcOffsetMins,AcceptLanguage) VALUES 
						(@VisitID,@SiteID,@WebServerID,@ApiServerID,GETDATE(),GETDATE(),
						@VisitGuid,@UserAgentID,@LandingUrlID,@RefererUrlID,
						@IntIpAddress,ISNULL(@IsBlocked,0),1,@UserID,@RobotID,NULL,
						@ReferCode,@UtcOffsetMins,@AcceptLanguage)
				COMMIT
			END
	END
	SELECT 
    @VisitID'VisitID',
    @VisitGuid'VisitGuid',
    @UserID'UserID',
    @IpAddress'ClientIP',
		@UtcOffsetMins'UtcOffsetMins',
		@AcceptLanguage'AcceptLanguage',
    @UserGuid'UserGuid',
    ISNULL(@IsBlocked,0)'IsBlocked',
    @ReferCode'IwsReferCode',
    @SiteID'SiteID',
    @CountryName'CountryName',
    @RegionName'RegionName',
    @CityName'CityName',
    @StateFips'StateFips',
    @CityFips'CityFips',
    @Longitude'Longitude',
    @Latitude'Latitude',
    @GeoLocationID'GeoLocationID',
		CONVERT(BIT,CASE WHEN ISNULL(@IwsUserTokenExpiry,'1/1/2000')>GETDATE() THEN 1 ELSE 0 END)'IsAuthenticated',
		@IwsUserToken'IwsUserToken',
		@IwsUserTokenExpiry'IwsUserTokenExpiry',
    @IwsUserID'IwsUserID',
		@StoreFrontUserToken'StorefrontUserToken',
		ISNULL(@RobotID,0)'RobotID'


		--EXEC spVisitGet @VisitID=@VisitID, @SiteID=@SiteID

GO
