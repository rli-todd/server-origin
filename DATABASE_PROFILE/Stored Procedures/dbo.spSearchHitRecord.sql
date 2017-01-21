SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spSearchHitRecord](@SearchResultsID INT,@VisitID INT=NULL /* Ignored */) AS
  SET NOCOUNT ON
  DECLARE @HitDate DATE=GETDATE()
  IF EXISTS (
    SELECT 1 
      FROM SearchHit 
      WHERE HitDate=@HitDate
      AND SearcHResultsID=@SearchResultsID
  )
    UPDATE SearchHit SET NumHits=ISNULL(NumHits,0)+1
      WHERE HitDate=@HitDate
      AND SearchResultsID=@SearchResultsID
  ELSE
    INSERT INTO SearchHit(SearchResultsID,HitDate,NumHits)
      SELECT @SearchResultsID,GETDATE(),1




GO
