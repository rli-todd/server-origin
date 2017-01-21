SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spRejectedNameUpdate] AS
  SET NOCOUNT ON
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
  INSERT INTO RejectedName(FirstNameID,LastNameID)
    SELECT DISTINCT FirstNameID,LastNameID
      FROM SearchResults
      JOIN FIrstName fn
        ON fn.ID=FirstNameID
      JOIN LastName ln
        ON ln.ID=LastNameID
      WHERE NOT EXISTS 
      (
        SELECT 1 
          FROM RejectedName
          WHERE FirstNameID=fn.ID
          AND LastNameID=ln.ID
      )
      AND dbo.fnIsNameRejected(FirstName,LastName)=1
  EXEC spPrint @@ROWCOUNT, ' rejected names added'

GO
