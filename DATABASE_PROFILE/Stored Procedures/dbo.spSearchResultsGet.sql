
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spSearchResultsGet]( 
	@SearchType SMALLINT,
	@FirstName VARCHAR(30),
	@MiddleName VARCHAR(30)=NULL,
	@LastName VARCHAR(30),
	@State VARCHAR(2)='',
	@VisitID INT=NULL,
	@FirstNameID INT=NULL,
	@LastNameID INT=NULL) AS
BEGIN
	SET NOCOUNT ON
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	DECLARE
		@QueryID INT,
    @CompressedResults VARBINARY(MAX),
		@MiddleInitial CHAR(1),
    @FileSize INT,
		@NameIsRejected BIT=0,
		@DirectoryType CHAR(1),
		@DirectoryID INT,
		@NumResults SMALLINT,
		@QueryDurationMsecs INT,
		@DateCreated DATETIME,
		@ApiSource VARCHAR(10),
		@ResultsAreEmpty BIT,
		@DateConsumed DATETIME,
		@ListProfileIDs VARCHAR(MAX),
		@DateCached DATETIME,
		@FullNameHits INT


	IF @FirstNameID IS NULL
		EXEC @FirstNameID=spFirstNameGet @FirstName
	
	IF @LastNameID IS NULL
		EXEC @LastNameID=spLastNameGet @LastName
	
	SET @MiddleName=UPPER(ISNULL(@MiddleName,''))
	SET @State=UPPER(RTRIM(LTRIM(ISNULL(@State,''))))
	SET @SearchType=ISNULL(@SearchType,0)

	IF ISNULL(@FirstNameID,0)=0 OR ISNULL(@LastNameID,0)=0
    SET @NameIsRejected=1

  IF /*dbo.fnIsNameRejected(@FirstName,@LastName)*/@NameIsRejected=0 
	  SELECT TOP 1
        @QueryID=ID,
				@DateCached=DateCached,
				@MiddleInitial=MiddleInitial,
				@DirectoryType=DirectoryType,
				@DirectoryID=DirectoryID,
				@NumResults=NumResults,
				@QueryDurationMsecs=QueryDurationMsecs,
				@DateCreated=DateCreated,
				@ApiSource=ApiSource,
				@ResultsAreEmpty=ResultsAreEmpty,
				@DateConsumed=DateConsumed,
				@ListProfileIDs=ListProfileIDs,
				@DateCached=DateCached,
				@FileSize=FileSize
		  FROM SearchResults
		  WHERE FirstNameID=@FirstNameID
		  AND LastNameID=@LastNameID
		  AND MiddleInitial=@MiddleName
		  AND State=@State
		  AND SearchType=@SearchType
      --AND (DateCached>DATEADD(month,-2,GETDATE()) OR @RobotID IS NOT NULL)
      AND ISNULL(ResultsAreEmpty,0)=0
      AND ISNULL(ApiSource,'') IN ('iws3.0','iws3.0+')
      ORDER BY ID

  IF @DateCached IS NOT NULL
  BEGIN
	  SELECT @CompressedResults=CompressedResults
      FROM IwsCache..IwsCache
      WHERE QueryID=@QueryID
  END

	IF @QueryID IS NOT NULL
		SELECT @FullNameHits=Hits
			FROM FullName
			WHERE FirstNameID=@FirstNameID
			AND LastNameID=@LastNameID
			AND MiddleInitial=CONVERT(VARCHAR(1),@MiddleName)
	--IF @QueryID IS NOT NULL AND @VisitID IS NOT NULL
 --   EXEC spSearchHitRecord @QueryID,@VisitID

  SELECT 
		@QueryID'QueryID',
		@SearchType'SearchType',
		@FirstNameID'FirstNameID',
		@LastNameID'LastNameID',
		@MiddleInitial'MiddleInitial',
		@State'State',
		@DirectoryType'DirectoryType',
		@DirectoryID'DirectoryID',
		@VisitID'VisitID',
		@NumResults'NumResults',
		@QueryDurationMsecs'QueryDurationMsecs',
		@DateCreated'DateCreated',
		@ApiSource'ApiSource',
		@ResultsAreEMpty'ResultsAreEmpty',
		@DateConsumed'DateConsumed',
		@ListProfileIDs'ListProfileIDs',
		@DateCached'DateCached',
		@FileSize'FileSize',
		DATALENGTH(@CompressedResults)'CompressedLength',
		@CompressedResults'CompressedResults',
		@FullNameHits'FullNameHits'
END
GO
