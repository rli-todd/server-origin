
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spSearchResultsDelete]( 
	@FirstName VARCHAR(20), 
	@LastName VARCHAR(20), 
	@State CHAR(2)=NULL,
	@OptOutDate DATETIME=NULL)
AS
		SET NOCOUNT ON
    DECLARE @IDs TABLE(SearchResultsID INT)

    INSERT INTO @IDS(SearchResultsID)
			SELECT sr.ID 
				FROM SearchResults sr
				JOIN FirstName fn
					ON fn.ID=sr.FirstNameID
				JOIN LastName ln
					ON ln.ID=sr.LastNameID
				WHERE FirstName=@FirstName
				AND LastName=@LastName
				AND State=ISNULL(@State,State)

		IF EXISTS (SELECT * FROM @IDs)
    BEGIN
      SELECT FirstName,LastName,s.*
        FROM SearchResults s
        JOIN @IDs i
          ON i.SearchResultsID=s.ID
				JOIN FirstName fn
					ON fn.ID=s.FirstNameID
				JOIN LastName ln
					ON ln.ID=s.LastNameID
			SET XACT_ABORT ON
			BEGIN TRAN
				SET @OptOutDate=ISNULL(@OptOutDate,GETDATE())
				DELETE SearchResults
					FROM SearchResults sr
					JOIN @IDs i
						ON i.SearchResultsID=sr.ID

				DELETE IwsCache..IwsCache
					FROM IwsCache..IwsCache ic
					JOIN @IDs i
						ON i.SearchResultsID=ic.QueryID

				INSERT INTO OptOut(FirstName,LastName,State,OptOutDate)
					VALUES (@FirstName,@LastName,@State,@OptOutDate)
			COMMIT
    END

GO
