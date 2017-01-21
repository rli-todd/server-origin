SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spUtilRebuildFragmentedIndexes]( @ReorganizeThreshold INT=10, @RebuildThreshold INT=30) AS
SET NOCOUNT ON;
DECLARE 
  @DBID int,
  @ObjectID int,
  @IndexID int,
  @SchemaName nvarchar(130), 
  @ObjectName nvarchar(130), 
  @IndexName nvarchar(130), 
  @PartitionNum bigint,
  @PartitionCount bigint,
  @Fragmentation float,
  @SQL nvarchar(4000); 

SET @DBID = DB_ID();
-- Conditionally select tables and indexes from the sys.dm_db_index_physical_stats function 
-- and convert object and index IDs to names.
SELECT
    object_id 'ObjectID',
    index_id  'IndexID',
    partition_number 'PartitionNum',
    avg_fragmentation_in_percent 'Frag'
  INTO #partitions
  FROM sys.dm_db_index_physical_stats (@DBID, NULL, NULL , NULL, 'LIMITED')
  WHERE avg_fragmentation_in_percent > @ReorganizeThreshold AND index_id > 0;

DECLARE partitions CURSOR FOR SELECT * FROM #partitions;
OPEN partitions;
FETCH NEXT FROM partitions INTO @ObjectID, @IndexID, @PartitionNum, @Fragmentation;
WHILE (@@FETCH_STATUS=0)
BEGIN;
  SELECT @ObjectName = QUOTENAME(o.name), @SchemaName = QUOTENAME(s.name)
    FROM sys.objects o
    JOIN sys.schemas s 
      ON s.schema_id = o.schema_id
    WHERE o.object_id = @ObjectID;
  
  SELECT @IndexName = QUOTENAME(name)
    FROM sys.indexes
    WHERE  object_id = @ObjectID 
    AND index_id = @IndexID;
  
  SELECT @PartitionCount = COUNT(*)
    FROM sys.partitions
    WHERE object_id = @ObjectID 
    AND index_id = @IndexID;

  SET @SQL = N'ALTER INDEX ' + @IndexName + N' ON ' + @SchemaName + N'.' + @ObjectName + 
    CASE WHEN @Fragmentation < @RebuildThreshold THEN N' REORGANIZE' ELSE N' REBUILD' END +
    CASE WHEN @PartitionCount > 1 THEN N' PARTITION=' + CAST(@PartitionNum AS NVARCHAR(10)) ELSE '' END
  EXEC spPrint 'Executing: ', @SQL, ' (fragmentation=', @Fragmentation, ')'
  EXEC (@SQL);
  FETCH NEXT FROM partitions INTO @ObjectID, @IndexID, @PartitionNum, @Fragmentation;
END;
CLOSE partitions;
DEALLOCATE partitions;
DROP TABLE #partitions;
GO
