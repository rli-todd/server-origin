CREATE FUNCTION [dbo].[fnDnsForwardLookup] (@Hostname [nvarchar] (max))
RETURNS [nvarchar] (max)
WITH EXECUTE AS CALLER
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.DnsHelper].[ForwardLookup]
GO
