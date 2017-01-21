SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vVisit] AS
	SELECT 
			v.ID'VisitID',v.DateCreated,DATEDIFF(minute,v.DateCreated,v.DateModified)'VisitDurationMins',UseCount'Hits',ISNULL(RobotName,'')'Robot',IsBlocked,
			SiteName,ws.ServerName'WebServer',apis.ServerName'ApiServer',
			dbo.fnIpAddressToString(IpAddress)'ClientIP',UserAgent,UtcOffsetMins,AcceptLanguage,FirstName,LastName,EmailAddress,
			lu.Path'LandingPath',
			rd.DomainName'RefererDomain'
		FROM Visit v
		JOIN Site
			ON site.ID=v.SiteID
		JOIN Server ws
			ON ws.ID=v.WebServerID
		JOIN Server apis
			ON apis.ID=v.ApiServerID
		JOIN UserAgent ua
			ON ua.ID=v.UserAgentID
		LEFT JOIN Url lu
			ON lu.ID=v.LandingUrlID
			AND lu.SiteID=v.SiteID
		LEFT JOIN Url ru
			ON ru.ID=v.RefererUrlID
			AND ru.SiteID=v.SiteID
		LEFT JOIN Domain rd
			ON rd.ID=ru.DomainID
		LEFT JOIN sg_Robot r
			ON r.ID=v.RobotID
		LEFT JOIN Users u
			ON u.ID=v.UserID



GO
