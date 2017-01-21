SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION dbo.fnSplit
(
	@RowData nvarchar(2000),
	@SplitOn nvarchar(5)
)  
RETURNS @RetVal TABLE
(
	ID INT IDENTITY(1,1),
	Data NVARCHAR(100)
) 
AS  
BEGIN 
	DECLARE @Cnt INT
	SET @Cnt = 1

	WHILE (CHARINDEX(@SplitOn,@RowData)>0)
	BEGIN
		INSERT INTO @RetVal (data)
		SELECT 
			Data = LTRIM(RTRIM(SUBSTRING(@RowData,1,CHARINDEX(@SplitOn,@RowData)-1)))

		SET @RowData = SUBSTRING(@RowData,CHARINDEX(@SplitOn,@RowData)+1,LEN(@RowData))
		SET @Cnt = @Cnt + 1
	END
	
	INSERT INTO @RetVal (data)
	SELECT Data = LTRIM(RTRIM(@RowData))
	RETURN
END
GO
