
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGpdBuildAll](@InitOnly BIT=0, @InitCityLookupSummary INT=1) AS
  SET NOCOUNT ON
  EXEC spGpdInit @InitCityLookupSummary
  IF @InitOnly=1
    RETURN;
  
  SELECT StateFips,SUM(PersonCount)'StatePersonCount'
    INTO #state
    FROM CityLookupSummary
    GROUP BY StateFips

  CREATE INDEX #ix_state ON #state(StateFips)
  EXEC spPrint '#state (PersonCount) created'
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  DECLARE cGeo CURSOR FOR
    -- most populous states first, and within the state, most populous cities first
    SELECT DISTINCT cls.StateFips,cls.CityFips,StateName,CityName,cls.PersonCount,StatePersonCount
    FROM SolishineGeo..GeoStateCity gsc
    JOIN CityLookupSummary cls
      ON cls.StateFips=gsc.StateFips
      AND cls.CityFips=gsc.CityFips
    JOIN #state s
      ON s.StateFips=gsc.StateFips
    ORDER BY StatePersonCount DESC,cls.PersonCount DESC
  DECLARE 
    @StateFips TINYINT, 
    @StateName VARCHAR(50),
    @CityFips INT, 
    @CityName VARCHAR(50),
    @PersonCount INT,
    @StatePersonCount INT,
    @ActiveCityCount INT,
    @SQL VARCHAR(MAX),
    @JobName VARCHAR(MAX)
  OPEN cGeo
  FETCH NEXT FROM cGeo INTO @StateFips,@CityFips,@StateName,@CityName,@PersonCount,@StatePersonCount
  WHILE @@FETCH_STATUS=0
  BEGIN
    SELECT @ActiveCityCount=COUNT(*) FROM vGpdBuildJobs WHERE StopDate IS NULL
    WHILE @ActiveCityCount>=dbo.fnGpdBuildMaxConcurrency()
    BEGIN
      WAITFOR DELAY '0:0:1'
      SELECT @ActiveCityCount=COUNT(*) FROM vGpdBuildJobs WHERE StopDate IS NULL
    END
    SET @SQL = 'EXEC PROFILE..spGpdBuild @StateFips='+CONVERT(VARCHAR,@StateFips)+',@CityFips='+CONVERT(VARCHAR,@CityFips)
    SET @JobName = 'gpd_build_'+@StateName+'_'+@CityName+'_'+CONVERT(VARCHAR,@PersonCount/1000)+'k_persons'
    EXEC sp_async_execute @SQL, @JobName
    FETCH NEXT FROM cGeo INTO @StateFips,@CityFips,@StateName,@CityName,@PersonCount,@StatePersonCount
  END
  CLOSE cGeo
  DEALLOCATE cGeo
GO
