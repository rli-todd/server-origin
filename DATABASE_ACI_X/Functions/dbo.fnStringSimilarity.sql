CREATE FUNCTION [dbo].[fnStringSimilarity]
(
	@s1 nvarchar(max),
	@s2 nvarchar(max),
	@normalize [bit],
	@strAlgorithm [nvarchar](20)
)
	RETURNS [float]
	WITH EXECUTE AS CALLER
AS
	EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.StringHelper].[GetSimilarity]
GO
