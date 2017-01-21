CREATE FUNCTION [dbo].[UncompressBlob] (@Input [varbinary] (max))
RETURNS [nvarchar] (max)
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[UncompressBlob]
GO
