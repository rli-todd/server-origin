
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGpdBuildStatus] AS
  SET NOCOUNT ON
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  
  DECLARE 
    @GpdCount INT,
    @GpdTopNCount INT,
    @GpdNextLetterCount INT

  EXEC spPrint 'Started'

  SELECT @GpdCount=SUM(st.row_count) 
    FROM sys.dm_db_partition_stats st
    WHERE object_name(object_id) = 'Gpd' 
    AND (index_id < 2)
  EXEC spPrint '@GpdCount=',@GpdCount

  SELECT @GpdTopNCount=SUM(st.row_count) 
    FROM sys.dm_db_partition_stats st
    WHERE object_name(object_id) = 'GpdTopN' 
    AND (index_id < 2)
  EXEC spPrint '@GpdTopNCount=',@GpdTopNCount

  SELECT @GpdNextLetterCount=SUM(st.row_count) 
    FROM sys.dm_db_partition_stats st
    WHERE object_name(object_id) = 'GpdNextLetter' 
    AND (index_id < 2)
  EXEC spPrint '@GpdNextLetterCount=',@GpdNextLetterCount


  SELECT s.StateFips,StateName,DATEDIFF(second,DateStarted,DateCompleted)'BuildSecs',DateStarted,DateCompleted
    INTO #stats
    FROM GpdBuildStats s
    JOIN SolishineGeo..xvFips gsc
      ON s.StateFips=gsc.StateFips
      AND s.CityFips=gsc.CityFips
    WHERE DateCompleted IS NOT NULL;
  EXEC spPrint @@ROWCOUNT, ' #stats rows'
    
  SELECT 
    StateFips,
    StateName,
    COUNT(*)'Cities',
    AVG(BuildSecs)/60'AvgBuildMins',
    SUM(BuildSecs)/60'TotalBuildMins',
    DATEDIFF(minute,MIN(DateStarted),MAX(DateCompleted))'RunMins',
    MAX(DateCompleted)'LastCompleted'
    INTO #stats2
    FROM #stats
    GROUP BY StateFips,StateName
  EXEC spPrint @@ROWCOUNT, ' #stats2 rows'

  SELECT StateFips,SUM(PersonCount)'PersonCount'
    INTO #cls
    FROM CityLookupSummary
    GROUP BY StateFips
  EXEC spPrint @@ROWCOUNT, ' #cls rows'

  SELECT stats2.*,CONVERT(MONEY,PersonCount)/1000000'PersonCount (M)'
    FROM #stats2 stats2
    JOIN #cls cls
      ON stats2.StateFips=cls.StateFIps
    ORDER BY LastCompleted DESC

  SELECT v.name,v.Elapsed/60'ElapsedMins',
    CONVERT(MONEY,@GpdCount)/1000000'Pages(M)',
    CONVERT(MONEY,@GpdTopNCOunt)/1000000'TopN(M)',
    CONVERT(MONEY,@GpdNextLetterCount)/1000000'NextLetter(M)'
   FROM vGpdBuildJobs v
   WHERE StopDate IS NULL
   ORDER BY Elapsed;

  SELECT TOP 100
      StateName,
      CityName,
      PersonCount/1000'Persons(k)',
      DateCompleted,
      DATEDIFF(minute,DateStarted,DateCompleted)'ProcessingMins',
      1000*CONVERT(BIGINT,PersonCount)/DATEDIFF(millisecond,DateStarted,DateCompleted)'Persons/Sec'
    FROM GpdBuildStats s
    JOIN SolishineGeo..xvFips gsc
      ON s.StateFips=gsc.StateFips
      AND s.CityFips=gsc.CityFips
    JOIN CityLookupSummary cls
      ON cls.StateFips=s.StateFips
      AND cls.CityFips=s.CityFips
      WHERE DateCompleted IS NOT NULL
      ORDER BY DateCompleted DESC
GO
