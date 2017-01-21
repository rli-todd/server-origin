CREATE PROCEDURE [dbo].[spClrExecuteAndCompress]
	@CommandText nvarchar(max),
	@BinaryFormat [bit]
AS
	EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.GzipDataSet].[ExecuteCommand]
GO
