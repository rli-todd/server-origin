SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC spPageUpdate( 
  @AuthorizedUserID INT,
  @PageID INT, 
  @PageCode VARCHAR(50), 
  @Description VARCHAR(MAX)) AS
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
  UPDATE Page SET PageCode=@PageCode,Description=@Description
    WHERE ID=@PageID
GO
