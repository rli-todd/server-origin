SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spUserUpdate](
  @SiteID TINYINT=NULL,
  @ExternalUserID INT=NULL,
  @UserID INT=NULL,
  @VisitID INT,
  @EmailAddress VARCHAR(128)=NULL,
  @FirstName VARCHAR(30)=NULL,
  @MiddleName VARCHAR(30)=NULL,
  @LastName VARCHAR(30)=NULL,
  @HasAcceptedUserAgreement BIT=NULL,
  @HasValidPaymentMethod BIT=NULL,
	@CardCVV VARCHAR(10)=NULL,
	@CardHash BINARY(20)=NULL,
	@CardLast4 CHAR(4)=NULL,
	@CardExpiry CHAR(4)=NULL,
	@CardholderName VARCHAR(128)=NULL,
	@CardAddress VARCHAR(128)=NULL,
	@CardCity VARCHAR(128)=NULL,
	@CardState CHAR(2)=NULL,
	@CardCountry CHAR(2)=NULL,
	@CardZip CHAR(10)=NULL,
	@CardLastModified DATETIME=NULL,
  @IwsUserToken VARCHAR(50)=NULL,
  @StorefrontUserToken VARCHAR(50)=NULL,
  @UpdateDateLastAuthenticated BIT=NULL)
AS
  SET NOCOUNT ON
  IF @UserID IS NULL
    SELECT @UserID=ID
      FROM Users
      WHERE SiteID=@SiteID
      AND ExternalID=@ExternalUserID

  UPDATE Users SET
	EmailAddress=ISNULL(@EmailAddress,EmailAddress),
	FirstName=ISNULL(@FirstName,FirstName),
	MiddleName=ISNULL(@MiddleName,MiddleName),
	LastName=ISNULL(@LastName,LastName),
	HasAcceptedUserAgreement=ISNULL(@HasAcceptedUserAgreement,HasAcceptedUserAgreement),
	HasValidPaymentMethod=ISNULL(@HasValidPaymentMethod,HasValidPaymentMethod),
	CardCVV=ISNULL(@CardCVV,CardCVV),
	CardHash=ISNULL(@CardHash,CardHash),
	CardLast4=ISNULL(@CardLast4,CardLast4),
	CardExpiry=ISNULL(@CardExpiry,CardExpiry),
	CardholderName=ISNULL(@CardholderName,CardholderName),
	CardAddress=ISNULL(@CardAddress,CardAddress),
	CardCity=ISNULL(@CardCity,CardCity),
	CardState=ISNULL(@CardState,CardState),
	CardCountry=ISNULL(@CardCountry,CardCountry),
	CardZip=ISNULL(@CardZip,CardZip),
	CardLastModified=ISNULL(@CardLastModified,CardLastModified),
	LastVisitID=ISNULL(@VisitID,LastVisitID),
	ExternalID=ISNULL(@ExternalUserID,ExternalID),
	DateLastAuthenticated=
	CASE 
		WHEN ISNULL(@UpdateDateLastAuthenticated,0)=1 
		THEN GETDATE() 
		ELSE DateLastAuthenticated
	END
	WHERE ID=@UserID

  IF @IwsUserToken IS NOT NULL
  EXEC spVisitUpdateIwsUserToken 
		@SiteID=@SIteID,
    @VisitID=@VisitID, 
    @UserID=@UserID,
    @IwsUserToken=@IwsUserToken,
    @IwsUserID=@ExternalUserID,
		@StorefrontUserToken=@StorefrontUserToken,
    @ReturnVisit=0

  SELECT * 
    FROM Users
    WHERE ID=@UserID


GO
