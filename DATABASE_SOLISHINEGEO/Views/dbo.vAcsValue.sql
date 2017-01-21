SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vAcsValue] AS
SELECT TOP 10000 
  ConceptCode,ConceptName,ItemCode,Description, 
  COUNT(*)'RowCount',SUM(CONVERT(BIGINT,av.Value))'SumValue',AVG(CONVERT(BIGINT,av.Value))'AvgValue'
  FROM vAcsConcept ac
  JOIN AcsValue av
    ON ac.AcsConceptID=av.AcsConceptID
    AND ac.AcsConceptItemID=av.AcsConceptItemID
  GROUP BY ConceptCode,ConceptName, ItemCode,Description
  ORDER BY ConceptCode,ItemCode

GO
