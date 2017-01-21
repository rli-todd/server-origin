CREATE PROCEDURE [dbo].[spCensusQueryAcsClr] (@BaseUrl [nvarchar] (max), @ApiKey [nvarchar] (max), @In [nvarchar] (255)=NULL, @For [nvarchar] (255)=NULL, @Get [nvarchar] (max))
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.Census].[QueryAcs]
GO
