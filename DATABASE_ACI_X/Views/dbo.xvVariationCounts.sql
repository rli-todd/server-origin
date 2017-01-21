SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO



CREATE VIEW [dbo].[xvVariationCounts] WITH SCHEMABINDING  AS
  SELECT BlockID,SectionID,BlockName,SectionName,s.SectionType,b.BlockType,COUNT_BIG(*)'VariationCount'
    FROM dbo.Block b
    JOIN dbo.Section s
      ON b.ID=s.BlockID
    JOIN dbo.Variation v
      ON s.ID=v.SectionID
    WHERE b.IsEnabled=1
    AND s.IsEnabled=1
    AND v.IsEnabled=1
    GROUP BY BlockID,SectionID,BlockName,SectionName,SectionType,BlockType






GO
SET ANSI_PADDING ON
GO
CREATE UNIQUE CLUSTERED INDEX [IX_xvVariationCounts]
	ON [dbo].[xvVariationCounts] ([BlockID], [SectionID])
	WITH ( PAD_INDEX = ON, FILLFACTOR = 80)
	ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
