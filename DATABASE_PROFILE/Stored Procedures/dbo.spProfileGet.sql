SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spProfileGet](@ProfileID CHAR(11), @ProfileAttributes VARCHAR(255)) AS
	SELECT *
		FROM Profile
		WHERE ProfileID=@ProfileID
		AND ProfileAttributes=@ProfileAttributes
GO
