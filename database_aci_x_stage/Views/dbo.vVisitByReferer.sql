SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW vVisitByReferer AS
	SELECT CONVERT(Date,DateCreated)'DateCreated',RefererDomain,COUNT(*)'Visits',SUM(Hits)'Hits'
		FROM vVisit
		GROUP BY CONVERT(Date,DateCreated),RefererDomain

GO
