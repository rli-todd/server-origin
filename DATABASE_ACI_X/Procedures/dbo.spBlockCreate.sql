SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spBlockCreate(
  @AuthorizedUserID INT,
  @BlockName VARCHAR(128), 
  @BlockType VARCHAR(20), 
  @IsEnabled BIT=0)
AS
  SET NOCOUNT ON
  DECLARE @BlockID INT

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

  IF EXISTS (
    SELECT 1
      FROM Block
      WHERE BlockName=@BlockName
  )
  BEGIN
    RAISERROR('Conflict: Block with that name already exists',11,1);
    RETURN;
  END

  INSERT INTO Block(BlockName,BlockType,IsEnabled)
    VALUES(@BlockName,@BlockType,@IsEnabled)
  SET @BlockID=SCOPE_IDENTITY()

  RETURN @BlockID
GO
