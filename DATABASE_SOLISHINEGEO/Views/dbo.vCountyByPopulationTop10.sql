SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[vCountyByPopulationTop10] AS
	SELECT TOP 10 StateFips,StateName,CountyFips,CountyName,Population
		FROM xvStateCountyPopulation WITH(NOEXPAND)
		ORDER BY Population DESC
GO
