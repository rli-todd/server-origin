SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spReportValidateAccess](@AuthorizedUserID INT, @SiteID TINYINT, @Keys ID_TABLE READONLY) AS
	SET NOCOUNT ON

	/*
	** If user is a BackofficeReader, the requirement that
	** the Report by owned by the calling user is removed
	*/
	IF EXISTS (SELECT 1
		FROM Users
		WHERE SiteID=@SiteID
		AND ID=@AuthorizedUserID
		AND IsBackofficeReader=1
	)
	BEGIN
		SET @AuthorizedUserID=NULL
	END

	SELECT r.ID
		FROM Report r
		JOIN @Keys k
			ON k.ID=r.ID
		WHERE SiteID=@SiteID
		AND r.UserID=ISNULL(@AuthorizedUserID,r.UserID)
GO
