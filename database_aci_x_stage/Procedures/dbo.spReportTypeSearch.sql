SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spReportTypeSearch(@SiteID TINYINT) AS
	SET NOCOUNT  ON
	SELECT TypeCode
		FROM ReportType
		WHERE SiteID=@SiteID
GO
