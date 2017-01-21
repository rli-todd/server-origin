SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spCartUpdate(
	@SiteID TINYINT, 
	@VisitID INT, 
	@OrderID INT=NULL,
	@ProductID INT=NULL,
	@QueryID INT=NULL,
	@ProfileID CHAR(11)=NULL,
	@State CHAR(2)=NULL) AS
	SET NOCOUNT ON

	DECLARE @CartID INT
	SELECT TOP 1 @CartID=ID
		FROM Cart 
		WHERE SiteID=@SiteID
		AND VisitID=@VisitID
		AND OrderID IS NULL
		ORDER BY DateModified DESC
	
	IF @CartID IS NULL 
	BEGIN
		INSERT INTO Cart(SiteID,VisitID,ProductID,QueryID,ProfileID,State,DateCreated,DateModified)
			VALUES (@SiteID,@VisitID,@ProductID,@QueryID,@ProfileID,@State,GETDATE(),GETDATE())
		SET @CartID=SCOPE_IDENTITY()
	END

	IF @OrderID IS NOT NULL
	BEGIN
		UPDATE Cart SET
			OrderID=@OrderID,
			DateModified=GETDATE()
			WHERE SiteID=@SiteID
			AND ID=@CartID
	END
	ELSE
		UPDATE Cart SET 
			ProductID=@ProductID,
			QueryID=@QueryID,
			ProfileID=@ProfileID,
			State=@State,
			DateModified=GETDATE()
			WHERE SiteID=@SiteID
			AND ID=@CartID
GO
