SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spProductGet](@SiteID TINYINT) AS
  SET NOCOUNT ON
	SELECT 
			p.ID'ProductID',
			p.CategoryID,c.CategoryName,c.ProductExternalID,p.ProductToken,CategoryType,p.ReportTypeCode,
			p.MSRP,p.Price,p.DiscountAmount,
			p.RecurringPrice,c.RequireQueryID,c.RequireState,c.RequireProfileID
		FROM Category c
		JOIN Product p
			ON p.SiteID=@SiteID
			AND p.CategoryID=c.ID
		WHERE c.IsActive=1
    AND c.SiteID=@SiteID
		ORDER BY CategoryID,p.ID
GO
