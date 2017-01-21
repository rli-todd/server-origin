SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[vProduct] AS
	SELECT TOP 1000
			c.SiteID,CategoryID,CategoryName,SkuID,ProductType,
			pi.ID'ProductID',pi.ProductName,pi.Price,pi.RecurringPrice,
			pi.DiscountAmount,pi.RequireQueryID,pi.RequireState,pi.RequireProfileID,
			pi.ReportTypeCode,rt.Title,rt.ProfileAttributes
		FROM Category c
		JOIN Sku s
			ON s.CategoryID=c.ID
			AND s.SiteID=c.SiteID
		JOIN Product pi
			ON pi.SkuID=s.ID
			AND pi.SiteID=s.SiteID
		LEFT JOIN ReportType rt
			ON rt.TypeCode=pi.ReportTypeCode
			AND rt.SiteID=pi.SiteID
		WHERE s.IsActive=1
		AND c.IsActive=1
		ORDER BY CategoryID,SkuID,ProductID






GO
