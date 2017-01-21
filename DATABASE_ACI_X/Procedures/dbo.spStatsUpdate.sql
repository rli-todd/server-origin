SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC spStatsUpdate( @StatsXml XML) AS
	SET NOCOUNT ON
  SELECT 
		CONVERT(SMALLINT,0)'ApiTemplateID',
    T.item.query(N'../../MinTime').value(N'.','DATETIME')'MinTime',
    T.item.query(N'../../MaxTime').value(N'.','DATETIME')'MaxTime',
		T.item.query(N'../../ClientIP').value(N'.','VARCHAR(50)')'ClientIP',
		T.item.query(N'./Template').value(N'.','VARCHAR(MAX)')'Template',
		T.item.query(N'./Method').value(N'.','VARCHAR(MAX)')'Method',
		T.item.query(N'./HttpStatusCode').value(N'.','INT')'HttpStatusCode',
		T.item.query(N'./Hits').value(N'.','INT')'Hits',
		T.item.query(N'./TotalMsecs').value(N'.','BIGINT')'TotalMsecs'
		INTO #stats
    FROM @StatsXml
      .nodes(N'/ApiStats/Stats/ApiStat') AS T(item);

	INSERT INTO ApiTemplate(Template)
		SELECT DISTINCT Template 
			FROM #stats s
			WHERE NOT EXISTS (
				SELECT 1
					FROM ApiTemplate t
					WHERE t.Template=s.Template
			)

	UPDATE #stats SET ApiTemplateID=at.ID
		FROM #stats s
		JOIN ApiTemplate at
			ON at.Template=s.Template

	DECLARE 
		@LogPeriod SMALLDATETIME,
		@ClientIP INT,
		@PeriodMins INT=5
	SELECT 
			@LogPeriod = CONVERT(SMALLDATETIME,MAX(MinTime)), 
			@ClientIP = dbo.fnIpAddressToInt(MAX(ClientIp))
		FROM #stats
		
	SELECT @LogPeriod=DATEADD(Minute,-DATEPART(minute,@LogPeriod) % @PeriodMins,@LogPeriod);				
	UPDATE ApiTemplateStats SET
		Hits=ats.Hits+s.Hits,
		TotalMsecs=ats.TotalMsecs+s.TotalMsecs
		FROM ApiTemplateStats ats
		JOIN #stats s
			ON ats.ApiTemplateID=s.ApiTemplateID
			AND ats.LogPeriod=@LogPeriod
			AND ats.Method=s.Method
			AND ats.HttpStatusCode=s.HttpStatusCode
			AND ats.ClientIP=@CLientIP
			
	INSERT INTO ApiTemplateStats(APiTemplateID,LogPeriod,ClientIP,Method,HttpStatusCode,Hits,TotalMsecs)
		SELECT DISTINCT ApiTemplateID,@LogPeriod,@ClientIP,Method,HttpStatusCode,Hits,TOtalMsecs
			FROM #stats s
			WHERE NOT EXISTS (
				SELECT 1
					FROM ApiTemplateStats ats
					WHERE ats.ApiTemplateID=s.ApiTemplateID
					AND ats.LogPeriod=@LogPeriod
					AND ats.ClientIP=@ClientIP
					AND ats.Method=s.Method
					AND ats.HttpStatusCode=s.HttpStatusCode
			) 																																																																																																																																																																																										
	SELECT * FROM #stats



GO
