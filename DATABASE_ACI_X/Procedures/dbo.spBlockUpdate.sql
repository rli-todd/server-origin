SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spBlockUpdate(
  @AuthorizedUserID INT,
  @BLockID INT,
  @BlockName VARCHAR(128)=NULL,
  @BlockType VARCHAR(20)=NULL,
  @IsEnabled BIT=0)
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
      FROM Block 
      WHERE ID=@BlockID
  )
  BEGIN
    RAISERROR('NotFound: Block not found',11,1);
    RETURN;
  END
  UPDATE Block SET 
    BlockName=ISNULL(@BlockName,BlockName),
    BlockType=ISNULL(@BlockTYpe,BlockType),
    IsEnabled=ISNULL(@IsEnabled,ISNULL(IsEnabled,0))
    WHERE ID=@BlockID
GO
