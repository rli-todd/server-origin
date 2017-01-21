CREATE PROCEDURE [dbo].[spCensusQueryClr]
	@Url nvarchar(max)
AS
	EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.Census].[QueryCensus]
GO
