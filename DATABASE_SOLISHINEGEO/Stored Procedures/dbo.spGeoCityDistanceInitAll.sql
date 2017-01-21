
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoCityDistanceInitAll]( @MaxDistance INT=200) AS
  SET NOCOUNT ON
  DECLARE @SQL NVARCHAR(MAX)=''
    TRUNCATE TABLE GeoCityDistance
    SELECT @SQL = @SQL + '
    EXEC sp_async_execute 
      @Sql=''
      EXEC spGeoCityDistanceInitState 
        @StateFips='+CONVERT(VARCHAR,StateFips) + ', 
        @MaxDistance=' + CONVERT(VARCHAR,@MaxDistance) + ',
        @InsertToTempTableOnly=1'', 
      @JobName=''spGeoCityDistanceInitState ' + StateName + ''',
      @Database=''SolishineGeo'''
    FROM GeoState
    EXEC spPrint @SQL
    EXEC (@SQL)
GO
