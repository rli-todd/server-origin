SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spAliasGetID](
  @FirstNameID INT, 
  @MiddleNameID INT,
  @LastNameID INT) AS
  SET NOCOUNT ON

  DECLARE 
    @AliasID INT

  SELECT @AliasID=ID
    FROM Alias
    WHERE FirstNameID=@FirstNameID
    AND MiddleNameID=@MiddleNameID
    AND LastNameID=@LastNameID

  IF @AliasID IS NULL
  BEGIN
    INSERT INTO Alias(FirstNameID,LastNameID,MiddleNameID)
      VALUES (@FirstNameID,@LastNameID,@MiddleNameID)
    SET @AliasID=SCOPE_IDENTITY()
  END
    
  RETURN @AliasID


GO
