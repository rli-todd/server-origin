SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[xvStateCountyPopulation] WITH SCHEMABINDING AS
  SELECT gs.StateFips,gsc.CountyFips,gs.StateName,gsc.CountyName, Value'Population',COUNT_BIG(*)'RowCount'
    FROM dbo.GeoState gs
		JOIN dbo.GeoStateCounty gsc
			ON gsc.StateFips=gs.StateFips
    JOIN dbo.AcsValue av
      ON av.StateFips=gs.StateFips
			AND av.CountyFips=gsc.CountyFips
    JOIN dbo.AcsConceptItem aci
      ON aci.AcsConceptItemID=av.AcsConceptItemID
    WHERE aci.ItemCode='B01003_001E'
    AND av.CityFips=0
    GROUP BY gs.StateFips,gsc.CountyFips,gs.StateName,gsc.CountyName,av.Value

GO
CREATE UNIQUE CLUSTERED INDEX [IX_xvStateCountyPopulation] ON [dbo].[xvStateCountyPopulation] ([StateFips], [CountyFips]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_xvStateCountyPopulation_Population] ON [dbo].[xvStateCountyPopulation] ([Population]) INCLUDE ([CountyFips], [CountyName], [StateFips], [StateName]) ON [PRIMARY]
GO
