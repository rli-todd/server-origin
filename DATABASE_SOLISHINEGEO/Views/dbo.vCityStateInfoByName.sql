SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
** We would like to make this an indexed-view, but are unable to do so because
** we need an expression on the result of an aggregate operation on a grouped column.
** So instead, we will simply select the entirety of this view into a new table,
** CityStateInfoByName, and put an index on that.    
*/
CREATE VIEW [dbo].[vCityStateInfoByName] WITH SCHEMABINDING AS
  SELECT 
			gsc.CityName+', ' + gsc.StateAbbr'CityAndState', 
			gsc.StateName,
			gsc.CityName,
			gsc.StateNormalized,
			gsc.CityNormalized,
			CountyName,
			gsc.StateFips,
			gsc.CityFips,
			Value'Population',
			--COUNT_BIG(*)'RowCount',
			CourtName,
			CourtLevel,
			Circuit,
			MainOffice,
			LocationAddress1'CourtAddress1',
			LocationAddress2'CourtAddress2',
			LocationCity'CourtCity',
			LocationState'CourtState',
			LocationZip'CourtZip',
			Phone'CourtPhone',
			Url'CourtUrl',
			Miles'MilesToCourt'
    FROM dbo.GeoStateCity gsc
    JOIN dbo.AcsValue av
      ON av.StateFips=gsc.StateFips
			AND av.CityFips=gsc.CityFips
    JOIN dbo.AcsConceptItem aci
      ON aci.AcsConceptItemID=av.AcsConceptItemID
		JOIN dbo.GeoStateCityFederalCircuitCourtsTopN c
			ON c.StateFips=gsc.StateFips
			AND c.CityFips=gsc.CityFips
			AND RowNum=1
		JOIN dbo.GeoStateCountyCity gscc
			ON gscc.StateFips=gsc.StateFips
			AND gscc.CityFips=gsc.CityFips
    WHERE aci.ItemCode='B01003_001E'
    AND av.CountyFips=0
    GROUP BY 
			gsc.StateFips,
			gsc.CityFips,
			gsc.StateAbbr,
			gsc.StateName,
			gsc.CityName,
			gsc.StateNormalized,
			gsc.CityNormalized,
			av.Value,
			CourtName,
			CourtLevel,
			Circuit,
			MainOffice,
			LocationAddress1,
			LocationAddress2,
			LocationCity,
			LocationState,
			LocationZip,
			Phone,
			Url,
			Miles,
			CountyName

GO
