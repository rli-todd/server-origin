
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spUtilUpdateSqlClrAssembly](@FileFolder VARCHAR(MAX)='C:\temp\') AS
  SET NOCOUNT ON
  /*
  ** Would be nice if we could automate the scripting of these objects, but they
  ** seem not to be stored in syscomments or available through sp_helptext.   
  ** I think they must be generated on the fly by SSMS when scripting objects.
  */
  DECLARE @SQL NVARCHAR(MAX)='
DROP FUNCTION [dbo].[fnRefererSearchTerm]
DROP FUNCTION [dbo].[fnIpAddressToString]
DROP FUNCTION [dbo].[fnIpAddressToInt]
DROP FUNCTION [dbo].[fnCidrToLowInt]
DROP FUNCTION [dbo].[fnCidrToHighInt]
DROP FUNCTION [dbo].[fnStringSimilarity]
DROP FUNCTION [dbo].[fnDnsForwardLookup]
DROP FUNCTION [dbo].[fnDnsReverseLookup]
DROP AGGREGATE [dbo].[fnConcatenate]
DROP AGGREGATE [dbo].[fnConcatenateNoDups]
DROP PROC [dbo].[spParseUrlClr]
DROP PROC [dbo].[spClrExecuteAndCompress]
DROP PROC [dbo].[spEvalRegexClr]
DROP PROC [dbo].[spCensusQueryAcsClr]
DROP ASSEMBLY [Solishine.Web.SqlFunctions]
CREATE ASSEMBLY [Solishine.Web.SqlFunctions] FROM ''' + @FileFolder + 'Solishine.Web.SqlFunctions.dll'' WITH PERMISSION_SET=UNSAFE
'
  EXEC (@SQL)

  EXEC (N'
CREATE PROC dbo.spClrExecuteAndCompress(@CommandText NVARCHAR(MAX), @BinaryFormat BIT=1)
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.GzipDataSet].[ExecuteCommand]')

  EXEC (N'
CREATE FUNCTION dbo.fnIpAddressToInt(@IpAddress [nvarchar](255))
RETURNS [int] 
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[IpAddrToInt]')

  EXEC (N'
CREATE FUNCTION dbo.fnCidrToLowInt(@IpAddress [nvarchar](255))
RETURNS [int] 
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[CidrToLowInt]')

  EXEC (N'
CREATE FUNCTION dbo.fnCidrToHighInt(@IpAddress [nvarchar](255))
RETURNS [int] 
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[CidrToHighInt]')

  EXEC (N'
CREATE FUNCTION dbo.fnIpAddressToString(@IpAddress [int])
RETURNS [nvarchar](255) 
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[IpAddrToString]')

  EXEC (N'
CREATE FUNCTION dbo.fnRefererSearchTerm(@RefererRegex [nvarchar](255), @Referer [nvarchar](512))
RETURNS [nvarchar](512) 
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.RegEx].[GetRefererSearchTerm]')

  EXEC (N'
  CREATE PROC dbo.spEvalRegexClr
  @Regex NVARCHAR(MAX),
	@Input NVARCHAR(MAX)
WITH EXECUTE AS CALLER
AS
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.RegEx].[EvalRegex]')

  EXEC (N'CREATE PROC dbo.spParseUrlClr
  @Url NVARCHAR(MAX),
  @Scheme NVARCHAR(10) OUT,
  @ServerName NVARCHAR(255) OUT,
  @DomainName NVARCHAR(255) OUT,
  @Path NVARCHAR(255) OUT,
  @QueryString NVARCHAR(255) OUT
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[ParseUrl]')

  EXEC (N'CREATE PROC dbo.spCensusQueryAcsClr
  @BaseUrl NVARCHAR(MAX),
  @ApiKey NVARCHAR(MAX),
  @In NVARCHAR(255)=NULL,
  @For NVARCHAR(255)=NULL,
  @Get NVARCHAR(MAX)
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.Census].[QueryAcs]')

 EXEC (N'CREATE FUNCTION [dbo].[fnStringSimilarity](
 @s1 [nvarchar](max), 
 @s2 [nvarchar](max), 
 @normalize [bit], 
 @strAlgorithm [nvarchar](20) = N''Levenshtein'')
RETURNS [float] WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.StringHelper].[GetSimilarity]')

  EXEC (N'CREATE AGGREGATE [dbo].fnConcatenate(@input NVARCHAR(MAX)) 
RETURNS NVARCHAR(max)
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.Concatenate]')

  EXEC (N'CREATE AGGREGATE [dbo].fnConcatenateNoDups(@input NVARCHAR(MAX)) 
RETURNS NVARCHAR(max)
EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.ConcatenateNoDups]')

	EXEC (N'
	CREATE FUNCTION dbo.fnDnsForwardLookup(@Hostname NVARCHAR(MAX))
	RETURNS NVARCHAR(MAX)
	WITH EXECUTE AS CALLER
	AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.DnsHelper].[ForwardLookup]')

	EXEC (N'
	CREATE FUNCTION dbo.fnDnsReverseLookup(@IpAddress NVARCHAR(MAX))
	RETURNS NVARCHAR(MAX)
	WITH EXECUTE AS CALLER
	AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.DnsHelper].[ReverseLookup]')

GO
