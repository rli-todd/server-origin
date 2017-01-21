SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vApiTemplateStatsByTemplate] AS
	SELECT ApiTemplateID,Template,Method,HttpStatusCode,SUM(Hits)'Hits',SUM(TotalMsecs)'TotalMsecs',SUM(TotalMsecs)/SUM(Hits)'AvgMsecs'
		FROM vApiTemplateStats
		GROUP BY ApiTemplateID,Template,Method,HttpStatusCode

GO
