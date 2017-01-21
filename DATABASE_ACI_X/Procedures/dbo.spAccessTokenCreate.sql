SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spAccessTokenCreate(
	@UserID INT, 
	@AccessTokenExpirySecs INT=1200, -- 20 minutes
	@RefreshTokenExpiryHours INT=720) -- 30 days
AS
	SET NOCOUNT ON
	DECLARE @AccessToken VARCHAR(128)=NEWID()

	INSERT INTO AccessToken(AccessToken,AccessTokenExpiry,RefreshToken,RefreshTokenExpiry,DateCreated,UserID)
		SELECT 
			@AccessToken,DATEADD(second,@AccessTokenExpirySecs,GETUTCDATE()),
			NEWID(),DATEADD(hour,@RefreshTokenExpiryHours,GETUTCDATE()),
			GETUTCDATE(),@UserID
	EXEC spAccessTokenGet @AccessToken=@AccessToken
GO
