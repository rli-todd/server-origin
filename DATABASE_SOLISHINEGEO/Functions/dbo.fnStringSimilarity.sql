CREATE FUNCTION [dbo].[fnStringSimilarity] (@s1 [nvarchar] (max), @s2 [nvarchar] (max), @normalize [bit], @strAlgorithm [nvarchar] (20)=N'Levenshtein')
RETURNS [float]
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.StringHelper].[GetSimilarity]
GO
