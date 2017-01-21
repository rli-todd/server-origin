
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spFirstNameGet]( @FirstName VARCHAR(30)) AS
	SET NOCOUNT ON
	DECLARE @FirstNameID INT
	--SET @FirstName=dbo.fnMixedCase(ISNULL(@FirstName,''))

  IF LEN(ISNULL(@FirstName,''))<1
  OR ( -- first 3 letters all the same
      LEN(@FirstName)>2
      AND SUBSTRING(@FirstName,1,1)=SUBSTRING(@FirstName,2,1)
      AND SUBSTRING(@FirstName,1,1)=SUBSTRING(@FirstName,3,1)
  )
  OR EXISTS (SELECT 1 FROM RejectedName WHERE @FirstName LIKE RejectedName)
		RETURN 0


	BEGIN TRAN
	SELECT @FirstNameID=ID FROM FirstName WHERE FirstName=@FirstName
	IF @FirstNameID IS NULL BEGIN
		INSERT INTO FirstName(FirstName) VALUES (@FirstName)
		SET @FirstNameID=SCOPE_IDENTITY()
	END
	COMMIT
	RETURN @FirstNameID

GO
