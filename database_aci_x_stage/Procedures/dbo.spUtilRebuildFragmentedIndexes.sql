SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spUtilRebuildFragmentedIndexes]( @ReorganizeThreshold INT=10, @RebuildThreshold INT=30, @Online BIT=1) AS
SET NOCOUNT ON;
DECLARE 
  @DBID int,
  @ObjectID int,
  @IndexID int,
  @SchemaName nvarchar(130), 
  @TableName nvarchar(130), 
  @IndexName nvarchar(130), 
	@Operation nvarchar(30),
  @PartitionNum bigint,
  @PartitionCount bigint,
  @Fragmentation float,
  @SQL nvarchar(4000); 

SET @DBID = DB_ID();
-- Conditionally select tables and indexes from the sys.dm_db_index_physical_stats function 
-- and convert object and index IDs to names.
SELECT
    o.object_id 'ObjectID',
    i.index_id  'IndexID',
		QUOTENAME(s.name)'SchemaName',
		QUOTENAME(o.name)'TableName',
		QUOTENAME(i.name)'IndexName',
    partition_number'PartitionNum',
    CONVERT(INT,avg_fragmentation_in_percent) 'Fragmentation',
		CASE WHEN CONVERT(INT,avg_fragmentation_in_percent) < @RebuildThreshold THEN 'Reorganize' ELSE 'Rebuild' END'Operation'
  INTO #partitions
  FROM sys.dm_db_index_physical_stats (@DBID, NULL, NULL , NULL, 'LIMITED') stats
	JOIN sys.objects o
		ON o.object_id=stats.object_id
	JOIN sys.schemas s
		ON s.schema_id=o.schema_id
	JOIN sys.indexes i
		ON i.object_id=stats.object_id
		AND i.index_id=stats.index_id
  WHERE avg_fragmentation_in_percent > @ReorganizeThreshold AND stats.index_id > 0
	ORDER BY o.Name,i.Name,partition_number;

SELECT * FROM #partitions

DECLARE partitions CURSOR FOR 
	SELECT ObjectID,IndexID,SchemaName,TableName,IndexName,PartitionNum,Fragmentation,Operation 
		FROM #partitions;
OPEN partitions;
FETCH NEXT 
	FROM partitions 
	INTO @ObjectID,@IndexID,@SchemaName,@TableName,@IndexName,@PartitionNum,@Fragmentation,@Operation
WHILE (@@FETCH_STATUS=0)
BEGIN
  SELECT @PartitionCount = COUNT(*)
    FROM sys.partitions
    WHERE object_id = @ObjectID 
    AND index_id = @IndexID;

  SET @SQL = N'ALTER INDEX ' + @IndexName + N' ON ' + @SchemaName + N'.' + @TableName + ' ' + UPPER(@Operation) +
    CASE WHEN @PartitionCount > 1 THEN N' PARTITION=' + CAST(@PartitionNum AS NVARCHAR(10)) ELSE '' END +
		CASE WHEN @Online=1 AND @Operation='Rebuild' THEN N' WITH (ONLINE=ON)' ELSE '' END
  EXEC spPrint 'Executing: ', @SQL, ' (fragmentation=', @Fragmentation, ')'
  EXEC (@SQL);
	FETCH NEXT 
		FROM partitions 
		INTO @ObjectID,@IndexID,@SchemaName,@TableName,@IndexName,@PartitionNum,@Fragmentation,@Operation
END;
CLOSE partitions;
DEALLOCATE partitions;
DROP TABLE #partitions;
GO
