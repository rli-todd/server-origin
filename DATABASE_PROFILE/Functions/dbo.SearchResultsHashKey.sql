SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[SearchResultsHashKey]( 
	@SearchType INT,
	@FirstName VARCHAR(30),
	@MiddleName VARCHAR(30),
	@LastName VARCHAR(30),
	@State VARCHAR(2)) RETURNS BINARY(20) AS 
BEGIN
	RETURN HASHBYTES('sha1',
		CONVERT(VARCHAR,ISNULL(@SearchType,0)) + '|' +
		ISNULL(@FirstName,'') + '|' +
		ISNULL(@MiddleName,'') + '|' +
		ISNULL(@LastName,'') + '|' +
		ISNULL(@State,''))
END

GO
