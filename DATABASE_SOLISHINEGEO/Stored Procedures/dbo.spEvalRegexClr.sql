CREATE PROCEDURE [dbo].[spEvalRegexClr] (@Regex [nvarchar] (max), @Input [nvarchar] (max))
WITH EXECUTE AS CALLER
AS EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.RegEx].[EvalRegex]
GO
