SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW vApiTemplateStatsByHour AS
	SELECT ApiTemplateID,Template,dbo.fnDateHour(LogPeriod)'LogPeriod',ClientIP,Method,HttpStatusCode,SUM(Hits)'Hits',SUM(TotalMsecs)'TotalMsecs',SUM(TotalMsecs)/SUM(Hits)'AvgMsecs'
		FROM vApiTemplateStats
		GROUP BY ApiTemplateID,Template,dbo.fnDateHour(LogPeriod),ClientIP,Method,HttpStatusCode
GO
