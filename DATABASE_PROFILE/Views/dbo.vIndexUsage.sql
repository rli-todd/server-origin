SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO




CREATE VIEW [dbo].[vIndexUsage] AS
  SELECT DISTINCT 
      OBJECT_NAME(sis.OBJECT_ID) 'TableName', 
      si.name 'IndexName', 
      --sc.Name 'ColumnName',
      si.Index_ID, 
      sis.user_seeks, 
      sis.user_scans, 
      sis.user_lookups, 
      sis.user_updates
    FROM sys.dm_db_index_usage_stats sis
    INNER JOIN sys.indexes si 
      ON sis.OBJECT_ID = si.OBJECT_ID 
      AND sis.Index_ID = si.Index_ID
    --INNER JOIN sys.index_columns sic 
    --  ON sis.OBJECT_ID = sic.OBJECT_ID 
    --  AND sic.Index_ID = si.Index_ID
    --INNER JOIN sys.columns sc 
    --  ON sis.OBJECT_ID = sc.OBJECT_ID 
    --  AND sic.Column_ID = sc.Column_ID
    WHERE sis.Database_ID = DB_ID(DB_NAME()) 





GO
