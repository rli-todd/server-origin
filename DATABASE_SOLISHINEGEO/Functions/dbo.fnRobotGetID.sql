SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE FUNCTION [dbo].[fnRobotGetID]( @UserAgent VARCHAR(255), @IpAddress INT) RETURNS INT
WITH EXECUTE AS CALLER
AS
	BEGIN
		DECLARE @RobotID INT
		SELECT @RobotID=ID FROM Robot	WHERE @UserAgent LIKE UserAgentLike
		IF @RobotID IS NULL BEGIN
			SELECT @RobotID=RobotID 
				FROM RobotAddressRange 
				WHERE @IpAddress>=IpAddressStart
				AND @IpAddress<=IpAddressEnd
		END
		RETURN @RobotID
	END

GO
GRANT EXECUTE ON  [dbo].[fnRobotGetID] TO [public]
GO
