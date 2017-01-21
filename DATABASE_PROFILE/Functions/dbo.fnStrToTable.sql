SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[fnStrToTable] (@StringList VARCHAR(MAX),	@Delimeter VARCHAR(10))
RETURNS @ListTable TABLE (	Item VARCHAR(MAX))
AS
BEGIN
	IF @StringList IS NOT NULL BEGIN
		IF CHARINDEX(@Delimeter,@StringList)=0 BEGIN
			INSERT INTO @ListTable
				SELECT @StringList
		END
		ELSE
      SET @StringList=@Delimeter+@StringList+@Delimeter
    INSERT INTO @ListTable(Item) 
    SELECT SUBSTRING(@StringList,N+LEN(@Delimeter),CHARINDEX(@Delimeter,@StringList,N+LEN(@Delimeter))-N-LEN(@Delimeter))   
      FROM dbo.Tally  
      WHERE N < LEN(@StringList)    
      AND SUBSTRING(@StringList,N,LEN(@Delimeter)) = @Delimeter --Notice how we find the comma 
    RETURN 
	END
	RETURN 
END
GO
