SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spAcsPopulate](@ConceptCode VARCHAR(20), @StateLevel BIT=1, @StateCountyLevel BIT=1, @StatePlaceLevel BIT=1) AS
  SET NOCOUNT ON
  DECLARE 
    @ApiKey VARCHAR(MAX)='479d71fea1a9507edbabf99b96d01523c1092908',
    @ApiUrl VARCHAR(MAX)= 'http://api.census.gov/data/2012/acs5',
    @MeasureNames VARCHAR(MAX),
    @StateFips TINYINT,
    @StateName VARCHAR(20),
    @InState VARCHAR(20),
    @For VARCHAR(20)

  DECLARE @State TABLE(StateFips TINYINT, MeasureName VARCHAR(20), MeasureValue INT )
  DECLARE @StatePlace TABLE(StateFips TINYINT, PlaceFips INT, MeasureName VARCHAR(20), MeasureValue INT)
  DECLARE @StateCounty TABLE(StateFips TINYINT, CountyFips SMALLINT, MeasureName VARCHAR(20), MeasureValue INT)
  DECLARE @National TABLE(USFips TINYINT, MeasureName VARCHAR(20), MeasureValue INT)

  SELECT AcsConceptItemID,ItemCode,ac.AcsConceptID
    INTO #itemCodes
    FROM AcsConcept ac
    JOIN AcsConceptItem aci
      ON ac.AcsConceptID=aci.AcsConceptID
    WHERE ac.ConceptCode=@ConceptCode
    AND IsMarginOfError=0

  CREATE UNIQUE CLUSTERED INDEX #ix_itemCode ON #itemCodes(ItemCode)
  CREATE INDEX #ix_itemCode2 ON #itemCodes(AcsConceptItemID)

  SELECT @MeasureNames=dbo.fnConcatenateNoDups(ItemCode)
    FROM #Itemcodes
  EXEC spPrint 'MeasureNames: ', @MeasureNames

  DELETE AcsValue
    FROM AcsValue av
    WHERE EXISTS (SELECT 1 FROM #ItemCodes ic WHERE ic.AcsConceptItemID=av.AcsConceptItemID)
    AND StateFips=0
    AND CityFips=0
    AND CountyFips=0
  EXEC spPrint @@ROWCOUNT, ' AcsValue rows deleted (National)'

  IF @StateLevel=1
  BEGIN
    DELETE AcsValue
      FROM AcsValue av
      WHERE EXISTS (SELECT 1 FROM #ItemCodes ic WHERE ic.AcsConceptItemID=av.AcsConceptItemID)
      AND StateFips<>0
      AND CityFips=0
      AND CountyFips=0
    EXEC spPrint @@ROWCOUNT, ' AcsValue rows deleted (State)'
  END

  IF @StateCountyLevel=1
  BEGIN
    DELETE AcsValue
      FROM AcsValue av
      WHERE EXISTS (SELECT 1 FROM #ItemCodes ic WHERE ic.AcsConceptItemID=av.AcsConceptItemID)
      AND StateFips<>0
      AND CityFips=0
      AND CountyFips<>0
    EXEC spPrint @@ROWCOUNT, ' AcsValue rows deleted (StateCounty)'
  END

  IF @StatePlaceLevel=1
  BEGIN
    DELETE AcsValue
      FROM AcsValue av
      WHERE EXISTS (SELECT 1 FROM #ItemCodes ic WHERE ic.AcsConceptItemID=av.AcsConceptItemID)
      AND StateFips<>0
      AND CityFips<>0
      AND CountyFips=0
    EXEC spPrint @@ROWCOUNT, ' AcsValue rows deleted (StateCounty)'
  END

  INSERT INTO @National(USFips, MeasureName, MeasureValue)
    EXEC spCensusQueryAcsClr 
      @ApiKey=@ApiKey,
      @BaseUrl=@ApiUrl,
      @For='us:1',
      @Get=@MeasureNames

  INSERT INTO AcsValue(StateFips,CountyFips,CityFips,AcsConceptID,AcsConceptItemID,Value)
    SELECT 0,0,0,AcsConceptID,AcsConceptItemID,n.MeasureValue
      FROM @National n
      JOIN #itemCodes ic
        ON n.MeasureName=ic.ItemCode

  EXEC spPrint 'MeasureNames: ', @MeasureNames
  /*
  ** Now we'll go through each state and get all the city(place) and county values
  */
  DECLARE cState CURSOR FOR SELECT StateFips, StateName FROM GeoState
  OPEN cState
  FETCH NEXT FROM cState INTO @StateFips, @StateName
  WHILE @@FETCH_STATUS=0 BEGIN
    EXEC spPrint @StateName

    IF @StateLevel=1
    BEGIN
      -- state 
      SET @For='state:'+CONVERT(VARCHAR,@StateFips)
      SET @InState = NULL
      EXEC spPrint @StateName, ': @For=', @For, ', @InState=', @InState
      INSERT INTO @State(StateFips,MeasureName, MeasureValue)
        EXEC spCensusQueryAcsClr 
          @ApiKey=@ApiKey,
          @BaseUrl=@ApiUrl,
          @For=@For,
          @In=@InState,
          @Get=@MeasureNames
      INSERT INTO AcsValue(StateFips,CountyFips,CityFips,AcsConceptID,AcsConceptItemID,Value)
        SELECT s.StateFips,0,0,AcsConceptID,AcsConceptItemID,s.MeasureValue
          FROM @State s
          JOIN #itemCodes ic
            ON s.MeasureName=ic.ItemCode
      EXEC spPrint @StateName, ': ', @@ROWCOUNT, ' state measure count inserted'
      DELETE @State
    END

    IF @StateCountyLevel=1
    BEGIN
      -- state county
      SET @InState = 'state:'+CONVERT(VARCHAR,@StateFips)
      SET @For='county:*'
      EXEC spPrint @StateName, ': @For=', @For, ', @InState=', @InState
      INSERT INTO @StateCounty(StateFips,CountyFips, MeasureName, MeasureValue)
        EXEC spCensusQueryAcsClr 
          @ApiKey=@ApiKey,
          @BaseUrl=@ApiUrl,
          @For=@For,
          @In=@InState,
          @Get=@MeasureNames
      INSERT INTO AcsValue(StateFips,CountyFips,CityFips,AcsConceptID,AcsConceptItemID,Value)
        SELECT sc.StateFips,sc.CountyFips,0,AcsConceptID,AcsConceptItemID,sc.MeasureValue
          FROM @StateCounty sc
          JOIN #itemCodes ic
            ON sc.MeasureName=ic.ItemCode
      EXEC spPrint @StateName, ': ', @@ROWCOUNT, ' county measure count inserted'
      DELETE @StateCounty
    END

    IF @StatePlaceLevel=1
    BEGIN
      SET @For='place:*'
      EXEC spPrint @StateName, ': @For=', @For, ', @InState=', @InState
      INSERT INTO @StatePlace(StateFips, PlaceFips, MeasureName, MeasureValue)
        EXEC spCensusQueryAcsClr 
          @ApiKey=@ApiKey,
          @BaseUrl=@ApiUrl,
          @For=@For,
          @In=@InState,
          @Get=@MeasureNames

      INSERT INTO AcsValue(StateFips,CountyFips,CityFips,AcsConceptID,AcsConceptItemID,Value)
        SELECT sp.StateFips,0,sp.PlaceFips,AcsConceptID,AcsConceptItemID,sp.MeasureValue
          FROM @StatePlace sp
          JOIN #itemCodes ic
            ON sp.MeasureName=ic.ItemCode

      EXEC spPrint @StateName, ': ', @@ROWCOUNT, ' place measure values inserted'
      DELETE @StatePlace
    END    
    FETCH NEXT FROM cState INTO @StateFips, @StateName 
  END
  CLOSE cState
  DEALLOCATE cState

GO
