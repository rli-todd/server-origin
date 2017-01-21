SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vOrderItem] AS 
SELECT 
		o.SiteID,OrderID,o.ExternalID'OrderExternalID',OrderDate,UserID,u.EmailAddress,o.VisitID,OrderTotal,QueryID,ProfileID,
		pi.CategoryID,pi.ProductID,pi.CategoryName,
		sr.State,fn.FirstName,sr.MiddleInitial,ln.LastName
	FROM Orders o
	JOIN OrderItem oi
		ON oi.OrderID=o.ID
		AND oi.SiteID=o.SiteID
	JOIN vProduct pi
		ON oi.ProductID=pi.ProductID
		AND oi.SiteID=pi.SiteID
	JOIN Category c
		ON c.ID=pi.CategoryID
		AND c.SiteID=pi.SiteID
	LEFT JOIN p_SearchResults sr
		ON sr.ID=oi.QueryID
	LEFT JOIN p_FirstName fn
		ON sr.FirstNameID=fn.ID
	LEFT JOIN p_LastName ln
		ON sr.LastNameID=ln.ID
	LEFT JOIN Users u
		ON u.ID=o.UserID


GO
