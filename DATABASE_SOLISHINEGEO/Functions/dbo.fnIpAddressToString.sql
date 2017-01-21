CREATE FUNCTION [dbo].[fnIpAddressToString] (@IpAddress [int])
RETURNS [nvarchar] (255)
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[IpAddrToString]
GO
