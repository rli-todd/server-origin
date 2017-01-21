SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spRobotAddAddressRange]( @RobotID INT, @IpAddressStart VARCHAR(20),@IpAddressEnd VARCHAR(20)=NULL, @Note VARCHAR(255)=NULL) AS

	INSERT INTO RobotAddressRange(RobotID,IpAddressStart,IpAddressEnd,Note)
		VALUES(@RobotID,dbo.fnIpAddressToInt(@IpAddressStart),dbo.fnIpAddressToInt(ISNULL(@IpAddressEnd,@IpAddressStart)),@Note)
GO
