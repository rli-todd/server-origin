
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spLastNameGet]( @LastName VARCHAR(30)) AS
	SET NOCOUNT ON
	DECLARE 
		@LastNameID INT

  IF LEN(ISNULL(@LastName,''))<2
  OR ( -- first 3 letters all the same
      LEN(@LastName)>2
      AND SUBSTRING(@LastName,1,1)=SUBSTRING(@LastName,2,1)
      AND SUBSTRING(@LastName,1,1)=SUBSTRING(@LastName,3,1)
  )
  OR EXISTS (SELECT 1 FROM RejectedName WHERE @LastName LIKE RejectedName)
		RETURN 0;

		
  --SET @LastName=dbo.fnMixedCase(@LastName)
	SELECT @LastNameID=ID FROM LastName WITH (INDEX(IX_LastName_Name)) WHERE LastName=@LastName
	IF @LastNameID IS NULL BEGIN
		INSERT INTO LastName(LastName) VALUES (@LastName)
		SET @LastNameID=SCOPE_IDENTITY()
	END
	RETURN @LastNameID

GO
