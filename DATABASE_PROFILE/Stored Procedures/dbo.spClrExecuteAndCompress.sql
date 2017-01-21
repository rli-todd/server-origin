CREATE PROCEDURE [dbo].[spClrExecuteAndCompress] (@CommandText [nvarchar] (max), @BinaryFormat [bit]=N'True')
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.GzipDataSet].[ExecuteCommand]
GO
