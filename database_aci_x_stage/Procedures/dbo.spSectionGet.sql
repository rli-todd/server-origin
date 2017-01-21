SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC spSectionGet( @AuthorizedUserID INT, @SectionID INT=NULL, @BlockID INT=NULL) AS
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
    SELECT s.*
      FROM Section s
      WHERE BlockID=@BlockID
  ELSE
    SELECT s.*
      FROM Section s
      WHERE ID=@SectionID

GO
