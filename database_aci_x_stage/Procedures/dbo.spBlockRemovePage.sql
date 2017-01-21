SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spBlockRemovePage( @AuthorizedUserID INT, @BlockID INT, @PageID INT) AS
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
      FROM Block 
      WHERE ID=@BlockID
  )
  BEGIN
    RAISERROR('NotFound: Block not found',11,1);
    RETURN;
  END

  DELETE PageBlock
    WHERE PageID=@PageID
    AND BlockID=@BlockID

GO
