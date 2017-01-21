SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO



CREATE PROC [dbo].[spReportSearch](
	@SiteID TINYINT,
	@AuthorizedUserID INT,
	@FirstName VARCHAR(30)=NULL,
	@MiddleInitial CHAR(1)=NULL,
	@LastName VARCHAR(30)=NULL,
	@State CHAR(2)=NULL,
	@ProfileID VARCHAR(11)=NULL,
	@OrderID INT=NULL) 
AS
	SET NOCOUNT ON
  /*
	** If user is a BackofficeReader and we're searching by OrderID, the requirement that
	** the order by owned by the calling user is removed
	*/
	IF @OrderID IS NOT NULL
	AND EXISTS (SELECT 1
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
		LEFT JOIN OrderItem oi
			ON oi.ID=r.OrderItemID
		LEFT JOIN p_SearchResults sr
			ON sr.ID=r.QueryID
		LEFT JOIN p_FirstName fn
			ON sr.FirstNameID=fn.ID
		LEFT JOIN p_LastName ln
			ON sr.LastnameID=ln.ID
		WHERE r.SiteID=@SiteID
		AND UserID=ISNULL(@AuthorizedUserID,UserID)
		AND ISNULL(r.ProfileID,'')=ISNULL(@ProfileID,ISNULL(r.ProfileID,''))
		AND ISNULL(FirstName,'')=ISNULL(@FirstName,ISNULL(FirstName,''))
		AND ISNULL(MiddleInitial,'')=ISNULL(@MiddleInitial,ISNULL(MiddleInitial,''))
		AND ISNULL(LastName,'')=ISNULL(@LastName,ISNULL(LastName,''))
		AND ISNULL(r.State,'')=ISNULL(@State,ISNULL(r.State,''))
		AND ISNULL(oi.OrderID,0)=ISNULL(@OrderID,ISNULL(oi.OrderID,0))




GO
