SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spOrderCreate]( @SiteID INT, @OrderXml XML, @QueryID INT=NULL, @ProfileID VARCHAR(20)=NULL) AS
	SET NOCOUNT ON;
		DECLARE @OrderID INT;

    WITH 
			XMLNAMESPACES (
				'https://api.intelius.com/commerce/2.1/wsdl' AS iws)
    SELECT 
      T.item.query(N'../../iws:OrderID').value(N'.','INT')'OrderID',
			T.item.query(N'../../iws:OrderDate').value(N'.','DATETIME')'OrderDate',
			T.item.query(N'../../iws:UserID').value(N'.','INT')'UserID',
			T.item.query(N'../../iws:TestCaseID').value(N'.','INT')'TestCaseID',
			T.item.query(N'../../iws:Channel/iws:ChannelAdWord').value(N'.','INT')'VisitID',
			T.item.query(N'../../iws:NetPrice').value(N'.','MONEY')'NetPrice',
			T.item.query(N'iws:OrderItemID').value('.','INT')'OrderItemID',
			T.item.query(N'iws:ProductID').value('.','INT')'ProductID',
			T.item.query(N'iws:ReferenceID').value('.','VARCHAR(MAX)')'ReferenceID',
			T.item.query(N'iws:ProductTitle').value('.','VARCHAR(MAX)')'ProductTitle',
			T.item.query(N'iws:Quantity').value('.','INT')'Quantity',
			T.item.query(N'iws:MSRPPrice').value(N'.','MONEY')'MSRPPrice',
			T.item.query(N'iws:Price').value(N'.','MONEY')'Price',
			T.item.query(N'iws:TotalPrice').value(N'.','MONEY')'TotalPrice',
			T.item.query(N'iws:TaxAmount').value(N'.','MONEY')'TaxAmount',
			T.item.query(N'./iws:Discounts/iws:DiscountDetail/iws:Discount').value(N'.','MONEY')'Discount',
			T.item.query(N'./iws:Discounts/iws:DiscountDetail/iws:PurchaseSubText').value(N'.','VARCHAR(MAX)')'DiscountDescription'
			INTO #orderItems
      FROM @OrderXml
        .nodes(N'/OrderType/iws:OrderItems/iws:OrderItem') AS T(item);

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
						@OrderID,@SiteID,t.OrderID,GETDATE(), t.UserID,v.UserID,
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
				SELECT DISTINCT @SiteID,0,t.ProductID,t.Price,t.Discount,0,ProductTitle
					FROM #orderItems t
					WHERE NOT EXISTS (
						SELECT * 
							FROM Product p
							WHERE SiteID=@SiteID
							AND p.ProductExternalID=t.ProductID
							AND p.Price=t.Price
					)


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
	COMMIT
	RETURN @OrderID
GO
