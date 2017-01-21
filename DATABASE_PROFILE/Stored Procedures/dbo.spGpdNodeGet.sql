
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGpdNodeGet](
  @StateFips TINYINT=NULL,
  @CityFips INT=NULL,
  @StateName VARCHAR(50)=NULL,
  @CityName VARCHAR(50)=NULL,
  @LastName VARCHAR(50),
  @FirstName VARCHAR(50)=NULL)
AS
  SET NOCOUNT ON
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  DECLARE 
    @GpdNodeID INT

  IF @StateFips IS NULL OR @CityFips IS NULL
    SELECT @StateFips=StateFips, @CityFips=CityFips
      FROM GeoStateCity
      WHERE StateName=@StateName
      AND CityName=@CityName

  IF @FirstName IS NULL
    SELECT @GpdNodeID=ID
      FROM GpdNode
      WHERE StateFips=@StateFips
      AND CityFips=@CityFips
      AND LastName=@LastName
      AND FirstName IS NULL
  ELSE
    SELECT @GpdNodeID=ID
      FROM GpdNode
      WHERE StateFips=@StateFips
      AND CityFips=@CityFips
      AND LastName=@LastName
      AND FirstName=@FirstName

	IF @GpdNodeID IS NULL
	BEGIN
		EXEC spGpdBuild
			@StateFips=@StateFips,
			@CityFips=@CityFips,
			@LastName=@LastName,
			@FirstName=@FirstName,
			@Recurse=0

		IF @FirstName IS NULL
			SELECT @GpdNodeID=ID
				FROM GpdNode
				WHERE StateFips=@StateFips
				AND CityFips=@CityFips
				AND LastName=@LastName
				AND FirstName IS NULL
		ELSE
			SELECT @GpdNodeID=ID
				FROM GpdNode
				WHERE StateFips=@StateFips
				AND CityFips=@CityFips
				AND LastName=@LastName
				AND FirstName=@FirstName
	END

  SELECT Name,PersonCount
    FROM GpdTopN t
    WHERE GpdNodeID=@GpdNodeID
    ORDER BY Name

  DECLARE @NextLetters VARCHAR(50)=''
  SELECT @NextLetters=@NextLetters+NextLetter
    FROM GpdNextLetter n
    WHERE GpdNodeID=@GpdNodeID
    ORDER BY NextLetter

  SELECT LOWER(@NextLetters)'NextLetters'
GO

GRANT EXECUTE ON  [dbo].[spGpdNodeGet] TO [api_dev]
GO
