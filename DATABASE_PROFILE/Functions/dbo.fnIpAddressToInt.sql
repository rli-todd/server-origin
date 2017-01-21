CREATE FUNCTION [dbo].[fnIpAddressToInt] (@IpAddress [nvarchar] (255))
RETURNS [int]
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[IpAddrToInt]
GO
