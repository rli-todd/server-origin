SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spSearchResultsSetProfileIDs]( @QueryID INT, @ListProfileIDs VARCHAR(MAX)) AS
  SET NOCOUNT ON
  UPDATE SearchResults SET DateConsumed=GETDATE(),ListProfileIDs=@ListProfileIDs
    WHERE ID=@QueryID

GO
