SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spApiLogView](
	@Mins INT=60,
	@ShowUnauthenticatedUsers BIT=0,
	@FirstName VARCHAR(20)=NULL, 
	@LastName VARCHAR(20)=NULL,
	@UserID INT=NULL,
  @UserID2 INT=NULL,
  @UserID3 INT=NULL,
  @UserID4 INT=NULL,
	@VisitID INT=NULL,
	@ClientIP VARCHAR(20)=NULL,
	@ApiLogTemplateID INT=NULL,
	@UserAgentID INT=NULL,
	@HttpStatusCode INT=NULL,
	@ByTemplate BIT=0,
	@ByUser BIT=0,
  @ByServer BIT=0,
  @Summary BIT=0,
	@ResolveHostNames BIT=0,
	@MinDuration INT=NULL,
	@EmailLike VARCHAR(255)=NULL,
	@TemplateLike VARCHAR(255)=NULL,
	@RequestBodyLike VARCHAR(255)=NULL,
	@PathLike VARCHAR(255)=NULL,
	@StartDate DATETIME=NULL,
	@EndDate DATETIME=NULL) AS
	SET NOCOUNT ON
	
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	SET @StartDate = ISNULL(@StartDate,DATEADD(minute,-@Mins,GETDATE()))
	SET @EndDate = ISNULL(@EndDate,GETDATE())

	IF @ResolveHostnames=1 BEGIN
		DECLARE @IpAddresses TABLE (IpAddress INT)
		INSERT INTO @IpAddresses(IpAddress) 
			SELECT DISTINCT ClientIP 
				FROM ApiLog
				WHERE LogDate>=@StartDate
				AND LogDate<@EndDate
		
		INSERT INTO Hostname(IpAddress,Hostname,DateCached)
			SELECT IpAddress,dbo.fnGetHostnameByInt(IpAddress),GETDATE()
				FROM @IpAddresses i
				WHERE NOT EXISTS (SELECT * FROM Hostname WHERE IpAddress = i.IpAddress)
	END

  IF @Summary=1
		SELECT MIN(l.LogDate)'MinLogDate',MAX(l.LogDate)'MaxLogDate',COUNT_BIG(*)'ApiCalls',
        AVG(CONVERT(BIGINT,ISNULL(ResponseSize,0)))'AvgResponseSize',
				MIN(CONVERT(BIGINT,DurationMsecs))'MinMsecs',
				MAX(CONVERT(BIGINT,DurationMsecs))'MaxMsecs',
				AVG(CONVERT(BIGINT,DurationMsecs))'AvgMsecs',
				SUM(CONVERT(BIGINT,DurationMsecs))'TotalMsecs',
				CASE 
					WHEN DATEDIFF(second,MIN(l.LogDate),MAX(l.LogDate))=0 THEN 0
					ELSE CONVERT(REAL,COUNT_BIG(*))/DATEDIFF(second,MIN(l.LogDate),MAX(l.LogDate))
				END'Calls/Second',
				CASE WHEN @ByServer=1 THEN ServerName ELSE 'Any' END'ServerName'
			FROM ApiLog l
      LEFT JOIN Users u
        ON u.ID=l.UserID
      LEFT JOIN ApiLogTemplate t
        ON t.ID=l.ApiLogTemplateID
      LEFT JOIN ApiLogPath p
        ON p.ID=l.ApiLogPathID
      LEFT JOIN Server s
        ON s.ID=l.ServerID
			WHERE l.LogDate>=@StartDate
			AND l.LogDate<@EndDate
			AND ((@ShowUnauthenticatedUsers=0 AND Firstname IS NOT NULL) OR @ShowUnauthenticatedUsers=1)
			AND (@FirstName IS NULL OR ISNULL(FirstName,'') LIKE '%'+@FirstName+'%')
			AND (@LastName IS NULL OR ISNULL(LastName,'') LIKE '%'+@LastName+'%')
			AND ISNULL(l.UserID,0) IN (ISNULL(@UserID,ISNULL(l.UserID,0)),ISNULL(@UserID2,0),ISNULL(@UserID3,0),ISNULL(@UserID4,0))
			AND HttpStatusCode=ISNULL(@HttpStatusCode,HttpStatusCode)
			AND ApiLogTemplateID=ISNULL(@ApiLogTemplateID,ApiLogTemplateID)
			AND UserAgentID=ISNULL(@UserAgentID,UserAgentID)
			AND (@TemplateLike IS NULL OR Template LIKE '%'+@TemplateLike+'%')
			AND (@RequestBodyLike IS NULL OR ISNULL(RequestBody,'') LIKE '%'+@RequestBodyLike+'%')
			AND (@PathLike IS NULL OR Path LIKE '%'+@PathLike+'%')
			AND (@EmailLike IS NULL OR EmailAddress LIKE '%'+@EmailLike+'%')
      AND (@ClientIP IS NULL OR dbo.fnIpAddressToInt(@ClientIP)=l.ClientIP)
			AND (@VisitID IS NULL OR VisitID=@VisitID)
			AND DurationMsecs>=ISNULL(@MinDuration,0)
			GROUP BY 
				CASE WHEN @ByServer=1 THEN ServerName ELSE 'Any' END
  	
	ELSE IF @ByTemplate=1 
		SELECT MIN(l.LogDate)'MinLogDate',MAX(l.LogDate)'MaxLogDate',COUNT_BIG(*)'ApiCalls',
        AVG(CONVERT(BIGINT,ISNULL(ResponseSize,0)))'AvgResponseSize',
				MIN(CONVERT(BIGINT,DurationMsecs))'MinMsecs',
				MAX(CONVERT(BIGINT,DurationMsecs))'MaxMsecs',
				AVG(CONVERT(BIGINT,DurationMsecs))'AvgMsecs',
				SUM(CONVERT(BIGINT,DurationMsecs))'TotalMsecs',
				CASE WHEN @ByServer=1 THEN ServerName ELSE 'Any' END'ServerName',
        RequestMethod,Template,HttpStatusCode,ApiLogTemplateID
			FROM ApiLog l
      LEFT JOIN Users u
        ON u.ID=l.UserID
      LEFT JOIN ApiLogTemplate t
        ON t.ID=l.ApiLogTemplateID
      LEFT JOIN ApiLogPath p
        ON p.ID=l.ApiLogPathID
      LEFT JOIN Server s
        ON s.ID=l.ServerID
			WHERE l.LogDate>=@StartDate
			AND l.LogDate<@EndDate
			AND ((@ShowUnauthenticatedUsers=0 AND Firstname IS NOT NULL) OR @ShowUnauthenticatedUsers=1)
			AND (@FirstName IS NULL OR ISNULL(FirstName,'') LIKE '%'+@FirstName+'%')
			AND (@LastName IS NULL OR ISNULL(LastName,'') LIKE '%'+@LastName+'%')
			AND ISNULL(l.UserID,0) IN (ISNULL(@UserID,ISNULL(l.UserID,0)),ISNULL(@UserID2,0),ISNULL(@UserID3,0),ISNULL(@UserID4,0))
			AND HttpStatusCode=ISNULL(@HttpStatusCode,HttpStatusCode)
			AND ApiLogTemplateID=ISNULL(@ApiLogTemplateID,ApiLogTemplateID)
			AND UserAgentID=ISNULL(@UserAgentID,UserAgentID)
			AND (@TemplateLike IS NULL OR Template LIKE '%'+@TemplateLike+'%')
			AND (@RequestBodyLike IS NULL OR ISNULL(RequestBody,'') LIKE '%'+@RequestBodyLike+'%')
			AND (@PathLike IS NULL OR Path LIKE '%'+@PathLike+'%')
			AND (@EmailLike IS NULL OR EmailAddress LIKE '%'+@EmailLike+'%')
      AND (@ClientIP IS NULL OR dbo.fnIpAddressToInt(@ClientIP)=l.ClientIP)
			AND (@VisitID IS NULL OR VisitID=@VisitID)
			AND DurationMsecs>=ISNULL(@MinDuration,0)
			GROUP BY 
        CASE WHEN @ByServer=1 THEN ServerName ELSE 'Any' END,
        l.RequestMethod,Template,HttpStatusCode,ApiLogTemplateID
			ORDER BY TotalMsecs DESC 
	
	ELSE IF @ByUser=1
		SELECT MIN(l.LogDate)'MinLogDate',MAX(l.LogDate)'MaxLogDate',COUNT(*)'ApiCalls',
      AVG(CONVERT(BIGINT,ISNULL(ResponseSize,0)))'AvgResponseSize',
			MIN(CONVERT(BIGINT,DurationMsecs))'MinMsecs',
			MAX(CONVERT(BIGINT,DurationMsecs))'MaxMsecs',
			AVG(DurationMsecs)'AvgMsecs',
			SUM(DurationMsecs)'TotalMsecs',
				l.UserID,FirstName,LastName
			FROM ApiLog l
      LEFT JOIN Users u
        ON u.ID=l.UserID
      LEFT JOIN ApiLogTemplate t
        ON t.ID=l.ApiLogTemplateID
      LEFT JOIN ApiLogPath p
        ON p.ID=l.ApiLogPathID
      LEFT JOIN Server s
        ON s.ID=l.ServerID
			WHERE l.LogDate>=@StartDate
			AND l.LogDate<@EndDate
			AND ((@ShowUnauthenticatedUsers=0 AND Firstname IS NOT NULL) OR @ShowUnauthenticatedUsers=1)
			AND (@FirstName IS NULL OR ISNULL(FirstName,'') LIKE '%'+@FirstName+'%')
			AND (@LastName IS NULL OR ISNULL(LastName,'') LIKE '%'+@LastName+'%')
			AND ISNULL(l.UserID,0) IN (ISNULL(@UserID,ISNULL(l.UserID,0)),ISNULL(@UserID2,0),ISNULL(@UserID3,0),ISNULL(@UserID4,0))
			AND HttpStatusCode=ISNULL(@HttpStatusCode,HttpStatusCode)
			AND ApiLogTemplateID=ISNULL(@ApiLogTemplateID,ApiLogTemplateID)
			AND UserAgentID=ISNULL(@UserAgentID,UserAgentID)
			AND (@TemplateLike IS NULL OR Template LIKE '%'+@TemplateLike+'%')
			AND (@RequestBodyLike IS NULL OR ISNULL(RequestBody,'') LIKE '%'+@RequestBodyLike+'%')
			AND (@PathLike IS NULL OR Path LIKE '%'+@PathLike+'%')
			AND (@EmailLike IS NULL OR EmailAddress LIKE '%'+@EmailLike+'%')
      AND (@ClientIP IS NULL OR dbo.fnIpAddressToInt(@ClientIP)=l.ClientIP)
			AND (@VisitID IS NULL OR VisitID=@VisitID)
			AND DurationMsecs>=ISNULL(@MinDuration,0)
			GROUP BY l.UserID,FirstName,LastName
			ORDER BY FirstName, LastName DESC
	ELSE
		SELECT l.LogDate,VisitID,DurationMsecs,l.UserID,FirstName,LastName,ServerName,RequestMethod,
					Path,HttpStatusCode,ResponseSIze,Template,RequestBody,RequestSize,ErrorType,ErrorMessage,
					ClientIP,dbo.fnIpAddressToString(ClientIP)'StrClientIP',UserAgent,ApiLogTemplateID,UserAgentID
			FROM ApiLog l
      LEFT JOIN Users u
        ON u.ID=l.UserID
      LEFT JOIN ApiLogTemplate t
        ON t.ID=l.ApiLogTemplateID
      LEFT JOIN ApiLogPath p
        ON p.ID=l.ApiLogPathID
      LEFT JOIN Server s
        ON s.ID=l.ServerID
			LEFT JOIN UserAgent ua
				ON ua.ID=l.UserAgentID
			WHERE l.LogDate>=@StartDate
			AND l.LogDate<@EndDate
			AND ((@ShowUnauthenticatedUsers=0 AND Firstname IS NOT NULL) OR @ShowUnauthenticatedUsers=1)
			AND (@FirstName IS NULL OR ISNULL(FirstName,'') LIKE '%'+@FirstName+'%')
			AND (@LastName IS NULL OR ISNULL(LastName,'') LIKE '%'+@LastName+'%')
			AND ISNULL(l.UserID,0) IN (ISNULL(@UserID,ISNULL(l.UserID,0)),ISNULL(@UserID2,0),ISNULL(@UserID3,0),ISNULL(@UserID4,0))
			AND HttpStatusCode=ISNULL(@HttpStatusCode,HttpStatusCode)
			AND ApiLogTemplateID=ISNULL(@ApiLogTemplateID,ApiLogTemplateID)
			AND UserAgentID=ISNULL(@UserAgentID,UserAgentID)
			AND (@TemplateLike IS NULL OR Template LIKE '%'+@TemplateLike+'%')
			AND (@RequestBodyLike IS NULL OR ISNULL(RequestBody,'') LIKE '%'+@RequestBodyLike+'%')
			AND (@PathLike IS NULL OR Path LIKE '%'+@PathLike+'%')
			AND (@EmailLike IS NULL OR EmailAddress LIKE '%'+@EmailLike+'%')
      AND (@ClientIP IS NULL OR dbo.fnIpAddressToInt(@ClientIP)=l.ClientIP)
			AND (@VisitID IS NULL OR VisitID=@VisitID)
			AND DurationMsecs>=ISNULL(@MinDuration,0)
			ORDER BY LogDate DESC
GO
