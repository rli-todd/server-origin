SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spVisitInfoGet]( @StringIpAddress VARCHAR(20), @UserAgent VARCHAR(255)) AS
	SET NOCOUNT ON
	DECLARE 
		@IntIpAddress INT,
		@RobotID INT,
		@IsBlocked BIT,
		@RobotName VARCHAR(50),
		@Country VARCHAR(50),
		@Region VARCHAR(50),
		@City VARCHAR(50)
		
	SET @IntIpAddress = dbo.fnIpAddressToInt(@StringIpAddress)
	SET @RobotID = dbo.fnRobotGetID(@UserAgent,@IntIpAddress)
	SELECT @IsBlocked=Block, @RobotName=RobotName
		FROM Robot
		WHERE ID=@RobotID
	SELECT @Country=Country,@Region=Region,@City=City
		FROM GeoLocation
		WHERE ID=dbo.fnGeoLocationIDFromIpAddrInt(@IntIpAddress)
	SELECT 
		@IntIpAddress'IntIpAddress',
		ISNULL(@RobotID,0)'RobotID',
		ISNULL(@RobotName,'')'RobotName',
		ISNULL(@IsBlocked,0)'IsBlocked',
		ISNULL(@Country,'')'Country',
		ISNULL(@Region,'')'Region',
		ISNULL(@City,'')'City'
		
GO
