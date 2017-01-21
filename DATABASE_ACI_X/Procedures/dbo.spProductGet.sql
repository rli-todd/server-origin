SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spProductGet](@SiteID TINYINT) AS
  SET NOCOUNT ON
	SELECT 
			pi.ID'ProductID',
			sku.CategoryID,CategoryName,sku.ProductExternalID,pi.ProductToken,SkuID,ProductType,CategoryCode,
			pi.ProductExternalID'ProductExternalID',pi.Price,pi.DiscountAmount,
			pi.RecurringPrice,ProductName,pi.RequireQueryID,pi.RequireState,pi.RequireProfileID
		FROM Sku sku
		JOIN Category c
			ON c.SiteID=@SiteID
			AND c.ID=sku.CategoryID
		JOIN Product pi
			ON pi.SiteID=@SiteID
			AND pi.SkuID=sku.ID
		WHERE c.IsActive=1
		AND sku.IsActive=1
		AND sku.SiteID=@SiteID
		ORDER BY CategoryID,SkuID,pi.ID
GO
