SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spCompanyGetID]( @CompanyName VARCHAR(128)) AS
	SET NOCOUNT ON
	DECLARE 
		@CompanyID INT
		
	SELECT @CompanyID=ID FROM Company WHERE CompanyName=@CompanyName
	IF @CompanyID IS NULL BEGIN
		INSERT INTO Company(CompanyName) VALUES (@CompanyName)
		SET @CompanyID=SCOPE_IDENTITY()
	END
	RETURN @CompanyID

GO
