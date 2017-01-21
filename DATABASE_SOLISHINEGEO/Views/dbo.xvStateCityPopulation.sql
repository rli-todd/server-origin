SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[xvStateCityPopulation] WITH SCHEMABINDING AS
  SELECT gs.StateFips,gsc.CityFips,gs.StateName,gsc.CityName, Value'Population',COUNT_BIG(*)'RowCount'
    FROM dbo.GeoState gs
		JOIN dbo.GeoStateCity gsc
			ON gsc.StateFips=gs.StateFips
    JOIN dbo.AcsValue av
      ON av.StateFips=gs.StateFips
			AND av.CityFips=gsc.CityFips
    JOIN dbo.AcsConceptItem aci
      ON aci.AcsConceptItemID=av.AcsConceptItemID
    WHERE aci.ItemCode='B01003_001E'
    AND av.CountyFips=0
    GROUP BY gs.StateFips,gsc.CityFips,gs.StateName,gsc.CityName,av.Value
GO
CREATE UNIQUE CLUSTERED INDEX [IX_xvStateCityPopulation] ON [dbo].[xvStateCityPopulation] ([StateFips], [CityFips]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_xvStateCityPopulation_Population] ON [dbo].[xvStateCityPopulation] ([Population]) INCLUDE ([CityFips], [CityName], [StateFips], [StateName]) ON [PRIMARY]
GO
