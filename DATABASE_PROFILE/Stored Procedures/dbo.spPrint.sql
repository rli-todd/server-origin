SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spPrint](
	@P1 VARCHAR(MAX)='',
	@P2 VARCHAR(MAX)='',
	@P3 VARCHAR(MAX)='',
	@P4 VARCHAR(MAX)='',
	@P5 VARCHAR(MAX)='',
	@P6 VARCHAR(MAX)='',
	@P7 VARCHAR(MAX)='',
	@P8 VARCHAR(MAX)='',
	@P9 VARCHAR(MAX)='',
	@P10 VARCHAR(MAX)='',
	@P11 VARCHAR(MAX)='',
	@P12 VARCHAR(MAX)='',
	@P13 VARCHAR(MAX)='',
	@P14 VARCHAR(MAX)='',
	@P15 VARCHAR(MAX)='',
	@P16 VARCHAR(MAX)='',
	@P17 VARCHAR(MAX)='',
	@P18 VARCHAR(MAX)='',
	@P19 VARCHAR(MAX)='',
  @P20 VARCHAR(MAX)='') AS
	SET NOCOUNT ON
	SET ANSI_WARNINGS OFF -- suppress "String or binary data would be truncated" message
	DECLARE @Message VARCHAR(MAX)
	DECLARE @Part VARCHAR(2000)
	DECLARE @LenMessage INT
	DECLARE @LenPart INT
	DECLARE @MaxPartLen INT
	SET @MaxPartLen=1000
	SET @Message = 
		ISNULL(@P1,'NULL')+
		ISNULL(@P2,'NULL')+
		ISNULL(@P3,'NULL')+
		ISNULL(@P4,'NULL')+
		ISNULL(@P5,'NULL')+
		ISNULL(@P6,'NULL')+
		ISNULL(@P7,'NULL')+
		ISNULL(@P8,'NULL')+
		ISNULL(@P9,'NULL')+
		ISNULL(@P10,'NULL')+
		ISNULL(@P11,'NULL')+
		ISNULL(@P12,'NULL')+
		ISNULL(@P13,'NULL')+
		ISNULL(@P14,'NULL')+
		ISNULL(@P15,'NULL')+
		ISNULL(@P16,'NULL')+
		ISNULL(@P17,'NULL')+
		ISNULL(@P18,'NULL')+
		ISNULL(@P19,'NULL')+
		ISNULL(@P20,'NULL')
	SET @Message = CONVERT(VARCHAR,GETDATE(),108)+': '+@Message
	SET @LenMessage = LEN(@Message)
	WHILE @LenMessage > 0 BEGIN
		SELECT @LenPart = CASE WHEN @LenMessage > @MaxPartLen THEN @MaxPartLen ELSE @LenMessage END
		WHILE(@LenMessage>@LenPart AND SUBSTRING(@Message,@LenPart,1) NOT IN (' ',CHAR(10),CHAR(13),CHAR(9)))
			SET @LenPart=@LenPart+1
		SELECT @Part=SUBSTRING(@Message,1,@LenPart)
		RAISERROR (@Part, 0, 1) WITH NOWAIT;
		SELECT @Message = CASE WHEN @LenMessage>@LenPart THEN SUBSTRING(@Message,@LenPart+1,@LenMessage-@LenPart) ELSE '' END
		SELECT @LenMessage=LEN(@Message)	
	END
GO
