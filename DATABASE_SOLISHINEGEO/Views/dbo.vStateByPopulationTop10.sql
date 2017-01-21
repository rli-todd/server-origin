SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vStateByPopulationTop10] AS
	SELECT TOP 10 StateFips,StateName,Population
		FROM xvStatePopulation WITH(NOEXPAND)
		ORDER BY Population DESC
GO
