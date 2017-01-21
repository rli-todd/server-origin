SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spSkuGet](@SiteID TINYINT, @SkuKeys ID_TABLE READONLY) AS
	SET NOCOUNT ON

	DECLARE @SkuIDs ID_TABLE

	IF EXISTS (SELECT 1 FROM @SkuKeys)
		INSERT INTO @SkuIDs(ID)
			SELECT ID
				FROM @SkuKeys
	ELSE
		INSERT INTO @SkuIDs(ID)
			SELECT ID
				FROM Sku
				WHERE SiteID=@SiteID
				AND IsActive=1

	SELECT *
		FROM Sku
		JOIN @SkuIDs si
			ON si.ID=sku.ID
		WHERE sku.SiteID=@SiteID

  SELECT p.SiteID,p.ID,p.SkuID,p.ProductExternalID,p.Price,p.DiscountAmount,p.RecurringPrice,
		REPLACE(REPLACE(p.ProductName,'<b>',''),'</b>','')'ProductName',
		p.RequireQueryID,p.RequireState,p.RequireProfileID,p.ReportTypeCode
    FROM Sku 
		JOIN @SkuIDs si
			ON si.ID=sku.ID
		JOIN Product p
			ON p.SiteID=sku.SiteID
			AND p.SkuID = sku.ID
    WHERE sku.SiteID=@SiteID

GO
