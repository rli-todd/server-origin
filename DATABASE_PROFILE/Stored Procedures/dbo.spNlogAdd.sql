SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spNlogAdd](@LogLevel VARCHAR(20), @Logger NVARCHAR(256), @Message NVARCHAR(MAX), @ClientIP VARCHAR(16), @ServerName VARCHAR(32))
AS 
  SET NOCOUNT ON
  INSERT INTO NLog(LogDate,Level,Logger,Message,ClientIP,ServerName)
    VALUES (GETDATE(),@LogLevel,@Logger,@Message,@ClientIP,@ServerName)
GO
