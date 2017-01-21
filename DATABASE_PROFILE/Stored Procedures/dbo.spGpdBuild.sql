
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spGpdBuild](
  @StateFips TINYINT,
  @CityFips INT, 
  @LastName VARCHAR(50)='',
  @FirstName VARCHAR(50)=NULL, /* NULL indicates partial/wildcard lastname */
  @PersonCount INT=NULL,
	@MaxTopN INT=50,
	@Recurse BIT=1)
AS
  SET NOCOUNT ON
  DECLARE 
		@TopNCount INT,
    @LastNameID INT,
    @GpdNodeID INT,
    @SQL NVARCHAR(MAX)='',
    @LogStats BIT=0,
    @DateStarted DATETIME=GETDATE(),
    @MSecs INT

  DECLARE @SqlTopN TABLE(SQL NVARCHAR(MAX))
  DECLARE @SqlNextLetter TABLE(SQL NVARCHAR(MAX))


  IF @Recurse=1 AND @LastName='' AND @FirstName IS NULL
  BEGIN
    -- Only log statistics at the top level for a city
    SET @LogStats=1
    IF EXISTS (
      SELECT 1
        FROM GpdBuildStats
        WHERE StateFips=@StateFips
        AND CityFips=@CityFips
    )
      UPDATE GpdBuildStats SET DateStarted=GETDATE()
        WHERE StateFips=@StateFips
        AND CityFips=@CityFips
    ELSE 
      INSERT INTO GpdBuildStats(StateFips,CityFips,DateStarted)
        VALUES (@StateFips,@CityFips,GETDATE())
  END

  DECLARE @TopN TABLE(Name VARCHAR(50), PersonCount INT)
  IF @FirstName IS NULL
  BEGIN
    -- partial/wildcard lastname

    -- Look for an existing directory node
    SELECT @GpdNodeID=ID
      FROM GpdNode
      WHERE StateFips=@StateFips
      AND CityFips=@CityFips
      AND LastName=@LastName
      AND FirstName IS NULL

    SET ROWCOUNT @MaxTopN
		INSERT INTO @TopN(Name,PersonCount)
      SELECT 
        LastName,SUM(PersonCount)
        FROM CityLookup cl
        WHERE cl.StateFips=@StateFips
        AND cl.CityFips=@CityFips
        AND cl.LastName LIKE @LastName + '%'
        GROUP BY LastName
        ORDER BY SUM(PersonCount) DESC
    SET @TopNCount=@@ROWCOUNT
		SET ROWCOUNT 0
  END
  ELSE
  BEGIN
    -- partial/wildcard firstname

    -- Look for an existing directory node
    SELECT @GpdNodeID=ID
      FROM GpdNode
      WHERE StateFips=@StateFips
      AND CityFips=@CityFips
      AND LastName=@LastName
      AND FirstName=@FirstName

		SET ROWCOUNT @MaxTopN
    INSERT INTO @TopN(Name,PersonCount)
      SELECT
        FirstName,SUM(PersonCount)
        FROM CityLookup cl
        WHERE cl.StateFips=@StateFips
        AND cl.CityFips=@CityFips
        AND cl.LastName=@LastName
        AND cl.FirstName LIKE @FirstName + '%'
        GROUP BY LastName,FirstName
        ORDER BY SUM(PersonCount) DESC
    SET @TopNCount=@@ROWCOUNT
		SET ROWCOUNT 0
  END

  IF @GpdNodeID IS NOT NULL
  BEGIN
    DELETE GpdTopN
      WHERE GpdNodeID=@GpdNodeID
    DELETE GpdNextLetter
      WHERE GpdNodeID=@GpdNodeID
  END
  ELSE
  BEGIN
    INSERT INTO GpdNode(StateFips,CityFips,LastName,FirstName)
      VALUES (@StateFips,@CityFips,@LastName,@FirstName)
    SET @GpdNodeID=SCOPE_IDENTITY()
  END

  INSERT INTO GpdTopN(GpdNodeID,Name,PersonCount)
    SELECT @GpdNodeID,Name,PersonCount
      FROM @TopN

  IF @FIrstName IS NULL
  BEGIN
    -- wildcard lastname
    INSERT INTO GpdNextLetter(GpdNodeID,NextLetter,PersonCount)
      SELECT @GpdNodeID, SUBSTRING(LastName,LEN(@LastName)+1,1),SUM(PersonCount)
        FROM CityLookup cl
        WHERE cl.StateFips=@StateFips
        AND cl.CityFips=@CityFips
        AND cl.LastName LIKE @LastName + '%'
        AND LEN(LastName)>LEN(@LastName)
        AND NOT EXISTS(
          SELECT 1
            FROM GpdTopN t
            WHERE GpdNodeID=@GpdNodeID
            AND t.Name=cl.LastName
        )
        GROUP BY SUBSTRING(LastName,LEN(@LastName)+1,1)
  END
  ELSE
  BEGIN
    -- wildcard firstname
    INSERT INTO GpdNextLetter(GpdNodeID,NextLetter,PersonCount)
      SELECT @GpdNodeID, SUBSTRING(FirstName,LEN(@FirstName)+1,1),SUM(PersonCount)
        FROM CityLookup cl
        WHERE cl.StateFips=@StateFips
        AND cl.CityFips=@CityFips
        AND cl.LastName=@LastName
        AND cl.FirstName LIKE @FirstName + '%'
        AND LEN(FirstName)>LEN(@FirstName)
        AND NOT EXISTS(
          SELECT 1
            FROM GpdTopN t
            WHERE GpdNodeID=@GpdNodeID
            AND t.Name=cl.FirstName
        )
        GROUP BY SUBSTRING(FirstName,LEN(@FirstName)+1,1)
  END

	IF @Recurse=0
		RETURN;

  /*
  ** Now decide whether we need to recurse
  */
  IF @FIrstName IS NULL
  BEGIN
    -- we have less than @TopN last names, so we're going to 
    -- recurse for each of them with a non-null, empty firstname
    INSERT INTO @SqlTopN(SQL)
      SELECT '
        EXEC spGpdBuild @StateFips='+CONVERT(NVARCHAR,@StateFips)+
          ', @CityFips='+CONVERT(NVARCHAR,@CityFips)+
          ', @LastName='''+REPLACE(Name,'''','''''')+''''+
          ', @FirstName='''''+
          ', @PersonCount='+CONVERT(NVARCHAR(MAX),PersonCount)
        FROM @TopN t
        ORDER BY PersonCount DESC
    SELECT @SQL=@SQL+SQL FROM @SqlTopN
  END

  IF @TopNCount >= @MaxTopN
  BEGIN
    -- recursion is necessary
    IF @FirstName IS NULL 
    BEGIN
    -- Was wildcard lastname, and we need to recurse to the next lastname level
      INSERT INTO @SqlNextLetter(SQL)
        SELECT N'
          EXEC spGpdBuild @StateFips='+CONVERT(NVARCHAR,@StateFips)+
            ', @CityFips='+CONVERT(NVARCHAR,@CityFips)+
            ', @LastName='''+REPLACE(ISNULL(@LastName,''),'''','''''')+ISNULL(NextLetter,'')+''''+
            ', @FirstName=NULL'+
            ', @PersonCount='+CONVERT(NVARCHAR(MAX),ISNULL(PersonCount,0))+';'
          FROM GpdNextLetter
          WHERE GpdNodeID=@GpdNodeID
          ORDER BY PersonCount DESC
    END
    ELSE
    BEGIN
    -- Was wildcard firstname with specific last name, and we need to recurse to the next firstname level
      INSERT INTO @SqlNextLetter(SQL)
        SELECT '
          EXEC spGpdBuild @StateFips='+CONVERT(NVARCHAR,@StateFips)+
            ', @CityFips='+CONVERT(NVARCHAR,@CityFips)+
            ', @LastName='''+REPLACE(@LastName,'''','''''')+''''+
            ', @FirstName='''+REPLACE(@FirstName,'''','''''')+NextLetter+''''+
            ', @PersonCount='+CONVERT(NVARCHAR(MAX),PersonCount)
          FROM GpdNextLetter
          WHERE GpdNodeID=@GpdNodeID
          ORDER BY PersonCount DESC
    END
    SELECT @SQL=@SQL+SQL FROM @SqlNextLetter
  END

  IF @SQL<>''
  BEGIN
    EXEC (@SQL)
  END
  SET @MSecs=DATEDIFF(millisecond,@DateStarted,GETDATE())
  EXEC spPrint 'Completed spGpdBuild @StateFips=', @StateFips,', @CityFips=', @CityFips, ' in ', @MSecs, ' msecs: ',
    @StateFips, ',', 
    @CityFips,',',
    @LastName,',',
    @FirstName,',',
    @PersonCount
  IF @LogStats=1
  BEGIN
    DECLARE 
      @TopNPersonCount INT, 
      @NExtLetterPersonCount INT

    SELECT @TopNPersonCount=SUM(PersonCount)
      FROM @TopN

    SELECT @NextLetterPersonCount=SUM(PersonCount)
      FROM GpdNextLetter
      WHERE GpdNodeID=@GpdNodeID

    UPDATE GpdBuildStats SET 
        DateCompleted=GETDATE(),
        TopNPersonCount=ISNULL(@TopNPersonCount,0),
        NextLetterPersonCount=ISNULL(@NextLetterPersonCount,0)
      WHERE StateFips=@StateFips
      AND CityFips=@CityFips
  END

GO
