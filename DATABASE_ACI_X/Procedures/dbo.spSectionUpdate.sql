SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC spSectionUpdate(
  @AuthorizedUserID INT,
  @SectionID INT, 
  @SectionName VARCHAR(128)=NULL, 
  @SectionType VARCHAR(20)=NULL, 
  @IsEnabled BIT=0) AS
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
      FROM Section
      WHERE ID=@SectionID
  )
  BEGIN
    RAISERROR('NotFound: Section not found',11,1);
    RETURN;
  END

  UPDATE Section SET
    SectionName=ISNULL(@SectionName,SectionName),
    SectionType=ISNULL(@SectionType,SectionType),
    IsEnabled=ISNULL(@IsEnabled,ISNULL(IsEnabled,0))
    WHERE ID=@SectionID
GO
