SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spVariationGet( 
  @AuthorizedUserID INT,
  @VariationID INT=NULL, 
  @SectionID INT=NULL) AS
  SET NOCOUNT ON

  IF NOT EXISTS (
    SELECT 1
      FROM Users
      WHERE ID=@AuthorizedUserID
      AND IsBackofficeReader=1
  )
  BEGIN
    RAISERROR('Unauthorized',11,1);
    RETURN;
  END

  IF @SectionID IS NOT NULL
    SELECT v.*
      FROM Variation v
      WHERE SectionID=@SectionID

  ELSE
    SELECT v.*
      FROM Variation v
      WHERE ID=@VariationID
GO
