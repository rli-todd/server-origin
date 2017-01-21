SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spUserCreate](
  @SiteID TINYINT,
  @ExternalID INT,
  @VisitID INT,
  @EmailAddress VARCHAR(128),
  @FirstName VARCHAR(30),
	@MiddleName VARCHAR(30)=NULL,
  @LastName VARCHAR(30),
  @HasAcceptedUserAgreement BIT,
	@IsBackofficeReader BIT,
  @IwsUserToken VARCHAR(50),
	@StorefrontUserToken VARCHAR(50))
AS
  SET NOCOUNT ON
  DECLARE @UserID INT

  IF EXISTS (
    SELECT 1
      FROM Users
      WHERE SiteID=@SiteID
      AND ExternalID=@ExternalID
  )
  BEGIN
    RAISERROR('Conflict: User already exists with that ID',11,1);
    RETURN;
  END

	IF EXISTS (SELECT 1 FROM InteliusAdmin WHERE EmailAddress=@EmailAddress)
		SET @IsBackofficeReader=1
  INSERT INTO Users(SiteID,UserGuid,FirstVisitID,LastVisitID,ExternalID,FirstName,MiddleName,LastName,
                    EmailAddress,HasAcceptedUserAgreement,HasValidPaymentMethod,
										IsBackOfficeReader)
    VALUES (@SiteID,NEWID(),@VisitID,@VisitID,@ExternalID,@FirstName,@MiddleName,@LastName,
              @EmailAddress,@HasAcceptedUserAgreement,0,
							@IsBackOfficeReader)
  SET @UserID=SCOPE_IDENTITY()

  EXEC spVisitUpdateIwsUserToken 
		@SiteID=@SiteID,
    @VisitID=@VisitID, 
    @UserID=@UserID,
    @IwsUserToken=@IwsUserToken,
    @IwsUserID=@ExternalID,
		@StorefrontUserToken=@StorefrontUserToken,
    @ReturnVisit=0

  SELECT * 
    FROM Users
    WHERE ID=@UserID


GO
