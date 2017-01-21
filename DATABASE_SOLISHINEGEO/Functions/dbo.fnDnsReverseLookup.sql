CREATE FUNCTION [dbo].[fnDnsReverseLookup] (@IpAddress [nvarchar] (max))
RETURNS [nvarchar] (max)
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.DnsHelper].[ReverseLookup]
GO
