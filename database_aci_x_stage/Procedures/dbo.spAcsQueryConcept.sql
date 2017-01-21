SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spAcsQueryConcept](@StateFips TINYINT, @CityFips INT, @ConceptCode VARCHAR(20)) AS
  SET NOCOUNT ON

  DECLARE @ApiUrl VARCHAR(MAX)
  DECLARE @ItemCodes TABLE(AcsConceptItemID INT, ItemCode VARCHAR(20))
  DECLARE @ItemCodesNeeded VARCHAR(MAX)
  DECLARE @Values TABLE(ItemCode VARCHAR(20), Value INT)


  INSERT INTO @ItemCodes(AcsConceptItemID,ItemCode)
    SELECT aci.ID'AcsConceptItemID',ItemCode
      FROM AcsConcept ac
      JOIN AcsConceptItem aci
        ON ac.ID=aci.AcsConceptID
      WHERE ac.ConceptCode=@ConceptCode
      AND IsMarginOfError=0

  SELECT @ItemCodesNeeded=dbo.fnConcatenateNoDups(ItemCode)
    FROM AcsConcept ac
    JOIN AcsConceptItem aci
      ON ac.ID=aci.AcsConceptID
    WHERE ac.ConceptCode=@ConceptCode
    AND IsMarginOfError=0
    AND NOT EXISTS (
      SELECT 1 
        FROM AcsValue av
        WHERE StateFips=@StateFips
        AND CityFips=@CityFips
        AND av.AcsConceptItemID=aci.ID
    )
    
  IF ISNULL(@ItemCodesNeeded,'')<>''
  BEGIN
    SELECT @ApiUrl = 'http://api.census.gov/data/2012/acs5'
      + '?key=479d71fea1a9507edbabf99b96d01523c1092908'
      + '&in=state:'+CONVERT(VARCHAR,@StateFips) 
      + '&for=place:'+CONVERT(VARCHAR,@CityFips)
      + '&get='+@ItemCodesNeeded
 
    INSERT INTO @Values(ItemCode,Value)
      EXEC spCensusQueryClr @ApiUrl

    INSERT INTO AcsValue(StateFips,CityFips,AcsConceptItemID,Value)
      SELECT DISTINCT @StateFips,@CityFips,aci.ID,v.Value
        FROM @Values v
        JOIN AcsConceptItem aci
          ON v.ItemCode=aci.ItemCode
        WHERE NOT EXISTS (
          SELECT 1
            FROM AcsValue av
            WHERE av.AcsConceptItemID=aci.ID
            AND StateFips=@StateFips
            AND CityFips=@CityFips
        )
  END
  SELECT *
    FROM AcsValue av
    JOIN AcsConceptItem aci
      ON aci.ID=av.AcsConceptItemID
    JOIN AcsConcept ac
      ON ac.ID=aci.AcsConceptID
    WHERE StateFips=@StateFips
    AND CityFips=@CityFips
   

GO
