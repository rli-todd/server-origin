SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spSectionCreate( 
  @AuthorizedUserID INT, 
  @BlockID INT, 
  @SectionName VARCHAR(128), 
  @SectionType VARCHAR(20), 
  @IsEnabled BIT=0) AS
  SET NOCOUNT ON
  DECLARE @SectionID INT

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
  INSERT INTO Section(BlockID,SectionName,SectionType,IsEnabled)
    VALUES(@BlockID,@SectionName,@SectionType,@IsEnabled)
  SET @SectionID=SCOPE_IDENTITY()
  RETURN @SectionID
GO
