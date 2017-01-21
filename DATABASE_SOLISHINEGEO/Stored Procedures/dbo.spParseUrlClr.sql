CREATE PROCEDURE [dbo].[spParseUrlClr] (@Url [nvarchar] (max), @Scheme [nvarchar] (10) OUTPUT, @ServerName [nvarchar] (255) OUTPUT, @DomainName [nvarchar] (255) OUTPUT, @Path [nvarchar] (255) OUTPUT, @QueryString [nvarchar] (255) OUTPUT)
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.UDF].[ParseUrl]
GO
