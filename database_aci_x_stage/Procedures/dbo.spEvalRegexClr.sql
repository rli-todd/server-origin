CREATE PROCEDURE [dbo].[spEvalRegexClr]
	@Regex nvarchar(max),
	@Input nvarchar(max)
AS
	EXTERNAL NAME [Solishine.Web.SqlFunctions].[Solishine.Web.SqlFunctions.RegEx].[EvalRegex]
GO
