SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW vDailyRepeatOrderSummary AS
WITH cte AS
(
  SELECT
    CONVERT(DATE,FIrstOrderDate)'NewCustDate',
    COUNT(UserID)'NewCustCount',
    SUM(CASE WHEN MinsToSecondOrder IS NOT NULL AND MinsToSecondOrder<=60 THEN 1 ELSE 0 END)'RepeatUnderOneHour',
    SUM(CASE WHEN MinsToSecondOrder IS NOT NULL AND MinsToSecondOrder>60 AND MinsToSecondOrder<=1440 THEN 1 ELSE 0 END)'RepeatUnderOneDay',
    SUM(CASE WHEN MinsToSecondOrder IS NOT NULL AND MinsToSecondOrder>1440 AND MinsToSecondOrder<=10080 THEN 1 ELSE 0 END)'RepeatUnderOneWeek',
    SUM(CASE WHEN MinsToSecondOrder IS NOT NULL AND MinsTOSecondOrder>1080 AND MinsToSecondOrder<=43200 THEN 1 ELSE 0 END)'RepeatUnderOneMonth'

    FROM vUserRepeatOrders
    GROUP BY CONVERT(DATE,FirstOrderDate)
)
  SELECT TOP 100000 NewCustDate,NewCustCount,
    CONVERT(MONEY,RepeatUnderOneHour)/NewCustCount'OneHourPcnt',
    CONVERT(MONEY,RepeatUnderOneDay)/NewCustCount'OneDayPcnt',
    CONVERT(MONEY,RepeatUnderOneWeek)/NewCustCount'OneWeekPcnt',
    CONVERT(MONEY,RepeatUnderOneMonth)/NewCustCount'OneMonthPcnt'
    FROM cte
    ORDER BY NewCustDate
GO
