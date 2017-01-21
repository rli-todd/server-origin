SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spBlockGet( @AuthorizedUserID INT, @BlockID INT=NULL, @PageID INT=NULL) AS
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

  IF @PageID IS NOT NULL
    SELECT b.*
      FROM Block b
      JOIN PageBlock pb
        ON pb.BlockID=b.ID
      WHERE pb.PageID=@PageID
  ELSE
    SELECT b.*
      FROM Block b
      WHERE ID=ISNULL(@BlockID,ID)
GO
