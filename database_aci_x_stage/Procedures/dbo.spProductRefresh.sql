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

		UPDATE Product SET
			Price=tpi.Price,
			DiscountAmount=tpi.DiscountAmount,
			ProductToken=tpi.ProductToken,
      RecurringPrice=tpi.SubscriptionRecurringPrice
			FROM #product_items tpi
			JOIN Category c
				ON c.SiteID=@SiteID
				AND c.ProductExternalID=tpi.ProductExternalID
			JOIN Product p
				ON p.SiteID=@SiteID
				AND p.CategoryID=c.ID

		INSERT INTO Product(
				SiteID,CategoryID,ProductToken,
				MSRP,Price,DiscountAmount,RecurringPrice)
			SELECT DISTINCT 
					@SiteID,c.ID,tpi.ProductToken,
					tpi.MSRPPrice,tpi.Price,tpi.DiscountAmount,tpi.SubscriptionRecurringPrice
				FROM #product_items tpi
				JOIN Category c
					ON c.SiteID=@SiteID
					AND c.ProductExternalID=tpi.ProductExternalID
				WHERE NOT EXISTS (
					SELECT 1
						FROM Product p
						WHERE p.SiteID=@SiteID
						AND p.ProductExternalID=c.ProductExternalID
            AND p.ProductToken=tpi.ProductToken
				)

GO
