CREATE FUNCTION [dbo].[fnCidrToHighInt] (@IpAddress [nvarchar] (255))
RETURNS [int]
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[CidrToHighInt]
GO
