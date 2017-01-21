SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spApiClientIsAuthorized](@ClientSecret VARCHAR(128)) AS
  SET NOCOUNT ON
  DECLARE @RetVal INT=0
  IF EXISTS(
    SELECT 1
      FROM ApiClient
      WHERE ClientSecret=@ClientSecret
      AND IsEnabled=1
  )
    SET @RetVal=1
  RETURN @RetVal
GO
