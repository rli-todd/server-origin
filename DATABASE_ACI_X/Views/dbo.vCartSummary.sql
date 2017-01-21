SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vCartSummary] AS
WITH v AS
(
	SELECT 
		CONVERT(DATE,v.DateCreated)'VisitDate',
		COUNT(DISTINCT v.ID)'Visits'
		FROM Visit v
		WHERE ISNULL(RobotID,0)=0
		AND v.DateCreated>='1/20/16'
		GROUP BY CONVERT(DATE,v.DateCreated)
),c AS
(
	SELECT 
		c.ID'CartID',
		CONVERT(DATE,c.DateModified)'CartDate',
		ProductID,
		p.ProductName,
		p.Price,
		OrderID,
		CASE WHEN OrderID IS NOT NULL THEN 'Order' ELSE 'Abandoned' END'Status',
		VisitID,
		CASE WHEN ConversionPathCode LIKE '%checkout%' THEN 1 ELSE 0 END'ReachedCheckout',
		u.ID'CreatedUserID'
			FROM Cart c
			JOIN Visit v
				ON v.SiteID=c.SiteID
				AND v.ID=c.VisitID
			LEFT JOIN ConversionPath cp
				ON cp.SiteID=v.SiteID
				AND cp.ID=v.ConversionPathID
			LEFT JOIN Users u
				ON u.SiteID=v.SiteID
				AND u.FirstVisitID=v.ID
			LEFT JOIN Product p
				ON p.SiteID=c.SiteID
				AND p.ID=c.ProductID
),
tc AS
(
	SELECT CartDate,COUNT(*)'TotalCarts'
		FROM c
		GROUP BY CartDate
),
f AS
(
	SELECT 
			v.VisitDate,
			c.ProductID,
			c.ProductName,
			c.Price,
			Visits,
			SUM(ReachedCheckout)'VisitsReachingCheckout',
			COUNT(DISTINCT CreatedUserID)'UserAccountsCreated',
			COUNT(DISTINCT OrderID)'CompletedOrders',
			COUNT(DISTINCT CartID)'CartsCreated',
			SUM(CASE WHEN Status='Abandoned' THEN 1 ELSE 0 END)'CartsAbandoned'
		FROM v
		LEFT JOIN c
			ON c.CartDate=v.VisitDate
		LEFT JOIN tc
			ON tc.CartDate=v.VisitDate
		GROUP BY v.VisitDate,Visits,c.ProductID,c.ProductName,c.Price
)
SELECT 
		VisitDate,ProductName,Price,
		VisitsReachingCheckout'CheckoutsStarted',
		Visits/VisitsReachingCheckout'VisitsPerCheckoutStarted',
		CartsCreated,
		UserAccountsCreated'NewUsers',
		Visits/UserAccountsCreated'VisitsPerNewUser',
		CompletedOrders,
		CartsAbandoned,
		(100*CartsAbandoned)/CartsCreated'%CartsAbandoned',
		CONVERT(MONEY,100*UserAccountsCreated)/Visits'%VisitsCreateAccount'
		FROM f

GO
