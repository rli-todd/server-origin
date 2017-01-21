SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].spSkuSearch(@SiteID TINYINT, @ExternalSkuIDs ID_TABLE READONLY) AS
	SET NOCOUNT ON
	/*
	** When searching externalID, we only want to return skus that have ONLY A SINGLE PRODUCT
	*/
	SELECT sku.ID
		FROM Sku 
		JOIN Product p
			ON p.SiteID=sku.SiteID
			AND p.SkuID=sku.ID
		JOIN @ExternalSkuIDs esi
			ON esi.ID=sku.ProductExternalID
		WHERE sku.SiteID=@SiteID
		GROUP BY sku.ID
		HAVING COUNT(*)=1

GO
