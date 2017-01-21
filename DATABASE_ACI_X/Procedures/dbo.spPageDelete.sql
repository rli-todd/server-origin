SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC spPageDelete( @AuthorizedUserID INT, @PageID INT ) AS
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
      FROM Page
      WHERE ID=@PageID
  )
  BEGIN
    RAISERROR('NotFound: Page not found',11,1);
    RETURN;
  END

  IF EXISTS (
    SELECT 1
      FROM PageBlock 
      WHERE PageID=@PageID
  )
  BEGIN
    RAISERROR('BadRequest: Page contains block(s)',11,1);
    RETURN;
  END

  DELETE Page
    WHERE ID=@PageID
GO
