SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vUserRepeatOrders] AS
WITH customer_order_summary AS
(
  SELECT UserID,FirstName,LastName,COUNT(*)'OrderCount'
    FROM Orders o
    JOIN Users u
      ON u.ID=o.UserID
    WHERE FirstName<>'Joe' OR LastName<>'Test'
    GROUP BY UserID,FIrstName,LastName
),
customer_orders_temp AS
(
  SELECT UserID,o.ID'OrderID',o.OrderDate,ROW_NUMBER() OVER (PARTITION BY UserID ORDER BY OrderDate)'Ordinal'
    FROM Orders o
),
customer_orders AS
(
  SELECT cs.*,OrderID,OrderDate,Ordinal
    FROM customer_orders_temp co
    JOIN customer_order_summary cs
      ON co.UserID=cs.UserID
),
repeat_summary AS
(
  SELECT co1.UserID,co1.FirstName,co1.LastName,co1.OrderCount,co1.OrderDate'FirstOrderDate',DATEDIFF(minute,co1.OrderDate,co2.OrderDate)'MinsToSecondOrder',DATEDIFF(minute,co1.OrderDate,co3.OrderDate)'MinsToLastOrder'
    FROM customer_orders co1
    LEFT JOIN customer_orders co2
      ON co1.UserID=co2.UserID
      AND co2.Ordinal=2
    LEFT JOIN customer_orders co3
      ON co1.UserID=co3.UserID
      AND co3.Ordinal=co3.OrderCount
      AND co3.OrderID<>co1.OrderID
    WHERE co1.Ordinal=1
)
  SELECT TOP 100000 *,
    MinsToSecondOrder/60'HoursToSecondOrder',
    MinsToLastOrder/60'HoursToLastOrder',
    MinsToSecondOrder/1440'DaysToSecondOrder',
    MinsToLastOrder/1440'DaysToLastOrder'
    FROM repeat_summary
    ORDER BY OrderCount DESC
GO
