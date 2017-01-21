SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW vUrl AS
	SELECT SiteID,u.ID,d.DomainName,
		'http' + CASE ISNULL(IsHttps,0) WHEN 0 THEN '' ELSE 's' END + 
		'://' + sd.DomainName + '.' + d.DomainName + Path + 
		CASE WHEN ISNULL(QueryString,'')='' THEN '' ELSE '?' + QueryString END 'Url'
		FROM Url u
		JOIN Domain d
			ON d.ID=u.DOmainID
		JOIN Domain sd
			ON sd.ID=u.SubDomainID
GO
