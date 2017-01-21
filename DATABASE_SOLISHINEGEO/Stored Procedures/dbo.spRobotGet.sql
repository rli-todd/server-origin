SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROC [dbo].[spRobotGet]( @UserAgent VARCHAR(255), @IpAddress INT, @StringIpAddress VARCHAR(20)=NULL, @RobotName VARCHAR(50) OUTPUT, @IsBlocked BIT OUTPUT)
AS
	DECLARE 
		@RobotID INT=0,
		@HostName VARCHAR(128),
		@HostNameLike VARCHAR(50),
		@IsFraud BIT=0,
		@ForwardLookup VARCHAR(50)

	/*
	** First try a lookup by IP address
	*/
	SELECT @RobotID=RobotID
		FROM RobotAddressRange
		WHERE @IpAddress>=IpAddressStart
		AND @IpAddress<=IpAddressEnd

	IF @RobotID IS NOT NULL
	BEGIN
		SELECT @RobotName=RobotName, @IsBlocked=Block
			FROM Robot
			WHERE ID=@RobotID
		RETURN @RobotID
	END

	/* 
	** Now try by user-agent string 
	*/
	SELECT @RobotID=ID, @RobotName=RobotName, @IsBlocked=Block, @HostnameLike=HostnameLike
		FROM Robot	
		WHERE @UserAgent LIKE UserAgentLike

	IF @RobotID IS NOT NULL AND ISNULL(@IsBLocked,0)=0
	BEGIN
		/*
		** Ensure that we have an IP address for this bot
		*/
		IF NOT EXISTS (
			SELECT 1 
				FROM RobotAddressRange
				WHERE @IpAddress>=IpAddressStart
				AND @IpAddress<=IpAddressEnd)
		BEGIN
			IF @StringIpAddress IS NULL
				SET @StringIpAddress=dbo.fnIpAddressToString(@IpAddress)
			SET @Hostname = ISNULL(dbo.fnDnsReverseLookup(@StringIpAddress),'')
			IF @Hostname=''
			OR ISNULL(dbo.fnDnsForwardLookup(@Hostname),'') <> @StringIpAddress
			OR @Hostname NOT LIKE @HostnameLike
				SET @IsFraud=1

			IF @IsFraud=0
				INSERT INTO RobotAddressRange(RobotID,IpAddressStart,IpAddressEnd,Note)
					VALUES(@RobotID,@IpAddress,@IpAddress,'Validated by spRobotGet: Hostname='+@Hostname)
			ELSE
			BEGIN
				DECLARE @FraudName VARCHAR(128)='FRAUD:'+@UserAgent
				SET @RobotID=NULL
				SELECT @RobotID=ID
					FROM Robot
					WHERE RobotName = @FraudName
				IF @RobotID IS NULL
				BEGIN
					INSERT INTO Robot(RobotName,UserAgentLike,Block)
						VALUES (@FraudName,NULL,1)
					SET @RobotID=SCOPE_IDENTITY()
				END
				INSERT INTO RobotAddressRange(RobotID,IpAddressStart,IpAddressEnd,Note)
					VALUES(@RobotID,@IpAddress,@IpAddress,'Fraud discovered by spRobotGet; Hostname='+@Hostname)
			END
		END
	END

	/*
	** See if it's from a blocked country
	*/

	IF @RobotID IS NULL
	BEGIN
		DECLARE 
			@CountryIsoCode CHAR(2),
			@CountryGeoNameID INT,
			@CountryName VARCHAR(255)
		SELECT 
				@CountryIsoCode=ISNULL(RepresentedCountryIsoCode,ISNULL(CountryIsoCode,ISNULL(RegisteredCountryIsoCode,'??'))),
				@CountryGeoNameID=ISNULL(RepresentedCountryGeoNameID,ISNULL(GeoNameID,RegisteredCountryGeoNameID))
			FROM MMCountryBlock
			WHERE IpAddrStart<= @IpAddress
			AND IpAddrEnd>=@IpAddress

		IF @CountryIsoCode NOT IN ('US','CA')
		BEGIN
			SELECT @RobotID=ID 
				FROM Robot
				WHERE RobotName='BlockedCountry:'+@CountryIsoCode
			IF @RobotID IS NULL
			BEGIN
				SELECT @COuntryName=CountryName
					FROM MMLocation
					WHERE GeoNameID=@CountryGeoNameID

				INSERT INTO Robot(RobotName,Block,Note)
					SELECT 'BlockedCountry:'+@CountryIsoCode,1,ISNULL(@CountryName,'Unknown')
			END
		END
	END
	RETURN ISNULL(@RobotID,0)

GO
