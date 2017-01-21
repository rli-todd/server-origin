CREATE AGGREGATE [dbo].[fnConcatenate] (@input [nvarchar] (max))
RETURNS [nvarchar] (max)
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.Concatenate]
GO
