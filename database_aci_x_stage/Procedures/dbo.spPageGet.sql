SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spPageGet( @AuthorizedUserID INT, @PageID INT=NULL, @BlockID INT=NULL) AS
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


  IF @BlockID IS NOT NULL
    SELECT p.*
      FROM Page p
      JOIN PageBlock pb
        ON pb.PageID=p.ID
      WHERE pb.BlockID=@BlockID
  ELSE
    SELECT p.*
      FROM Page p
      WHERE ID=ISNULL(@PageID,ID)
GO
