SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vOrderSummary] AS
WITH v AS
(
	SELECT 
		CONVERT(DATE,v.DateCreated)'VisitDate',
		COUNT(DISTINCT v.ID)'Visits'
		FROM Visit v
		WHERE ISNULL(RobotID,0)=0
		AND v.DateCreated>='12/22/15'
		GROUP BY CONVERT(DATE,v.DateCreated)
),
o AS
(
	SELECT 
		CONVERT(DATE,o.OrderDate)'OrderDate',
		COUNT(DISTINCT OrderID)'Orders',
		SUM(OrderTotal)'GrossRev',
		CONVERT(MONEY,.75*SUM(OrderTotal))'AciGrossRev'
		FROM Orders o
		JOIN OrderItem oi
			ON oi.OrderID=o.ID
		GROUP BY CONVERT(DATE,o.OrderDate)
),
u AS
(
	SELECT 
		CONVERT(DATE,u.DateCreated)'DateCreated',
		COUNT(DISTINCT u.ID)'NewUsers'
		FROM Users u
		WHERE EmailAddress NOT LIKE 'iws_test%'
		GROUP BY CONVERT(DATE,u.DateCreated)
),
c AS
(
	SELECT 
		CONVERT(DATE,u.DateCreated)'DateCreated',
		COUNT(DISTINCT u.ID)'NewCustomers'
		FROM Users u
		JOIN Orders o
			ON o.UserID=u.ID
		WHERE EmailAddress NOT LIKE 'iws_test%'
		GROUP BY CONVERT(DATE,u.DateCreated)
)
	SELECT TOP 1000
			v.VisitDate'OrderDate',v.Visits,
			NewUsers,NewCustomers,o.Orders,GrossRev,AciGrossRev,
			CONVERT(MONEY,100*CONVERT(DECIMAL(10,5),NewCustomers)/Visits)'NewCustVisitConvPercent',
			Visits/NewCustomers'VisitsPerNewCust',
			CONVERT(MONEY,AciGrossRev/NewCustomers)'AciNewCustRpa',
			CONVERT(MONEY,AciGrossRev/NewCustomers)/(Visits/NewCustomers)'BreakEvenCPC'
		FROM v
		LEFT JOIN o
			ON v.VisitDate=o.OrderDate
		LEFT JOIN c
			ON c.DateCreated=v.VisitDate
		LEFT JOIN u
			ON u.DateCreated=v.VisitDate
		ORDER BY v.VisitDate DESC


GO
GRANT SELECT
	ON [dbo].[vOrderSummary]
	TO [analytics_readers]
GO
