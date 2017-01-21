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
DROP PROC [dbo].[spParseUrlClr]
DROP PROC [dbo].[spClrExecuteAndCompress]
DROP PROC [dbo].[spEvalRegexClr]
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
CREATE FUNCTION dbo.fnIpAddressToString(@IpAddress [int])
RETURNS [nvarchar](255) 
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[IpAddrToString]')

  EXEC (N'
CREATE FUNCTION dbo.fnRefererSearchTerm(@RefererRegex [nvarchar](255), @Referer [nvarchar](512))
RETURNS [nvarchar](512) 
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.RegEx].[GetRefererSearchTerm]')

EXEC (N'CREATE PROC dbo.spEvalRegexClr
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
GO
