SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spReportCreate](
	@SiteID TINYINT,
	@UserID INT,
	@OrderID INT,
	@QueryID INT,
	@ProfileID CHAR(11),
	@State CHAR(2)=NULL,
	@ReportTypeCode VARCHAR(3),
	@JsonLen INT,
	@CompressedJson VARBINARY(MAX),
	@HtmlLen INT,
	@CompressedHtml VARBINARY(MAX),
	@ReportCreationMsecs INT)
AS
	SET NOCOUNT ON 
	DECLARE 
		@ReportID INT,
		@OrderItemID INT

	SELECT @OrderItemID=oi.ID
		FROM OrderItem oi
		JOIN Product p
			ON p.SiteID=oi.SiteID
			AND p.ID=oi.ProductID
		WHERE oi.SiteID=@SiteID
		AND oi.OrderID=@OrderID
		AND ReportTypeCode IS NOT NULL

		DECLARE @BaseID INT=DATEDIFF(day,'1/1/15',GETDATE()) * 65536;
		SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
		BEGIN TRAN
			SELECT @ReportID=1+ISNULL(MAX(ID),@BaseID) FROM Report WHERE ID >= @BaseID
			INSERT INTO Report(
					ID,SiteID,UserID,OrderItemID,QueryID,ProfileID,State,ReportTypeCode,ReportDate,
					JsonLen,CompressedJson,HtmlLen,CompressedHtml,ReportCreationMsecs)
				VALUES(
						@ReportID,
						@SiteID,@UserID,@OrderItemID,@QueryID,@ProfileID,@State,@ReportTypeCode,GETDATE(),
						@JsonLen,@CompressedJson,@HtmlLen,@CompressedHtml,@ReportCreationMsecs)
		COMMIT
	RETURN @ReportID
GO
