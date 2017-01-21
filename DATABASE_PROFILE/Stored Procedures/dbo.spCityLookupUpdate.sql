
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spCityLookupUpdate]( @StateFips TINYINT, @CityFips INT) AS
  SET NOCOUNT ON
  DECLARE 
    @CityName VARCHAR(50),
    @StateName VARCHAR(50),
    @Rows INT,
    @PersonCount INT

  EXEC spPrint 'Started'

  DECLARE @GeoIDs TABLE(ID INT)
  --DECLARE @PersonIDs TABLE (ID INT)
	CREATE TABLE #personIDs(ID INT)
  SELECT TOP 1 @CityName=CityName, @StateName=RegionName
    FROM SolishineGeo..vGeoLocation gl
    WHERE CityFips = @CityFips
    AND StateFips = @StateFips
  EXEC spPrint @CityFips,':',@CityName, ', ', @StateFips,':',@StateName, ': CityName selected'

  INSERT INTO @GeoIDs(ID)
    SELECT ID
      FROM SolishineGeo..GeoLocation
      WHERE CityFips=@CityFips
      AND StateFips=@StateFips
  EXEC spPrint @CityFips,':',@CityName, ', ', @StateFips,':',@StateName, ': ', @@ROWCOUNT, ' GeoLocations'
  --DELETE CityLookup
  --  WHERE StateFips=@StateFips
  --  AND CityFips=@CityFips
  --EXEC spPrint @CityFips,':',@CityName, ', ', @StateFips,':',@StateName, ': ', @@ROWCOUNT, ' rows deleted'

  INSERT INTO #PersonIDs(ID)
    SELECT PersonID
      FROM PersonGeoLocation pgl
      JOIN @GeoIDs i
        ON i.ID=pgl.GeoLocationID
  
  EXEC spPrint @CityFips,':',@CityName, ', ', @StateFips,':',@StateName, ': ', @@ROWCOUNT, ' people selected'
  SELECT 
      CONVERT(VARCHAR(50),REPLACE(LastName,'''',''))'LastName',
      CONVERT(VARCHAR(50),REPLACE(FirstName,'''',''))'FirstName',
      LastNameID,FirstNameID,COUNT(*)'PersonCount'
    INTO #tcitylookup
    FROM Person p
    JOIN FirstName fn
      ON fn.ID=p.FirstNameID
    JOIN LastName ln
      ON ln.ID=p.LastNameID
		JOIN #PersonIDs pids
			ON pids.ID=p.ID
    GROUP BY LastName,FirstName,LastNameID,FirstNameID
  
  EXEC spPrint @CityFips,':',@CityName, ', ', @StateFips,':',@StateName, ': ', @@ROWCOUNT, ' #tcitylookup rows'

	CREATE INDEX #ixtcl ON #tcitylookup(LastName,FirstName)
  EXEC spPrint @CityFips,':',@CityName, ', ', @StateFips,':',@StateName, ': #tcitylookup index created'

	DELETE CityLookup
		FROM CityLookup cl
		WHERE StateFips=@StateFips
		AND CityFips=@CityFips
		AND NOT EXISTS (
			SELECT 1
				FROM #tcitylookup tcl
				WHERE tcl.FirstNameID=cl.FirstNameID
				AND tcl.LastNameID=cl.LastNameID
				AND tcl.FirstName=cl.FirstName
				AND tcl.LastName=cl.LastName
				AND tcl.PersonCount=cl.PersonCount
		)
  EXEC spPrint @CityFips,':',@CityName, ', ', @StateFips,':',@StateName, ': ', @@ROWCOUNT, ' rows deleted'

  INSERT INTO CityLookup(StateFips,CityFips,LastName,FirstName,LastNameID,FirstNameID,PersonCount)
    SELECT @StateFips,@CityFips,LastName,FirstName,LastNameID,FirstNameID,PersonCount
      FROM #tcitylookup tcl
			WHERE NOT EXISTS (
				SELECT 1
					FROM CityLookup cl
					WHERE StateFips=@StateFIps
					AND CityFips=@CityFips
					AND tcl.FirstNameID=cl.FirstNameID
					AND tcl.LastNameID=cl.LastNameID
					AND tcl.FirstName=cl.FirstName
					AND tcl.LastName=cl.LastName
					AND tcl.PersonCount=cl.PersonCount
			)
  EXEC spPrint @CityFips,':',@CityName, ', ', @StateFips,':',@StateName, ': ', @@ROWCOUNT, ' rows inserted'

  SELECT @Rows=COUNT(*), @PersonCount=SUM(PersonCount)
    FROM CityLookup
    WHERE StateFips=@StateFips
		AND CityFips=@CityFips
  EXEC spPrint @CityFips,':',@CityName,', ',@StateFips,':',@StateName, ': ', @Rows, ' rows & ', @PersonCount, ' people'
GO
