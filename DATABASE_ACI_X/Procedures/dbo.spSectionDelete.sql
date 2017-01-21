SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE  PROC spSectionDelete(@AuthorizedUserID INT, @SectionID INT)
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
      FROM Section
      WHERE ID=@SectionID
  )
  BEGIN
    RAISERROR('NotFound: Section not found',11,1);
    RETURN;
  END

  IF EXISTS (
    SELECT 1
      FROM Variation
      WHERE SectionID=@SectionID
  )
  BEGIN
    RAISERROR('BadRequest: Section contains variation(s)',11,1);
    RETURN;
  END

  DELETE Section
    WHERE ID=@SectionID
GO
