SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spOrderValidateAccess](@AuthorizedUserID INT, @SiteID TINYINT, @Keys ID_TABLE READONLY) AS
	SET NOCOUNT ON

	/*
	** If user is a BackofficeReader, the requirement that
	** the order by owned by the calling user is removed
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

	SELECT o.ID
		FROM Orders o
		JOIN @Keys k
			ON k.ID=o.ID
		WHERE SiteID=@SiteID
		AND o.UserID=ISNULL(@AuthorizedUserID,o.UserID)
GO
