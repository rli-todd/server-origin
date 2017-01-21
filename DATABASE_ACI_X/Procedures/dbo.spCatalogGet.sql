SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spCatalogGet]( @SiteID TINYINT, @CategoryID INT=NULL, @IsActive BIT=1) AS
  SET NOCOUNT ON
  SELECT *
    FROM Category
    WHERE SiteID=@SiteID
		AND ID=ISNULL(@CategoryID,ID)
		AND IsActive=@IsActive

  SELECT sku.*
    FROM Category c
		JOIN Sku 
			ON c.SiteID=sku.SiteID
			AND c.ID=sku.CategoryID
    WHERE c.SiteID=@SiteID
		AND c.ID=ISNULL(@CategoryID,c.ID)
		AND c.IsActive=@IsActive
		AND sku.IsActive=@IsActive

  SELECT p.SiteID,p.ID,p.SkuID,p.ProductExternalID,p.ProductToken,p.Price,p.DiscountAmount,p.RecurringPrice,
		REPLACE(REPLACE(p.ProductName,'<b>',''),'</b>','')'ProductName',
		p.RequireQueryID,p.RequireState,p.RequireProfileID,p.ReportTypeCode
    FROM Category c
		JOIN Sku 
			ON c.SiteID=sku.SiteID
			AND c.ID=sku.CategoryID
		JOIN Product p
			ON p.SiteID=sku.SiteID
			AND p.SkuID = sku.ID
    WHERE c.SiteID=@SiteID
		AND c.ID=ISNULL(@CategoryID,c.ID)
		AND c.IsActive=@IsActive
		AND sku.IsActive=@IsActive
GO
