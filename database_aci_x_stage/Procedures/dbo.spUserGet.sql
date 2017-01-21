SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC spUserGet(@SiteID TINYINT, @UserKeys ID_TABLE READONLY) AS
	SET NOCOUNT ON;
	SELECT * 
		FROM Users u
		WHERE SiteID=@SiteID
		AND EXISTS (
			SELECT 1
				FROM @UserKeys k
				WHERE k.ID=u.ID
		)
GO
