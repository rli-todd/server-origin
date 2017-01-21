
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO



CREATE ViEW [dbo].[xvStateCityHouseholdIncome]  WITH SCHEMABINDING   AS
SELECT av.StateFips,StateAbbr,StateName,gc.CityFips,gc.CityName,aci.ItemCode,aci.Description,av.Value
  FROM dbo.AcsConcept ac
  JOIN dbo.AcsConceptItem aci
    ON aci.AcsConceptID=ac.AcsConceptID
  JOIN dbo.AcsValue av
    ON av.AcsConceptID=aci.AcsConceptID
    AND av.AcsConceptItemID=aci.AcsConceptItemID
  JOIN dbo.GeoState gs
    ON gs.StateFips=av.StateFips
  JOIN dbo.GeoCity gc
    ON gc.StateFips=gs.StateFips
    AND gc.CityFips=av.CityFips
  WHERE ConceptCode='B19001'
  AND IsMarginOfError=0
  AND av.StateFips<>0
  AND CountyFips=0
  AND av.CityFips<>0





GO

CREATE UNIQUE CLUSTERED INDEX [IX_xvStateCityHouseholdIncome] ON [dbo].[xvStateCityHouseholdIncome] ([StateFips], [CityFips], [ItemCode]) ON [PRIMARY]
GO
