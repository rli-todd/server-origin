SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spBlockAddPage( @AuthorizedUserID INT, @BlockID INT, @PageID INT) AS
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

  IF NOT EXISTS (
    SELECT 1
      FROM Block 
      WHERE ID=@BlockID
  )
  BEGIN
    RAISERROR('NotFound: Block not found',11,1);
    RETURN;
  END

  INSERT INTO PageBlock(BlockID,PageID)
    SELECT @BlockID,@PageID
      WHERE NOT EXISTS (
        SELECT 1
          FROM PageBlock pb
          WHERE pb.PageID=@PageID
          AND pb.BlockID=@BLockID
      )
GO
