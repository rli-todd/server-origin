SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spMiddleNameGetID]( @MiddleName VARCHAR(30)) AS
	SET NOCOUNT ON
	DECLARE 
		@MiddleNameID INT
		
	SELECT @MiddleNameID=ID FROM MiddleName WHERE MiddleName=ISNULL(@MiddleName,'')
	IF @MiddleNameID IS NULL BEGIN
		INSERT INTO MiddleName(MiddleName) VALUES (ISNULL(@MiddleName,''))
		SET @MiddleNameID=SCOPE_IDENTITY()
	END
	RETURN @MiddleNameID

GO
