SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spConversionPathStats]( @OrderDate DATETIME=NULL, @Mins INT=NULL, @TopPaths INT=25) AS
SET NOCOUNT ON
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
IF @Mins IS NOT NULL
	SET @OrderDate=DATEADD(minute,-@Mins,GETDATE())

SET @OrderDate=ISNULL(@OrderDate,CONVERT(DATE,GETDATE()))
SELECT
		@OrderDate'OrderDate', 
		COnversionPathCode,
		COUNT(*)'Visits',
		CASE WHEN ConversionPathCode LIKE '%srp%' THEN 1 ELSE 0 END'SRP',
		CASE WHEN ConversionPathCode LIKE '%find%' THEN 1 ELSE 0 END'Find',
		CASE WHEN ConversionPathCode LIKE '%select%' THEN 1 ELSE 0 END'Select',
		CASE WHEN ConversionPathCode LIKE '%checkout%' THEN 1 ELSE 0 END'Checkout'
	INTO #cp
	FROM Visit v
	JOIN ConversionPath cp
		ON cp.ID=v.ConversionPathID
	WHERE (v.DateModified>=@OrderDate OR v.DateCreated>=@OrderDate)
	AND (v.DateModified<DATEADD(day,1,CONVERT(DATE,@OrderDate)) OR v.DateModified<DATEADD(day,1,CONVERT(DATE,@OrderDate)))
	AND ISNULL(RobotID,0)=0
	GROUP BY ConversionPathCode
	ORDER BY 2 DESC;

SET ROWCOUNT @TopPaths
SELECT * FROM #cp ORDER BY Visits DESC
SET ROWCOUNT 0

SELECT 
	SUM(Visits)'Visits',
	SUM(Visits*SRP)'SRP',
	SUM(Visits*Find)'Find',
	SUM(Visits*[Select])'Select',
	SUM(Visits*Checkout)'Checkout',
	CONVERT(MONEY,100*SUM(Visits*SRP))/SUM(Visits)'SRP%',
	CONVERT(MONEY,100*SUM(Visits*Find))/SUM(Visits)'Find%',
	CONVERT(MONEY,100*SUM(Visits*[Select]))/SUM(Visits)'Select%',
	CONVERT(MONEY,100*SUM(Visits*Checkout))/SUM(Visits)'Checkout%',
	CONVERT(MONEY,100*SUM(Visits*SRP*Find))/SUM(Visits*SRP)'SRP->Find%',
	CONVERT(MONEY,100*SUM(Visits*Find*[Select]))/SUM(Visits*Find)'Find->Select%',
	CONVERT(MONEY,100*SUM(Visits*[Select]*Checkout))/SUM(Visits*[Select])'Select->Checkout%'
	FROM #cp

DROP TABLE #cp
GO
