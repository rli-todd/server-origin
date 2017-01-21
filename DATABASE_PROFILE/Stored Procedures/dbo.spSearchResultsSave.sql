
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spSearchResultsSave]( 
	@SearchType INT,
	@FirstName VARCHAR(30),
	@MiddleName VARCHAR(30)='',
	@LastName VARCHAR(30),
	@State VARCHAR(2)='',
  @CompressedResults VARBINARY(MAX)=null,
	@VisitID INT=NULL,
	@NumResults INT=NULL,
	@QueryDurationMsecs INT=NULL,
  @ApiSource VARCHAR(10)='iws1.0',
  @ResultsAreEmpty BIT=0,
  @FileSize INT=NULL,
	@FullNameHits INT=NULL OUTPUT) AS
BEGIN
	SET NOCOUNT ON
	DECLARE 
		@HashKey BINARY(20),
		@FirstNameID INT,
		@LastNameID INT,
		@ExistingVisitID INT,
		@RobotID INT,
		@QueryID INT,
    @MiddleInitial CHAR(1)
	
	SET @SearchType=ISNULL(@SearchType,0)
	SET @State=UPPER(dbo.fnAlphaOnly(@State))
  SET @MiddleInitial=CONVERT(CHAR(1),UPPER(dbo.fnAlphaOnly(@MiddleName)))

	EXEC @FirstNameID=spFirstNameGet @FirstName
	EXEC @LastNameID=spLastNameGet @LastName

  IF dbo.fnIsNameRejected(@FirstName,@LastName)=1
    RETURN 0;


	SELECT TOP 1 @QueryID=ID 
		FROM SearchResults
		WHERE FirstNameID=@FirstNameID
		AND LastNameID=@LastNameID
		AND MiddleInitial=@MiddleInitial
		AND State=@State
		AND SearchType=@SearchType
    ORDER BY ID

  EXEC spPrint '@QueryID=',@QueryID
	IF @QueryID IS NOT NULL 
  BEGIN
    IF @CompressedResults IS NOT NULL
    BEGIN
	    IF EXISTS (SELECT 1 FROM IwsCache..IwsCache WITH (INDEX(IX_IwsCache)) WHERE QueryID=@QueryID)
        UPDATE IwsCache..IwsCache SET
          CompressedResults=ISNULL(@CompressedResults,CompressedResults)
          WHERE QueryID=@QueryID
      ELSE 
        INSERT INTO IwsCache..IwsCache(QueryID,CompressedResults)
          VALUES(@QueryID,@CompressedResults)
    END
  	UPDATE SearchResults	SET 
			DateCached=GETDATE(),
			VisitID=@VisitID,
			NumResults=@NumResults,
			QueryDurationMsecs=ISNULL(@QueryDurationMsecs,QueryDurationMsecs),
      ApiSource=@ApiSource,
      DirectoryType='R',
      ResultsAreEmpty=CASE WHEN @ResultsAreEmpty=0 AND @CompressedResults IS NOT NULL THEN 0 ELSE 1 END,
      FileSize=@FileSize
			WHERE ID=@QueryID
  END
	ELSE 
  BEGIN
    SELECT @QueryID=NEXT VALUE FOR QueryID
	    IF EXISTS (SELECT 1 FROM IwsCache..IwsCache WITH (INDEX(IX_IwsCache)) WHERE QueryID=@QueryID)
        UPDATE IwsCache..IwsCache SET
          CompressedResults=ISNULL(@CompressedResults,CompressedResults)
          WHERE QueryID=@QueryID
      ELSE 
        INSERT INTO IwsCache..IwsCache(QueryID,CompressedResults)
          VALUES(@QueryID,@CompressedResults)

		INSERT INTO SearchResults(ID,
			SearchType,FirstNameID,LastNameID,MiddleInitial,
			[State],DateCreated,DateCached,VisitID,DirectoryType,NumResults,QueryDurationMsecs,ApiSource,ResultsAreEmpty,FileSize) 
			VALUES (@QueryID,CONVERT(SMALLINT,@SearchType),@FirstNameID,
				@LastNameID,@MiddleInitial,
				@State,	GETDATE(),GETDATE(),@VisitID,'R',
				@NumResults,@QueryDurationMsecs,@ApiSource,CASE WHEN @ResultsAreEmpty=0 AND @CompressedResults IS NOT NULL THEN 0 ELSE 1 END,@FileSize)	
	END
  --EXEC spSearchHitRecord @QueryID

	SELECT @FullNameHits=Hits
		FROM FullName
		WHERE FirstNameID=@FirstNameID
		AND LastNameID=@LastNameID
		AND MiddleInitial=CONVERT(VARCHAR(1),@MiddleInitial)


  RETURN @QueryID	
END
GO
