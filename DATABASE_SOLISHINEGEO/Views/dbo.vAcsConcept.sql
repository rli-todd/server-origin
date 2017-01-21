SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vAcsConcept] AS
  SELECT ac.AcsConceptID,ConceptCode,ConceptName,
          AcsConceptItemID,ItemCode,Description,SubDescription,SubDescription2
    FROM AcsConcept ac
    JOIN AcsConceptItem aci
      ON aci.AcsConceptID=ac.AcsConceptID
    WHERE IsMarginOfError=0
GO
