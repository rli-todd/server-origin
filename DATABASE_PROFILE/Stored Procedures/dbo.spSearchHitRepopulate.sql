SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spSearchHitRepopulate] AS
  SET NOCOUNT ON
  DROP TABLE #SH
  DECLARE 
    @D DATE='10/30/13',
    @R INT

  CREATE TABLE #SH(SearchResultsID INT, NumHits SMALLINT)

  SET ROWCOUNT 0
  WHILE EXISTS (SELECT 1 FROM PR_Restored..SearchHit)
  BEGIN
    INSERT INTO #SH(SearchResultsID,NumHits)
      SELECT SearchResultsID,COUNT(*)
        FROM PR_Restored..SearchHit
        WHERE HitDate=@D
        GROUP BY SearchResultsID
    SET @R=@@ROWCOUNT;
    EXEC spPrint @D,': ', @R, ' rows collected'

    WHILE @R>0 
    BEGIN
      SET ROWCOUNT 1000000
      INSERT INTO SearchHit(HitDate,SearchResultsID,NumHits)
        SELECT @D,SearchResultsID,NumHits
          FROM #SH sh
          WHERE NOT EXISTS (
            SELECT 1
              FROM SearchHit
              WHERE HitDate=@D
              AND SearchResultsID=sh.SearchResultsID
          )
      SET @R=@@ROWCOUNT
      EXEC spPrint @D,': ', @R, ' rows inserted'
      WAITFOR DELAY '0:0:1'
    END
    SET ROWCOUNT 0
    SET @D=DATEADD(day,-1,@D) 
    TRUNCATE TABLE #sh
  END
GO
