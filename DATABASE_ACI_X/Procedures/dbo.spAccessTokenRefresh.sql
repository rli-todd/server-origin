SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spAccessTokenRefresh(@RefreshToken VARCHAR(128)) AS
	SET NOCOUNT ON
	DECLARE @AccessToken VARCHAR(128)=NEWID()
	IF EXISTS (
		SELECT 1
			FROM AccessToken
			WHERE RefreshToken=@RefreshToken
			AND RefreshTokenExpiry > GETUTCDATE()
	)
	BEGIN
		UPDATE AccessToken SET AccessToken=@AccessToken
			WHERE RefreshToken=@RefreshToken
			AND RefreshTokenExpiry > GETUTCDATE()
		EXEC spAccessTokenGet @AccessToken=@AccessToken
	END
	ELSE
		RAISERROR(N'InvalidRefreshToken',11,1);
GO
