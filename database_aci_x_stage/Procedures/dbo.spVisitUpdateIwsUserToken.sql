SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spVisitUpdateIwsUserToken]( 
		@SiteID TINYINT,
    @VisitID INT, 
    @UserID INT,
    @IwsUserToken VARCHAR(50)=NULL, 
		@StorefrontUserToken VARCHAR(50)=NULL,
    @IwsUserID INT,
    @ReturnVisit BIT=1) AS
  SET NOCOUNT ON
  UPDATE Visit SET 
      UserID=@UserID,
      IwsUserToken=ISNULL(@IwsUserToken,IwsUserToken), 
      IwsUserTokenExpiry=DATEADD(day,1,GETDATE()),
      IwsUserID=ISNULL(@IwsUserID,IwsUserID),
			StorefrontUserToken=ISNULL(@StorefrontUserToken,StoreFrontUserToken),
      DateModified=GETDATE()
    WHERE ID=@VisitID
  
  IF @ReturnVisit=1
    EXEC spVisitGet @SiteID=@SiteID,@VisitID=@VisitID
GO
