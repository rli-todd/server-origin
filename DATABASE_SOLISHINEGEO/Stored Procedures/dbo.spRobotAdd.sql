SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spRobotAdd]( @RobotName VARCHAR(255),@UserAgentLike VARCHAR(255)=NULL,@Block BIT=0, @IpAddressStart VARCHAR(20)=NULL, @IpAddressEnd VARCHAR(20)=NULL, @Note VARCHAR(255)=NULL)
AS
	SET NOCOUNT ON
	DECLARE @RobotID INT
	INSERT INTO Robot(RobotName,UserAgentLike,Block)
		SELECT @RobotName,@UserAgentLike,@Block
	SET @RobotID=SCOPE_IDENTITY()
	IF @IpAddressStart IS NOT NULL
		EXEC spRobotAddAddressRange
			@RobotID=@RobotID,
			@IpAddressStart=@IpAddressStart,
			@IpAddressEnd=@IpAddressEnd,
			@Note=@Note
	SELECT * FROM vRobot WHERE RobotID=@RobotID
GO
