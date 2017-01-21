SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC spPageCreate( @AuthorizedUserID INT, @PageCode VARCHAR(50), @Description VARCHAR(MAX)) AS
  SET NOCOUNT ON
  DECLARE @PageID INT

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
      FROM Page
      WHERE PageCode=@PageCode
  )
  BEGIN
    RAISERROR('Conflict: Page with that page code already exists',11,1);
    RETURN;
  END

  INSERT INTO Page(PageCode,Description)
    VALUES(@PageCode,@Description)

  SET @PageID=SCOPE_IDENTITY();
  RETURN @PageID
GO
