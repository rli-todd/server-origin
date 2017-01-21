SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spFirstNameCreatePartial] AS
  SET NOCOUNT ON
  DECLARE @Rows INT=1
  /*
  ** Probably not the best place for this..
  ** COnvert all the names with double quotes to single quotes
  */
  WHILE @Rows>0
  BEGIN
    UPDATE FirstName SET FirstName=REPLACE(FirstName,'''''','''')
      WHERE FirstName LIKE '%''''%'
    SET @ROws=@@ROWCOUNT
  END
  SET @Rows=1
  WHILE @Rows>0
  BEGIN
    INSERT INTO FirstName(FirstName,IsPartial)
      SELECT DISTINCT SUBSTRING(fn.FirstName,1,LEN(fn.FirstName)-1),1
        FROM FirstName fn
        WHERE LEN(fn.FirstName)>1
        AND NOT EXISTS (
          SELECT 1
            FROM FirstName fn2
            WHERE fn2.FirstName=SUBSTRING(fn.FirstName,1,LEN(fn.FirstName)-1)
        )
    SET @ROws=@@ROWCOUNT
    EXEC spPrint @Rows, 'Partial FirstName rows added'
  END
GO
