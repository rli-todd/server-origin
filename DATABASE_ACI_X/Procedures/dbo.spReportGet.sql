SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spReportGet](@SiteID TINYINT, @Keys ID_TABLE READONLY) AS
	SET NOCOUNT ON
	SELECT r.*,OrderID,o.ExternalID'OrderExternalID',Title,FirstName,MiddleInitial,LastName,ISNULL(r.State,sr.State)'State'
		FROM Report r
		JOIN ReportType rt
			ON rt.SiteID=r.SiteID
			AND rt.TypeCode=r.ReportTypeCode
		JOIN @Keys k
			ON k.ID=r.ID
		LEFT JOIN p_SearchResults sr
			ON sr.ID=r.QueryID
		LEFT JOIN p_FirstName fn
			ON fn.ID=sr.FirstNameID
		LEFT JOIN p_LastName ln
			ON ln.ID=sr.LastNameID
		LEFT JOIN OrderItem oi
			ON oi.SiteID=@SiteID
			AND oi.ID=r.OrderItemID
		LEFT JOIN Orders o
			ON o.SiteID=@SiteID
			AND o.ID=oi.OrderID
		WHERE r.SiteID=@SiteID
		ORDER BY r.ReportDate DESC


GO
