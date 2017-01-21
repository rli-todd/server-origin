CREATE FUNCTION [dbo].[fnCidrToLowInt] (@IpAddress [nvarchar] (255))
RETURNS [int]
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[CidrToLowInt]
GO
