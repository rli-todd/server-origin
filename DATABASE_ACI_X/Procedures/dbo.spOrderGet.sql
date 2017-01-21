SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spOrderGet](@SiteID TINYINT, @Keys ID_TABLE READONLY) AS
	SET NOCOUNT ON
	SELECT o.SiteID,o.ID'OrderID',o.ExternalID'OrderExternalID',OrderDate,o.UserID,o.VisitID,o.OrderTotal
		FROM Orders o
		JOIN @Keys k
			ON k.ID=o.ID
		WHERE SiteID=@SiteID

	SELECT 
			oi.SiteID,oi.OrderID,oi.ID'OrderItemID',oi.ExternalID'OrderItemExternalID',
			oi.ProductID,oi.ProductExternalID,p.SkuID,oi.Quantity,
			S.Price'RegularPrice',oi.Price,oi.DiscountAmount,oi.DiscountDescription,
			ISNULL(p.RecurringPrice,0)'RecurringPrice',p.ReportTypeCode,
			c.CategoryName,oi.QueryID,oi.ProfileID,FirstName,LastName,MiddleInitial,oi.State
		FROM OrderItem oi
		JOIN @Keys k
			ON k.ID=oi.OrderID
		JOIN Product p
			ON p.SiteID=@SiteID
			AND p.ID=oi.ProductID
		JOIN Sku s
			ON s.SiteID=@SiteID
			AND s.ID=p.SkuID
		JOIN Category c
			ON c.SiteID=@SiteID
			AND c.ID=s.CategoryID
		LEFT JOIN p_SearchResults sr
			ON sr.ID=oi.QueryID
		LEFT JOIN p_FirstName fn
			ON sr.FirstNameID=fn.ID
		LEFT JOIN p_LastName ln
			ON sr.LastNameID=ln.ID
		WHERE oi.SiteID=@SiteID
GO
