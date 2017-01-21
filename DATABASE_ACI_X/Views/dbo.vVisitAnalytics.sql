SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[vVisitAnalytics] AS
	SELECT v.SiteID,
		v.ID,
		UserAgent,
		CONVERT(VARCHAR,'') 'ThemeCode',
		ISNULL(s.ShortName,CONVERT(VARCHAR,s.ID))+':'+ru.DomainName'SourceCode',
		CONVERT(VARCHAR,'')'MatchTypeCode',
		CONVERT(BIGINT,0)'ExternalCreativeID',
		dbo.fnIpAddressToString(IpAddress)'IpAddress',
		CONVERT(VARCHAR,'')'NetworkCode',
		ISNULL(SiteName,'')+':'+ISNULL(ConversionPathCode,'')'ConversionPathCode',
		CONVERT(VARCHAR,'')'QueryText',
		CONVERT(VARCHAR,'')'KeywordText',
		CONVERT(VARCHAR,'')'Placement',
		lu.Url 'LandingUrl',
		ru.Url 'Referer',
		0 'QuietMode',
		DateCreated 'VisitDate',
		DateModified,
		UseCount,
		GeoLocationID,
		0'CookiesDisabled',
		v.IsRecorded
		FROM Visit v
		JOIN vUrl ru
			ON ru.ID=v.RefererUrlID
			AND ru.SiteID=v.SiteID
		JOIN vUrl lu
			ON lu.ID=v.LandingUrlID
			AND lu.SiteID=v.SiteID
		JOIN UserAgent ua
			ON ua.ID=v.UserAgentID
		JOIN Site s
			ON s.ID=v.SiteID
		LEFT JOIN ConversionPath cp
			ON cp.SIteID=v.SiteID
			AND cp.ID=v.ConversionPathID



GO
