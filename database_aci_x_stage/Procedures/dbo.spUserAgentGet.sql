SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spUserAgentGet]( @UserAgent VARCHAR(255)) AS
  SET NOCOUNT ON
  DECLARE @UserAgentID INT
  SELECT @UserAgentID=ID
    FROM UserAgent
    WHERE UserAgent=@UserAgent
  IF @UserAgentID IS NULL
  BEGIN
    INSERT INTO UserAgent(UserAgent)VALUES(@UserAgent)
    SET @UserAgentID=SCOPE_IDENTITY()
  END
  RETURN @UserAgentID

GO
