CREATE FUNCTION [dbo].[fnRefererSearchTerm] (@RefererRegex [nvarchar] (255), @Referer [nvarchar] (512))
RETURNS [nvarchar] (512)
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.RegEx].[GetRefererSearchTerm]
GO
