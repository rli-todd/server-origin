SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spUtilCreateStateCityCountyTables] AS
/*
** Creates distinct state, city, county and CountyCity tables from StateCountyCity
** data imported from http://www.opengeocode.org/download.ph.  These tables are then
** used to populate StateFips and CityFips in the GeoCity table, and then the StateFips,
** cityFips, and CountyFips columns in GeoLocation.
*/

CREATE TABLE _State(StateFips VARCHAR(5), StateAbbr CHAR(2))
CREATE TABLE _City(StateFips VARCHAR(5), CityFips VARCHAR(5), CityName VARCHAR(50))
CREATE TABLE _County(StateFips VARCHAR(5), CountyFips VARCHAR(5), CountyName VARCHAR(50))
CREATE TABLE _CountyCity(StateFips VARCHAR(5), CountyFips VARCHAR(5), CityFips VARCHAR(5))

INSERT INTO _State(StateAbbr,StateFips)
  SELECT DISTINCT StateAbbr,StateFips 
    FROM StateCountyCity

INSERT INTO _City(StateFips,CityFips,CityName)
  SELECT DISTINCT StateFips, CityFips, CityName
    FROM StateCountyCity

INSERT INTO _County(StateFips,CountyFips,CountyName)
  SELECT DISTINCT StateFips, CountyFips, CountyName
    FROM StateCountyCity

INSERT INTO _CountyCity(StateFips,CountyFips,CityFips)
  SELECT DISTINCT StateFips,CountyFips,CityFips
    FROM StateCountyCity



UPDATE GeoCity SET StateFips=c.StateFips
  FROM GeoCity gc
  JOIN GeoRegion gr
    ON gr.ID=gc.GeoRegionID
  JOIN _City c
    ON c.StateFips=gr.StateFips
    AND c.CityName=gc.CityName

GO
