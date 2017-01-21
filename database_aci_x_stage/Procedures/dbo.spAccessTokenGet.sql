SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spAccessTokenGet(@AccessToken VARCHAR(128)=NULL, @RefreshToken VARCHAR(128)=NULL) AS
	SET NOCOUNT ON
	IF @AccessToken IS NOT NULL
		SELECT *
			FROM AccessToken
			WHERE AccessToken=@AccessToken
			AND AccessTokenExpiry > GETUTCDATE()

	ELSE
		SELECT *
			FROM AccessToken
			WHERE RefreshToken=@RefreshToken
			AND RefreshTokenExpiry > GETUTCDATE()
GO
