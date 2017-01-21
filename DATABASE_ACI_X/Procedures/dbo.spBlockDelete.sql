SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spBlockDelete( @AuthorizedUserID INT, @BlockID INT)
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

  IF EXISTS (
    SELECT 1
      FROM PageBlock 
      WHERE BlockID=@BlockID
  )
  BEGIN
    RAISERROR('BadRequest: Block assigned to page(s)',11,1);
    RETURN;
  END

  IF EXISTS (
    SELECT 1
      FROM Section
      WHERE BlockID=@BlockID
  )
  BEGIN
    RAISERROR('BadRequest: Block has section(s)',11,1);
    RETURN;
  END

  DELETE Block
    WHERE ID=@BlockID
GO
