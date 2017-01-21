SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spViewCreateIndex]( 
	@ViewName VARCHAR(100), 
	@IndexColumns VARCHAR(MAX), 
	@IndexType VARCHAR(30)='UNIQUE CLUSTERED', 
	@NameSuffix VARCHAR(MAX)='',
	@IncludeColumns VARCHAR(MAX)=NULL,
  @FileGroup VARCHAR(MAX)='[PRIMARY]',
	@DropIndex BIT=0,
	@CreateIndex BIT=1) AS
	SET NOCOUNT ON
	DECLARE 
		@SQL NVARCHAR(MAX)='',
		@IndexName NVARCHAR(MAX) = 'IX_' + @ViewName + @NameSuffix;

  DECLARE
		@ExistingIndexSQL NVARCHAR(MAX)='EXISTS (
		SELECT 1 
			FROM sys.indexes 
			WHERE object_id = OBJECT_ID(N''[dbo].[' + @ViewName + ']'') 
			AND name = N''' + @IndexName+ ''')'

	IF @DropIndex=1 
		SET @SQL=@SQL + '
		IF ' + @ExistingIndexSQL + '
		BEGIN
      EXEC spPrint ''Dropping INDEX ' + @IndexName + '''
      DROP INDEX dbo.['+@ViewName+'].' + @IndexName + '
    END'

	IF @CreateIndex=1 
		SET @SQL=@SQL + '
		IF NOT ' + @ExistingIndexSql + '
	BEGIN
		EXEC spPrint ''Creating INDEX ' + @IndexName +'''
		CREATE ' + @IndexType + ' INDEX ' + @IndexName + ' ON [dbo].'+@ViewName+'
		(
			'+@IndexColumns+'
		)' + CASE WHEN @IncludeColumns IS NOT NULL THEN ' INCLUDE (' + @IncludeColumns + ')' ELSE'' END + '
		WITH (
			PAD_INDEX=ON, 
			STATISTICS_NORECOMPUTE=OFF, 
			SORT_IN_TEMPDB=OFF, 
			IGNORE_DUP_KEY=OFF, 
			DROP_EXISTING=OFF, 
			ONLINE=OFF,
			FILLFACTOR=80,
			ALLOW_ROW_LOCKS=ON, 
			ALLOW_PAGE_LOCKS=ON) ON ' + @FileGroup + '
		EXEC spPrint ''Done''
	END'
	--EXEC spPrint @SQL
	EXEC sp_ExecuteSQL @SQL








GO
