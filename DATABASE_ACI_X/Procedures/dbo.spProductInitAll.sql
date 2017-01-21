SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spProductInitAll] WITH RECOMPILE AS
	SET NOCOUNT ON
	DECLARE 
		@DefaultProducts TABLE(
			ExternalID INT, 
			ProductName VARCHAR(50), 
			RequireQueryID BIT, 
			RequireState BIT,
			RequireProfileID BIT,
			ProfileAttributes VARCHAR(255),
			ReportTypeCode VARCHAR(3));
	DECLARE 
		@CR_SiteID TINYINT=1

	INSERT INTO @DefaultProducts(
			ExternalID,ProductName, RequireQueryID, RequireState, RequireProfileID,ReportTypeCode) VALUES
		--(450,'Instant People Lookup Report',1,0,1,'PL'),
		--(452,'Statewide Criminal Check',1,1,1,'SCC'),
		--(453,'Nationwide Criminal Check',1,0,1,'NCC')
		(1247,'Instant People Lookup Report',1,0,1,'PL'),
		(1248,'Statewide Criminal Check',1,1,1,'SCC'),
		(1249,'Nationwide Criminal Check',1,0,1,'NCC')

	INSERT INTO Category(SiteID,ExternalID,CategoryName,IsActive)
		SELECT @CR_SiteID,ExternalID,ProductName,1
			FROM @DefaultProducts dp
			WHERE NOT EXISTS (
				SELECT 1
					FROM Category c
					WHERE c.SiteID=@CR_SiteID
					AND c.ExternalID=dp.ExternalID
			)

	UPDATE Category SET CategoryCode=d.ReportTypeCode
		FROM Category c
		JOIN @DefaultProducts d
			ON c.SiteID=@CR_SiteID
			AND c.ExternalID=d.ExternalID

	UPDATE Product SET 
			RequireQUeryID=d.RequireQueryID,
			RequireProfileID=d.RequireProfileID,
			RequireState=d.RequireState,
			ReportTypeCode=d.ReportTypeCode
		FROM Product pi
		JOIN @DefaultProducts d
			ON d.ExternalID=pi.ProductExternalID

	/*
	** Disable SKUs with a subscription
	*/

	UPDATE Sku SET IsActive=0
		FROM Sku s
		JOIN Product p
			ON p.SiteID=s.SiteID
			AND p.SkuID=s.ID
		WHERE p.RecurringPrice>0

	/*
	** Create an "inactive" SKU with a price of zero for all active SKUs
	*/

	INSERT INTO Sku(SiteID,CategoryID,ProductExternalID,ProductType,IsActive,Price,RecurringPrice)
		SELECT SiteID,CategoryID,ProductExternalID,ProductType,0,0,0
			FROM Sku
			WHERE NOT EXISTS (
				SELECT 1
					FROM Sku sku2
					WHERE sku.SiteID=sku2.SiteID
					AND sku.CategoryID=sku2.CategoryID
					AND sku.ProductExternalID=sku2.ProductExternalID
					AND sku.ProductType=sku2.ProductTYpe
					AND sku2.IsActive=0
					AND sku2.Price=0
					AND sku2.RecurringPrice=0
			)

	INSERT INTO Product(SiteID,SkuID,ProductExternalID,Price,DiscountAmount,RecurringPrice,
		ProductName,RequireQueryID,RequireProfileID,RequireState,ReportTypeCode)
		SELECT p.SiteID,sku.ID,p.ProductExternalID,0,0,0,
				ProductName,RequireQueryID,RequireProfileID,RequireState,ReportTypeCode
			FROM Product p
			JOIN Sku 
				ON p.SiteID=sku.SiteID
				AND p.ProductExternalID=sku.ProductExternalID
			WHERE p.Price > 0
			AND sku.IsActive=0
			AND NOT EXISTS (
				SELECT 1
					FROM Product p2
					WHERE p.SiteID=p2.SiteID
					AND p.ProductExternalID=p2.ProductExternalID
					AND sku.Price=0
					AND p2.Price=0
					AND p2.DiscountAmount=0
					AND p2.RecurringPrice=0
			)

GO
