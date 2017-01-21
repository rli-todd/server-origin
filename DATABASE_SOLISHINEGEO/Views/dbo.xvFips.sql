
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO







CREATE VIEW [dbo].[xvFips]  WITH SCHEMABINDING  AS
SELECT 
  CONVERT(TINYINT,ISNULL(gr.StateFips,ISNULL(gc.StateFips,ISNULL(gl.StateFips,0))))'StateFips',
  CONVERT(INT,ISNULL(gc.CityFips,ISNULL(gl.CityFips,0)))'CityFips',
  RegionCode'StateAbbr',
  RegionName'StateName',
  CityName,
  --AvgLatitude,
  --AvgLongitude,
  COUNT_BIG(*)'RowCount'
  FROM dbo.GeoLocation gl
  JOIN dbo.GeoCity gc
    ON gl.GeoCityID=gc.ID
  JOIN dbo.GeoRegion gr
    ON gr.ID=gc.GeoRegionID
  WHERE ISNULL(gr.StateFips,ISNULL(gc.StateFips,ISNULL(gl.StateFips,0)))<>0
  AND ISNULL(gc.CityFips,ISNULL(gl.CityFips,0))<>0
  GROUP BY 
    CONVERT(TINYINT,ISNULL(gr.StateFips,ISNULL(gc.StateFips,ISNULL(gl.StateFips,0)))),
    CONVERT(INT,ISNULL(gc.CityFips,ISNULL(gl.CityFips,0))),
    RegionCode,RegionName,CityName
    --,AvgLatitude,AvgLongitude



GO

CREATE UNIQUE CLUSTERED INDEX [IX_xvFips] ON [dbo].[xvFips] ([StateFips], [CityFips]) ON [PRIMARY]
GO
