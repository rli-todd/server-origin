SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vVariation] AS
SELECT 
  PageCode,
  BlockName,
  b.BlockType,
  SectionName,
  s.SectionType,
  v.Description,
  MultirowPrefix,
  MultirowSuffix,
  MultirowDelimiter,
  ViewName,
  ViewFieldNames,
  HeaderTemplate,
  BodyTemplate,
  v.ID'VariationID',
  s.ID'SectionID',
  b.ID'BlockID',
  v.IsEnabled
  FROM Block b
  JOIN Section s
    ON s.BlockID=b.ID
  JOIN PageBlock pb
    ON pb.BlockID=b.ID
  JOIN Page p
    ON p.ID=pb.PageID
  JOIN Variation v
    ON v.SectionID=s.ID



GO
