
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spStateOffensesCreate] AS
  SET NOCOUNT ON
  DECLARE 
    @NationalPop VARCHAR(20),
    @NationalPop100k VARCHAR(20),
    @TableCols NVARCHAR(MAX),
    @NationalSQL NVARCHAR(MAX),
    @CteSql NVARCHAR(MAX),
    @SelectSql NVARCHAR(MAX),
    @CreateSql NVARCHAR(MAX),
    @InsertSql NVARCHAR(MAX),
    @GroupSql NVARCHAR(MAX),
    @SQL NVARCHAR(MAX)

  DECLARE @Measures TABLE(MeasureName VARCHAR(50))
  INSERT INTO @Measures(MeasureName)VALUES
  ('ViolentCrime'),
  ('MurderAndNonNegligentManslaughter'),
  ('ForcibleRape'),
  ('Robbery'),
  ('AggravatedAssault'),
  ('PropertyCrime'),
  ('Burglary'),
  ('LarcenyTheft'),
  ('MotorVehicleTheft'),
  ('Arson')

  SELECT @TableCols=dbo.fnConcatenateNoDups(MeasureName+'Per100kNatlAvg REAL')
    FROM @Measures

  SELECT 
    @NationalPop=CONVERT(VARCHAR(20),SUM(Population)),
    @NationalPop100k=CONVERT(VARCHAR(20),CONVERT(REAL,SUM(Population))/100000)
    FROM xvStatePopulation WITH (NOEXPAND)

  SELECT StateFips,ROW_NUMBER() OVER(ORDER BY Population DESC)'PopulationRankState'
    INTO #population_rank
    FROM xvStatePopulation

  SELECT 
    @NationalSQL = dbo.fnConcatenateNoDups('
      CONVERT(REAL,SUM(ISNULL('+MeasureName+',0)))/'+@NationalPop100k+'''' + MeasureName + 'Per100kNatlAvg'''),
    @CteSQL = dbo.fnConcatenateNoDups('
      CONVERT(REAL,SUM(ISNULL(o.'+MeasureName+',0)))'''+MeasureName+''',
      '+MeasureName+'Per100kNatlAvg'),
    @SelectSql = dbo.fnConcatenateNoDups('
      '+MeasureName+',
      '+MeasureName+'/Population100k'''+MeasureName+'Per100k'',
      '+MeasureName+'Per100kNatlAvg,
      CASE 
        WHEN '+MeasureName+'/Population100k<'+MeasureName+'Per100kNatlAvg 
        THEN ''lower'' 
        ELSE ''higher'' 
      END'''+MeasureName+'RelNatlAvg'''),
    @CreateSql=dbo.fnConcatenateNoDups('
      '+MeasureName+' INT,
      '+MeasureName+'Per100k REAL,
      '+MeasureName+'Per100kNatlAvg REAL,
      '+MeasureName+'RelNatlAvg VARCHAR(10),
      '+MeasureName+'Per100kMinState VARCHAR(30),
      '+MeasureName+'Per100kMaxState VARCHAR(30),
      '+MeasureName+'Per100kMinStateValue REAL,
      '+MeasureName+'Per100kMaxStateValue REAL'),
    @InsertSql=dbo.fnConcatenateNoDups('
      '+MeasureName+',
      '+MeasureName+'Per100k,
      '+MeasureName+'Per100kNatlAvg,
      '+MeasureName+'RelNatlAvg'),
    @GroupSql=dbo.fnConcatenateNoDups(MeasureName+'Per100kNatlAvg')
    FROM @Measures

  EXEC ('DROP TABLE StateOffenses')
  SET @SQL = '
    CREATE TABLE StateOffenses(
      StateFips TINYINT,
      StateName VARCHAR(20),
      Year INT,
      Population INT,
      Population100k REAL,
      PopulationRankState INT,
      LandAreaState INT,
      LandAreaRankState INT,
      ' + @CreateSQL + ')'

  EXEC (@SQL)

  SET @SQL = '
    SELECT ' + @NationalSql + '
    INTO #national
    FROM GeoStateCityOffenses2012;

    WITH cte AS
    (
      SELECT 
        gs.StateFips,gs.StateName,
        CONVERT(REAL,xvsp.Population)''Population'',
        CONVERT(REAL,xvsp.Population)/100000''Population100k'',
        ' + @CteSQL + '
        FROM #national,dbo.GeoStateCityOffenses2012 o
        RIGHT JOIN dbo.GeoState gs
          ON gs.StateFips=o.StateFips
        RIGHT JOIN xvStatePopulation xvsp WITH (NOEXPAND)
          ON xvsp.StateFips=gs.StateFips
        GROUP BY gs.StateFips,gs.StateName,xvsp.Population,' + @GroupSql + '
    )
      INSERT INTO StateOffenses(StateFips,StateName,Year,Population,Population100k,PopulationRankState,LandAreaState,LandAreaRankState,
      '+@InsertSql+')
      SELECT 
        cte.StateFips,cte.StateName,''2012'',Population,Population100k,PopulationRankState,gs.LandArea,gs.LandAreaRank,
        ' + @SelectSql + '
        FROM cte 
        JOIN #population_rank pr
          ON pr.StateFips=cte.StateFips
        JOIN GeoState gs
          ON gs.StateFips=cte.StateFips
        
     CREATE UNIQUE CLUSTERED INDEX IX_StateOffenses ON StateOffenses(StateFips)'

 EXEC spPrint @SQL
  EXEC (@SQL)
GO
