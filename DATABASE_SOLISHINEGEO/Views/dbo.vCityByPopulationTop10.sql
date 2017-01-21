SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vCityByPopulationTop10] AS
	SELECT TOP 10 StateFips,StateName,CityFips,CityName,Population
		FROM xvStateCityPopulation WITH(NOEXPAND)
		ORDER BY Population DESC
GO
