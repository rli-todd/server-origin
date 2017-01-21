CREATE FUNCTION [dbo].[CompressString] (@Input [nvarchar] (max))
RETURNS [varbinary] (max)
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[CompressString]
GO
