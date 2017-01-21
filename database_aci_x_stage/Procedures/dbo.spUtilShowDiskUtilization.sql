SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spUtilShowDiskUtilization] AS
  SET NOCOUNT ON
  CREATE TABLE #Temp (
      Name NVARCHAR(128),
      ROWS CHAR(11),
      Reserved VARCHAR(18),
      Data VARCHAR(18),
      Index_size VARCHAR(18),
      Unused VARCHAR(18)
  )

INSERT INTO #Temp
    EXEC sp_msforeachtable 'sp_spaceused ''?'''

DECLARE @sql NVARCHAR(MAX)=''
CREATE TABLE #xv (ViewName VARCHAR(50), Rows INT)
SELECT @Sql = @SQL + '
  INSERT INTO #temp EXEC sp_spaceused '''+Name+''''
  FROM sys.views WHERE Name LIKE 'xv%'

EXEC (@SQL)
SELECT Name,Rows,
  CONVERT(DECIMAL(18,3),CAST(REPLACE(reserved,' kb','') AS INT))/1000'Reserved_MB',
  CONVERT(DECIMAL(18,3),CAST(REPLACE(data,' kb','') AS INT))/1000'Data_MB',
  CONVERT(DECIMAL(18,3),CAST(REPLACE(index_size,' kb','') AS INT))/1000'Index_MB',
  CONVERT(DECIMAL(18,3),CAST(REPLACE(unused,' kb','') AS INT))/1000'Unused_MB'
  FROM #temp
  ORDER BY 3 DESC
GO
