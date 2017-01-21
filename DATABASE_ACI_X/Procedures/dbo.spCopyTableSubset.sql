SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


CREATE PROC [dbo].[spCopyTableSubset]( 
	@TableName VARCHAR(50), 
	@IDTableName VARCHAR(50)=NULL, 
	@IDColName VARCHAR(50)='ID',
	@SourceDB VARCHAR(50)='ACI_X',
	@PrintSql BIT=0) AS
	SET NOCOUNT ON

	DECLARE 
		@ColNames NVARCHAR(MAX)='',
		@SQL NVARCHAR(MAX)='',
		@IsIdentity BIT=0

	IF EXISTS(
		SELECT 1
			FROM information_schema.columns
			WHERE table_name=@TableName
			AND COLUMNPROPERTY(OBJECT_ID(@TableName), column_name, 'IsIdentity') = 1
	)
		SET @IsIdentity=1

	SELECT @ColNames = @ColNames + column_name + ','
		FROM information_schema.columns
		WHERE table_name=@TableName
	SET @ColNames = SUBSTRING(@ColNames,1,LEN(@ColNames)-1)

	IF @IsIdentity=1 SET @SQL = @SQL + '
	SET IDENTITY_INSERT ' + @TableName + ' ON'
	SET @SQL = @SQL + '
	INSERT INTO ' + @TableName + '(' + @ColNames + ')
		SELECT ' + @ColNames + '
			FROM ' + @SourceDB + '..' + @TableName + ' src'
	IF @IDTableName IS NOT NULL SET @SQL = @SQL + '
			WHERE EXISTS (
				SELECT 1
					FROM ' + @IDTableName + ' id
					WHERE id.ID=src.' + @IDColName + '
			)'
	IF @IDColName IS NOT NULL SET @SQL = @SQL + CASE WHEN @IDTableName IS NOT NULL THEN '
			AND' ELSE '
			WHERE' END + ' NOT EXISTS (
				SELECT 1
					FROM ' + @TableName + ' dest
					WHERE dest.' + @IDColName + '=src.' + @IDColName + '
			)'
	IF @IsIdentity=1 SET @SQL = @SQL + '
	SET IDENTITY_INSERT ' + @TableName + ' OFF'
	EXEC (@SQL)

GO
