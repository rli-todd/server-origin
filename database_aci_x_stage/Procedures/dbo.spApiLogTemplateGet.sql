SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spApiLogTemplateGet]( @RequestTemplate VARCHAR(255) ) AS
	SET NOCOUNT ON
	DECLARE @ApiLogTemplateID INT
	BEGIN TRAN
		SELECT @ApiLogTemplateID=ID FROM ApiLogTemplate WHERE Template=ISNULL(@RequestTemplate,'???')
		IF @ApiLogTemplateID IS NULL BEGIN
			INSERT INTO ApiLogTemplate(Template,DateCreated)VALUES(ISNULL(@RequestTemplate,'???'),GETUTCDATE())
			SET @ApiLogTemplateID=SCOPE_IDENTITY()
		END
	COMMIT
	RETURN @ApiLogTemplateID

GO
