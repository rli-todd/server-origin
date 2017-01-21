SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spLastNameGetID]( @LastName VARCHAR(30)) AS
	SET NOCOUNT ON
	DECLARE 
		@LastNameID INT
		
	SELECT @LastNameID=ID FROM LastName WHERE LastName=ISNULL(@LastName,'')
	IF @LastNameID IS NULL BEGIN
		INSERT INTO LastName(LastName) VALUES (ISNULL(@LastName,''))
		SET @LastNameID=SCOPE_IDENTITY()
	END
	RETURN @LastNameID
GO