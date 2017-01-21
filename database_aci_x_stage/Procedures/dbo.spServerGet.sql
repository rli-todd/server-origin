SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spServerGet]( @ServerName VARCHAR(50)) AS
  SET NOCOUNT ON
  DECLARE @ServerID TINYINT

  SELECT @ServerID=ID
    FROM Server
    WHERE ServerName=@ServerName

  IF @ServerID IS NULL
  BEGIN
    INSERT INTO Server(ServerName)VALUES(@ServerName)
    SET @ServerID=SCOPE_IDENTITY()
  END
  RETURN @ServerID
GO
