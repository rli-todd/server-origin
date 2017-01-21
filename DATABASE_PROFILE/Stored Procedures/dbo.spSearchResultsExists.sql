SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spSearchResultsExists]( 
	@SearchType INT,
	@FirstName VARCHAR(30),
	@MiddleName VARCHAR(30),
	@LastName VARCHAR(30),
	@State VARCHAR(2)) AS
BEGIN
	SET NOCOUNT ON
	DECLARE
		@FirstNameID INT,
		@LastNameID INT,
		@SearchResultsID INT
		
	EXEC @FirstNameID=spFirstNameGet @FirstName
	EXEC @LastNameID=spLastNameGet @LastName
	
	SET @MiddleName=UPPER(ISNULL(@MiddleName,''))
	SET @State=UPPER(RTRIM(LTRIM(ISNULL(@State,''))))
	SET @SearchType=ISNULL(@SearchType,0)

	SELECT @SearchResultsID=ID
		FROM SearchResults
		WHERE FirstNameID=@FirstNameID
		AND LastNameID=@LastNameID
		AND MiddleInitial=@MiddleName
		AND State=@State
		AND SearchType=@SearchType

  RETURN CASE WHEN @SearchResultsID IS NOT NULL THEN 1 ELSE 0 END

END
GO
