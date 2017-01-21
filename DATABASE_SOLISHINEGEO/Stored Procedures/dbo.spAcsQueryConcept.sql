SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spAcsQueryConcept](@For VARCHAR(MAX), @In VARCHAR(MAX), @ConceptCode VARCHAR(20)) AS
  SET NOCOUNT ON

  DECLARE @ApiUrl VARCHAR(MAX)
  DECLARE @ItemCodes TABLE(AcsConceptItemID INT, ItemCode VARCHAR(20))
  DECLARE @ItemCodesNeeded VARCHAR(MAX)
  DECLARE @Values TABLE(ItemCode VARCHAR(20), Value INT)


  INSERT INTO @ItemCodes(AcsConceptItemID,ItemCode)
    SELECT AcsConceptItemID,ItemCode
      FROM AcsConcept ac
      JOIN AcsConceptItem aci
        ON ac.AcsConceptID=aci.AcsConceptID
      WHERE ac.ConceptCode=@ConceptCode
      AND IsMarginOfError=0


  DELETE AcsValue
    FROM AcsValue av
    WHERE EXISTS (SELECT 1 FROM @ItemCodes ic WHERE ic.AcsConceptItemID=av.AcsConceptItemID)
    
  SELECT @ApiUrl = 'http://api.census.gov/data/2012/acs5'
    + '?key=479d71fea1a9507edbabf99b96d01523c1092908'
    + '&in=' + @In
    + '&for=' + @For
    + '&get='+dbo.fnConcatenateNoDups(ItemCode)
    FROM @ItemCodes
 
  EXEC spCensusQueryClr @ApiUrl
GO
