SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spOrderCreate]( @SiteID INT, @UserID INT,@OrderXml XML, @QueryID INT=NULL, @ProfileID VARCHAR(20)=NULL) AS
	SET NOCOUNT ON;
		DECLARE @OrderID INT;

     SELECT 
      T.item.query(N'../../transactionId').value(N'.','INT')'OrderID',
			T.item.query(N'../../transactionDate').value(N'.','DATETIME')'OrderDate',
			T.item.query(N'../../totalPrice').value(N'.','MONEY')'NetPrice',
      T.item.query(N'../../transactionDetails/adword').value(N'.','INT')'VisitID',
			T.item.query(N'lineItemId').value('.','INT')'OrderItemID',
			T.item.query(N'product/productOfferingId').value('.','VARCHAR(MAX)')'ProductOfferingID',
			T.item.query(N'product/productId').value('.','VARCHAR(MAX)')'ProductID',
			T.item.query(N'product/title').value('.','VARCHAR(MAX)')'ProductTitle',
			T.item.query(N'product/quantity').value('.','INT')'Quantity',
			T.item.query(N'product/msrp').value(N'.','MONEY')'MSRPPrice',
			T.item.query(N'product/price').value(N'.','MONEY')'Price',
			T.item.query(N'product/amount').value(N'.','MONEY')'TotalPrice',
			T.item.query(N'tax').value(N'.','MONEY')'TaxAmount'
      INTO #orderItems
      FROM @OrderXml
        .nodes(N'/TransactionResponse/transaction/lineItems/TransactionLineItem') AS T(item);

		/*
		** Get an Order ID
		*/
		DECLARE @BaseID INT=DATEDIFF(day,'1/1/15',GETDATE()) * 65536;
		SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
		SET XACT_ABORT ON
		BEGIN TRAN
			SELECT @OrderID=1+ISNULL(MAX(ID),@BaseID) FROM Orders WHERE ID >= @BaseID
			INSERT INTO Orders(
					ID,SiteID,ExternalID,OrderDate,UserExternalID,UserID,
					VisitID,OrderTotal)
				SELECT DISTINCT 
						@OrderID,@SiteID,t.OrderID,T.OrderDate, v.IwsUserID,v.UserID,
						t.VisitID,NetPrice
					FROM #orderItems t
					LEFT JOIN Visit v
						ON v.SiteID=@SiteID
						AND v.ID=t.VisitID

			/*
			** Make sure there is a product item for every order item.  
			** These should already exist, but the data on the IWS side could change.
			*/
			INSERT INTO Product(SiteID,SkuID,ProductExternalID,Price,DiscountAmount,RecurringPrice,ProductName)
				SELECT DISTINCT @SiteID,0,t.ProductID,t.Price,0,0,ProductTitle
					FROM #orderItems t
					WHERE NOT EXISTS (
						SELECT * 
							FROM Product p
							WHERE SiteID=@SiteID
							AND p.ProductExternalID=t.ProductID
							AND p.Price=t.Price
					)

/*
			INSERT INTO OrderItem(
					SiteID,OrderID,ExternalID,ProductID,ProductExternalID,ReferenceID,Quantity,
					Price,DiscountAmount,DiscountDescription,QueryID,ProfileID,State)
				SELECT DISTINCT 
						@SiteID,o.ID,t.OrderItemID,p.ID,t.ProductID, t.ReferenceID,t.Quantity,
						t.TotalPrice,t.Discount,t.DiscountDescription,@QueryID,@ProfileID,sr.State
					FROM #orderItems t
					JOIN Orders o
						ON o.SiteID=@SiteID
						AND o.ExternalID=t.OrderID
					JOIN Product p
						ON p.SiteID=@SiteID
						AND p.ProductExternalID=t.ProductID
						AND p.Price=t.Price
						AND p.RecurringPrice=0
					JOIN Sku sku
						ON sku.SiteID=p.SiteID
						AND sku.ID=p.SkuID
						--AND sku.IsActive=1
					LEFT JOIN p_SearchResults sr
						ON sr.ID=@QueryID
					WHERE NOT EXISTS (
						SELECT 1
							FROM OrderItem oi
							WHERE SiteID=@SiteID
							AND OrderID=o.ID
							AND oi.ExternalID=t.OrderItemID
					)
*/
	COMMIT
	RETURN @OrderID
GO
