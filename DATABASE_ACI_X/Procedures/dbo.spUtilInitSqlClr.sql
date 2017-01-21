SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spUtilInitSqlClr] AS 
	SET NOCOUNT ON
	EXEc sp_configure 'show advanced options', 1;
	RECONFIGURE;
	EXEC sp_configure 'clr enabled', 1;
	RECONFIGURE;
	EXEC sp_changedbowner 'sa'
	DECLARE @SQL NVARCHAR(MAX)='ALTER DATABASE '+DB_NAME()+' SET TRUSTWORTHY ON'
	EXEC (@SQL)
GO
