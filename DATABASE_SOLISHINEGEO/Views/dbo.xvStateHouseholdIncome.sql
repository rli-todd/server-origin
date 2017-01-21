SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE ViEW [dbo].[xvStateHouseholdIncome] WITH SCHEMABINDING AS
SELECT av.StateFips,StateAbbr,StateName,aci.ItemCode,aci.Description,av.Value
  FROM dbo.AcsConcept ac
  JOIN dbo.AcsConceptItem aci
    ON aci.AcsConceptID=ac.AcsConceptID
  JOIN dbo.AcsValue av
    ON av.AcsConceptID=aci.AcsConceptID
    AND av.AcsConceptItemID=aci.AcsConceptItemID
  JOIN dbo.GeoState gs
    ON gs.StateFips=av.StateFips
  WHERE ConceptCode='B19001'
  AND IsMarginOfError=0
  AND av.StateFips<>0
  AND CountyFips=0
  AND CityFips=0

GO
CREATE UNIQUE CLUSTERED INDEX [IX_xvStateHouseholdIncome] ON [dbo].[xvStateHouseholdIncome] ([StateFips], [ItemCode]) WITH (FILLFACTOR=80, PAD_INDEX=ON) ON [PRIMARY]
GO
