SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spVaritionDelete( @AuthorizedUserID INT, @VariationID INT)
AS
  SET NOCOUNT ON

  IF NOT EXISTS (
    SELECT 1
      FROM Users
      WHERE ID=@AuthorizedUserID
      AND IsBackofficeWriter=1
  )
  BEGIN
    RAISERROR('Unauthorized',11,1);
    RETURN;
  END


  IF NOT EXISTS (
    SELECT 1
      FROM Variation
      WHERE ID=@VariationID
  )
  BEGIN
    RAISERROR('NotFound: Variation not found',11,1);
    RETURN;
  END

  DELETE Variation
    WHERE ID=@VariationID                   
GO
