SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spQueryGet] ( @QueryiD INT) AS
	SET NOCOUNT ON
	SELECT sr.ID,fn.FirstName,ln.LastName,sr.MiddleInitial,sr.State
		FROM p_SearchResults sr
		JOIN p_FirstName fn
			ON sr.FirstNameID=fn.ID
		JOIN p_LastName ln
			ON sr.LastNameID=ln.ID
		WHERE sr.ID=@QueryID
GO
