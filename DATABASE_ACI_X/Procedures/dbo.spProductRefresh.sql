SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spProductRefresh]( @SiteID TINYINT,@ExternalProductsXml XML) AS
/*
** Get all Product items for list of "Intelius Products" that we want
** For each product ID, each should have an "alacarte" Product
** Some will have an alacartewithSubscription Product (with subscription details)
*/
  SET NOCOUNT ON;

    WITH XMLNAMESPACES ('https://api.intelius.com/commerce/2.1/wsdl' AS iws)
    SELECT 
      T.item.query(N'./iws:ProductReferenceID')
        .value(N'.','INT')'ProductExternalID',
      T.item.query(N'./iws:PriceListings/iws:PriceList[1]/iws:Price')
        .value(N'.','MONEY')'Price',
      T.item.query(N'./iws:PriceListings/iws:PriceList[1]/iws:MSRPPrice')
        .value(N'.','MONEY')'MSRPPrice',
      T.item.query(N'./iws:SubscriptionDetails/iws:Title')
        .value(N'.','VARCHAR(MAX)')'SubscriptionProductName',
      T.item.query(N'./iws:SubscriptionDetails/iws:Savings')
        .value(N'.','MONEY')'DiscountAmount',
      T.item.query(N'./iws:SubscriptionDetails/iws:ProductReferenceID')
        .value(N'.','INT')'SubscriptionProductExternalID',
      T.item.query(N'./iws:SubscriptionDetails/iws:RecurringPrice')
        .value(N'.','MONEY')'SubscriptionRecurringPrice',
      T.item.query(N'./iws:ProductType')
        .value(N'.','VARCHAR(255)')'ProductType',
      T.item.query(N'./iws:ProductID')
        .value(N'.','VARCHAR(255)')'ProductToken',
      T.item.query(N'./iws:Quantity')
        .value(N'.','INT')'Quantity',
      T.item.query(N'./iws:Units')
        .value(N'.','INT')'Units',
      T.item.query(N'./iws:Title')
        .value(N'.','VARCHAR(MAX)')'Title'
      INTO #product_items
      FROM @ExternalProductsXml
        .nodes(N'ArrayOfProduct/Product') AS T(item);

		/*
		** First take care of the product options
		*/
		INSERT INTO Sku(
				SiteID,CategoryID,ProductType,ProductExternalID,IsActive)
			SELECT DISTINCT			
					@SiteID,c.ID,tpi.ProductType,tpi.ProductExternalID,c.IsActive
				FROM #product_items tpi
				JOIN Category c
					ON c.SiteID=@SiteID
					AND c.ExternalID=tpi.ProductExternalID
				WHERE NOT EXISTS (
					SELECT 1
						FROM Sku sku
						WHERE sku.SiteID=@SiteID
						AND sku.CategoryID=c.ID
						AND sku.ProductType=tpi.ProductType
				)

		UPDATE Sku SET IsActive=c.IsActive
			FROM Category c
			JOIN Sku s
				ON s.SiteID=@SiteID
				AND s.CategoryID=c.ID
			WHERE c.SiteID=@SiteID
		/*
		** First the a la carte product options.
		** These will be a single item, with no discount or subscription
		*/
		UPDATE Product SET
			Price=tpi.Price,
			DiscountAmount=tpi.DiscountAmount,
			ProductToken=tpi.ProductToken
			FROM #product_items tpi
			JOIN Category c
				ON c.SiteID=@SiteID
				AND c.ExternalID=tpi.ProductExternalID
			JOIN Sku sku
				ON sku.SiteID=@SiteID
				AND sku.CategoryID=c.ID
				AND sku.ProductType=tpi.ProductType
			JOIN Product pi
				ON pi.SiteID=@SiteID
				AND pi.SkuID=sku.ID
			WHERE tpi.ProductType='AlaCart'

		INSERT INTO Product(
				SiteID,SkuID,ProductExternalID,
				Price,DiscountAmount,RecurringPrice,ProductName,
				RequireQueryID,RequireState,RequireProfileID)
			SELECT DISTINCT 
					@SiteID,sku.ID,tpi.ProductExternalID,
					tpi.Price,tpi.DiscountAmount,0,tpi.Title,
					0,0,0
				FROM #product_items tpi
				JOIN Category c
					ON c.SiteID=@SiteID
					AND c.ExternalID=tpi.ProductExternalID
				JOIN Sku sku
					ON sku.SiteID=@SiteID
					AND sku.CategoryID=c.ID
					AND sku.ProductType=tpi.ProductType
				WHERE tpi.ProductType='AlaCart'
				AND NOT EXISTS (
					SELECT 1
						FROM Product p
						WHERE p.SiteID=@SiteID
						AND p.SkuID=sku.ID
						AND p.ProductExternalID=tpi.ProductExternalID
				)

		/*
		** Now for the subscription product options.
		** These Sku groups will contain two Products:
		** 1) The main product that the customer is ordering, with a discount.
		** 2) The subscription product, with a recurring price.
		*/
		
		-- First update the existing "main product"
		UPDATE Product SET
			Price=tpi.Price,
			DiscountAmount=tpi.DiscountAmount,
			RecurringPrice=0,
			ProductToken=tpi.ProductToken
			FROM #product_items tpi
			JOIN Category c
				ON c.SiteID=@SiteID
				AND c.ExternalID=tpi.ProductExternalID
			JOIN Sku sku
				ON sku.SiteID=@SiteID
				AND sku.CategoryID=c.ID
				AND sku.ProductType=tpi.ProductType
			JOIN Product p
				ON p.SiteID=@SiteID
				AND p.SkuID=sku.ID
				AND p.ProductExternalID=tpi.ProductExternalID
			WHERE tpi.ProductType='AlaCartWithSubscription'
			
		-- Now update the existing "subscription product"
		UPDATE Product SET
			Price=0,
			DiscountAmount=0,
			RecurringPrice=tpi.SubscriptionRecurringPrice,
			ProductToken=tpi.ProductToken
			FROM #product_items tpi
			JOIN Category c
				ON c.SiteID=@SiteID
				AND c.ExternalID=tpi.ProductExternalID
			JOIN Sku sku
				ON sku.SiteID=@SiteID
				AND sku.CategoryID = c.ID
				AND sku.ProductType=tpi.ProductType
			JOIN Product pi
				ON pi.SiteID=@SiteID
				AND pi.SkuID=sku.ID
				AND pi.ProductExternalID=tpi.SubscriptionProductExternalID
			WHERE tpi.ProductType='AlaCartWithSubscription'
			
		-- Insert a yet-to-be-created "main product"
		INSERT INTO Product(
				SiteID,SkuID,ProductExternalID,ProductToken,
				Price,DiscountAmount,RecurringPrice,ProductName,
				RequireQueryID,RequireState,RequireProfileID)
			SELECT DISTINCT 
					@SiteID,sku.ID,tpi.ProductExternalID,ProductToken,
					tpi.Price,tpi.DiscountAmount,0,c.CategoryName,
					0,0,0
				FROM #product_items tpi
				JOIN Category c
					ON c.SiteID=@SiteID
					AND c.ExternalID=tpi.ProductExternalID
				JOIN Sku sku
					ON sku.SiteID=@SiteID
					AND sku.CategoryID=c.ID
				WHERE tpi.ProductType='AlaCartWithSubscription'
				AND NOT EXISTS (
					SELECT 1
						FROM Product pi
						WHERE pi.SiteID=@SiteID
						AND pi.SkuID=sku.ID
						AND pi.ProductExternalID=tpi.ProductExternalID
				)
			
		-- Insert a yet-to-be-created "subscription product"
		INSERT INTO Product(
				SiteID,SkuID,ProductExternalID,ProductToken,
				Price,DiscountAmount,RecurringPrice,ProductName,
				RequireQueryID,RequireState,RequireProfileID)
			SELECT DISTINCT 
					@SiteID,sku.ID,tpi.SubscriptionProductExternalID,ProductToken,
					0,0,tpi.SubscriptionRecurringPrice,SubscriptionProductName,
					0,0,0
				FROM #product_items tpi
				JOIN Category c
					ON c.SiteID=@SiteID
					AND c.ExternalID=tpi.ProductExternalID
				JOIN Sku sku
					ON sku.SiteID=@SiteID
					AND sku.CategoryID=c.ID
					AND sku.ProductType=tpi.ProductType
				WHERE tpi.ProductType='AlaCartWithSubscription'
				AND NOT EXISTS (
					SELECT 1
						FROM Product pi
						WHERE pi.SiteID=@SiteID
						AND pi.SkuID=sku.ID
						AND pi.ProductExternalID=tpi.SubscriptionProductExternalID
				)

		UPDATE Sku 
			SET Price=p.Price,RecurringPrice=p.RecurringPrice
				FROM Sku 
				JOIN Product p
					ON p.SiteID=sku.SiteID
					AND p.ProductExternalID=sku.ProductExternalID
		--INSERT INTO Sku(SiteID,ProductExternalID,CategoryName,IsActive,Price,RecurringPrice)
		--	SELECT DISTINCT @SiteID,pi.ProductExternalID,ProductName,0,pi.Price,pi.RecurringPrice
		--		FROM Product pi
		--		WHERE NOT EXISTS (
		--			SELECT 1
		--				FROM Category
		--				WHERE SiteID=@SiteID
		--				AND ExternalID=pi.ProductExternalID
		--		)



GO
