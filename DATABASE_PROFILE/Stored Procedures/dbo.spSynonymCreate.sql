SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROC [dbo].[spSynonymCreate]( @ServerName VARCHAR(50), @DatabaseName VARCHAR(50), @ObjectName VARCHAR(50), @Prefix VARCHAR(20)='sg') AS
BEGIN
	SET NOCOUNT ON
	DECLARE @SQL NVARCHAR(MAX)
	DECLARE @SynName VARCHAR(50)
	SET @SynName=@Prefix+'_'+@ObjectName
	SET @SQL = '
	IF EXISTS (SELECT * FROM sys.synonyms WHERE name=N''' + @SynName + ''')
		DROP SYNONYM [dbo].[' + @SynName + ']
	CREATE SYNONYM [dbo].[' + @SynName + '] FOR '
	IF @ServerName IS NOT NULL 
		SET @SQL = @SQL + '[' + @ServerName + '].'
	SET @SQL = @SQL + '[' + @DatabaseName + '].dbo.[' + @ObjectName + ']'
	EXEC sp_executeSql @SQL
END


GO
