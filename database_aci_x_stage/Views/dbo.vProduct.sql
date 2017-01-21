SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO







CREATE VIEW [dbo].[vProduct] AS
	SELECT TOP 1000
			c.SiteID,CategoryID,CategoryName,CategoryType,
			pi.ID'ProductID',pi.MSRP,pi.Price,pi.RecurringPrice,
			pi.DiscountAmount,c.RequireQueryID,c.RequireState,c.RequireProfileID,
			pi.ReportTypeCode,rt.Title,rt.ProfileAttributes,pi.ProductToken
		FROM Category c
		JOIN Product pi
			ON pi.CategoryID=c.ID
			AND pi.SiteID=c.SiteID
		LEFT JOIN ReportType rt
			ON rt.TypeCode=pi.ReportTypeCode
			AND rt.SiteID=pi.SiteID
		WHERE c.IsActive=1
		ORDER BY CategoryID,ProductID








GO
