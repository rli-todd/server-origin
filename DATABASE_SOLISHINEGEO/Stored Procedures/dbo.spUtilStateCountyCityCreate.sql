SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spUtilStateCountyCityCreate] AS
/*

040 = State 
050 = County 
061 = Minor Civil Division 
071 = Minor Civil Division place part 
157 = County place part 
162 = Incorporated place 
170 = Consolidated city 
172 = Consolidated city -- place within consolidated city

*/

DECLARE @Suffix TABLE(Suffix VARCHAR(50))
DECLARE @Prefix TABLE(Prefix VARCHAR(50))
INSERT INTO @Prefix(Prefix) VALUES
  ('Balance of')
INSERT INTO @Suffix(Suffix) VALUES
  (' (balance)'),
  (' city'),
  (' city (pt.)'),
  (' city (balance)'),
  (' city (balance) (pt.)'),
  (' town'),
  (' town (pt.)'),
  (' plantation'),
  (' township'),
  (' village'),
  (' village (pt.)'),
  (' borough'),
  (' borough (pt.)'),
  (' urban county'),
  (' municipality'),
  (' purchase'),
  (' grant'),
  (' location'),
  (' UT'),
  (' metro government'),
  (' metro government (balance)'),
  (' unified government'),
  (' consolidated government');

WITH state 
AS (
  SELECT 
    State'StateFips',
    StName'StateName'
    FROM [SUB-EST2012]
    WHERE SumLev='040'
),
county
AS (
  SELECT 
    State'StateFips',
    County'CountyFips',
    Name'CountyName'
    FROM [Sub-est2012]
    WHERE SumLev='050'
),
pre AS
(
  SELECT State'StateFips',County'CountyFips',Place,Cousub,Suffix,
      Census2010Pop,EstimatesBase2010,PopEstimate2010,PopEstimate2011,PopEstimate2012,
    CASE 
      WHEN Prefix IS NOT NULL
        THEN SUBSTRING(Name,LEN(Prefix)+2,LEN(Name)-(LEN(Prefix)+1))
      ELSE Name
    END'ModifiedName',
    CASE 
      WHEN Prefix IS NOT NULL
        THEN SUBSTRING(Prefix,1,LEN(Prefix))
      ELSE '' 
    END'Type'
    FROM [Sub-est2012] place
    --COLLATE Latin1_General_CS_AS 
    LEFT JOIN @Suffix s
      ON place.Name LIKE '%'+Suffix
    LEFT JOIN @Prefix p
      ON place.Name LIKE Prefix+' %'
    WHERE place.SumLev NOT IN ('040','050')
),
places AS
(
  SELECT pre.StateFips,pre.COuntyFips,pre.Place,pre.CouSub,
    Census2010Pop,EstimatesBase2010,PopEstimate2010,PopEstimate2011,PopEstimate2012,
    CASE 
      WHEN Suffix IS NOT NULL
        THEN SUBSTRING(ModifiedName,1,LEN(ModifiedNAME)-LEN(Suffix))
      ELSE ModifiedName
    END'ModifiedName',
    CASE 
      WHEN Suffix IS NOT NULL 
        THEN SUBSTRING(Suffix,2,LEN(Suffix)-1) +','+pre.Type
      ELSE pre.Type 
    END'Type'
    FROM pre
)
  SELECT p.StateFips,s.StateName,p.CountyFips,c.CountyName,p.ModifiedName'CityName',COUNT(*)'rows',NULL'GeoLocationID'
  INTO StateCountyCity
  FROM places p
  JOIN State s
    ON s.StateFips=p.StateFips
  JOIN County c
    ON c.StateFips=p.StateFips
    AND c.CountyFips=p.COuntyFips
  WHERE ModifiedName NOT LIKE '% County'
  GROUP BY p.StateFips,s.StateName,p.COuntyFips,c.COuntyName,p.ModifiedName
  ORDER BY p.StateFips,s.StateName,p.COuntyFips,c.COuntyName,p.ModifiedName


UPDATE StateCountyCity
  SET GeoLocationID=gl.ID
  FROM StateCountyCity scc
  JOIN GeoRegion gr
    ON gr.RegionName=scc.StateName
  JOIN GeoCity gc
    ON gc.GeoRegionID=gr.ID
    AND gc.CityName=scc.CityName
  JOIN GeoLocation gl
    ON gl.GeoCityID=gc.ID
    
GO
