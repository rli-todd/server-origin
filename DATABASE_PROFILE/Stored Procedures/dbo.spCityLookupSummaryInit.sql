SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spCityLookupSummaryInit] AS
  SET NOCOUNT ON
  TRUNCATE TABLE CityLookupSummary
  INSERT INTO CityLookupSummary(StateFips,CityFips,PersonCount)
    SELECT StateFips,CityFips,SUM(PersonCount)'PersonCount'
      FROM CityLookup
      GROUP BY StateFips,CityFips
GO
