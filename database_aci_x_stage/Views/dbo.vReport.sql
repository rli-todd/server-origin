SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vReport] AS 
SELECT 
		r.ID'ReportID',r.ReportDate,r.SiteID,r.UserID,oi.OrderID,r.OrderItemID,r.ReportTypeCode,r.QueryID,
		r.ProfileID,fn.FirstName,sr.MiddleInitial,ln.LastName,sr.State,
		JsonLen,DATALENGTH(CompressedJson)'CompressedJsonLen',
		HtmlLen,DATALENGTH(CompressedHtml)'CompressedHtmlLen',
		ReportCreationMsecs
	FROM Report r
	LEFT JOIN p_SearchResults sr
		ON sr.ID=r.QueryID
	LEFT JOIN p_FirstName fn
		ON sr.FirstNameID=fn.ID
	LEFT JOIN p_LastName ln
		ON sr.LastNameID=ln.ID
	LEFT JOIN OrderItem oi
		ON oi.ID=r.OrderItemID







GO
