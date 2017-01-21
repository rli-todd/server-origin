SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spOrderSearch](
	@SiteID TINYINT, 
	@AuthorizedUserID INT, 
	@FirstName VARCHAR(30)=NULL,
	@MiddleInitial CHAR(1)=NULL,
	@LastName VARCHAR(30)=NULL,
	@State VARCHAR(2)=NULL,
	@ProfileID VARCHAR(11)=NULL,
	@SelectedUserID INT=NULL,
	@ExternalOrderIDs ID_TABLE READONLY) AS
	SET NOCOUNT ON

	/*
	** If user is a BackofficeReader, the requirement that
	** the order by owned by the calling user is removed
	*/
	IF (EXISTS (SELECT 1 FROM @ExternalOrderIDs) OR @SelectedUserID IS NOT NULL)
	AND EXISTS (SELECT 1
		FROM Users
		WHERE SiteID=@SiteID
		AND ID=@AuthorizedUserID
		AND IsBackofficeReader=1
	)
	BEGIN
		SET @AuthorizedUserID=NULL
	END

	IF EXISTS (SELECT 1 FROM @ExternalOrderIDs)
		SELECT o.ID
			FROM Orders o
			JOIN @ExternalOrderIDs eoi
				ON eoi.ID=o.ExternalID
			WHERE o.SiteID=@SiteID
			--AND o.UserID=ISNULL(@AuthorizedUserID,o.UserID)
	ELSE
		SELECT o.ID
			FROM Orders o
			LEFT JOIN OrderItem oi
				ON oi.SiteID=o.SiteID
				AND oi.OrderID=o.ID
			LEFT JOIN p_SearchResults sr
				ON sr.ID=oi.QueryID
			LEFT JOIN p_FirstName fn
				ON sr.FirstNameID=fn.ID
			LEFT JOIN p_LastName ln
				ON sr.LastnameID=ln.ID
			WHERE o.SiteID=@SiteID
			AND o.UserID=ISNULL(@AuthorizedUserID,ISNULL(@SelectedUserID, o.UserID))
			AND ISNULL(ProfileID,'')=ISNULL(@ProfileID,ISNULL(ProfileID,''))
			AND ISNULL(fn.FirstName,'')=ISNULL(@FirstName,ISNULL(fn.FirstName,''))
			AND ISNULL(MiddleInitial,'')=ISNULL(@MiddleInitial,ISNULL(MiddleInitial,''))
			AND ISNULL(ln.LastName,'')=ISNULL(@LastName,ISNULL(ln.LastName,''))
			AND ISNULL(oi.State,'')=ISNULL(@State,ISNULL(oi.State,''))
GO
