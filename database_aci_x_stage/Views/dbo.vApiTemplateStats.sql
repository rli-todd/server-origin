SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW vApiTemplateStats AS
	SELECT ApiTemplateID,Template,LogPeriod,dbo.fnIpAddressToString(ClientIP)'ClientIP',Method,HttpStatusCode,Hits,TotalMsecs,TotalMsecs/Hits'AvgMsecs'
		FROM ApiTemplateStats ats
		JOIN ApiTemplate at
			ON at.ID=ats.APiTemplateID
GO
