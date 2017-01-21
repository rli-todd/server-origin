
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE VIEW [dbo].[xvStatePopulation] WITH SCHEMABINDING AS
  SELECT gs.StateFips,gs.StateName,Value'Population',COUNT_BIG(*)'RowCount'
    FROM dbo.GeoState gs
    JOIN dbo.AcsValue av
      ON av.StateFips=gs.StateFips
    JOIN dbo.AcsConceptItem aci
      ON aci.AcsConceptItemID=av.AcsConceptItemID
    WHERE aci.ItemCode='B01003_001E'
    AND av.CityFips=0
    AND av.CountyFips=0
    GROUP BY gs.StateFips,gs.StateName,av.Value


GO

CREATE UNIQUE CLUSTERED INDEX [IX_xvStatePopulation] ON [dbo].[xvStatePopulation] ([StateFips]) ON [PRIMARY]

CREATE NONCLUSTERED INDEX [IX_xvStatePopulation_Population] ON [dbo].[xvStatePopulation] ([Population]) INCLUDE ([StateFips], [StateName]) ON [PRIMARY]
GO
