CREATE AGGREGATE [dbo].[fnConcatenateNoDups] (	@input nvarchar(max))
	RETURNS nvarchar(max)
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.ConcatenateNoDups]
GO
