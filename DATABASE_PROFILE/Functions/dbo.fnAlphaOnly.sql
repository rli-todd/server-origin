
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO



CREATE FUNCTION [dbo].[fnAlphaOnly]( @Input VARCHAR(255) ) RETURNS VARCHAR(255) AS
BEGIN
	DECLARE 
		@Output VARCHAR(255),
		@Len INT,
		@Pos INT,
		@C CHAR
		
	SET @Output=''
	SET @Pos=1
	SET @Len=LEN(@Input)
	IF @Input IS NOT NULL WHILE @Pos<=@Len BEGIN
		SET @C=LOWER(SUBSTRING(@Input,@Pos,1))
		IF ASCII(@C)>=ASCII('a') AND ASCII(@C)<=ASCII('z')
			SET @Output=@Output + @C
		SET @Pos=@Pos+1
	END
	RETURN @Output
		
END


GO
