SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spLastNameCreatePartial] AS
  SET NOCOUNT ON
  DECLARE @Rows INT=1
  /*
  ** Probably not the best place for this..
  ** COnvert all the names with double quotes to single quotes
  */
  WHILE @Rows>0
  BEGIN
    UPDATE LastName SET LastName=REPLACE(LastName,'''''','''')
      WHERE LastName LIKE '%''''%'
    SET @ROws=@@ROWCOUNT
  END
  SET @Rows=1
  WHILE @Rows>0
  BEGIN
    INSERT INTO LastName(LastName,IsPartial)
      SELECT DISTINCT SUBSTRING(ln.LastName,1,LEN(ln.LastName)-1),1
        FROM LastName ln
        WHERE LEN(ln.LastName)>1
        AND NOT EXISTS (
          SELECT 1
            FROM LastName ln2
            WHERE ln2.LastName=SUBSTRING(ln.LastName,1,LEN(ln.LastName)-1)
        )
    SET @ROws=@@ROWCOUNT
    EXEC spPrint @Rows, 'Partial LastName rows added'
  END
GO
