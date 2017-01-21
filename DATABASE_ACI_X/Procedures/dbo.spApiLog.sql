SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spApiLog]( 
	@SiteID TINYINT,
	@VisitID INT,
	@UserID INT=NULL, 
	@ClientIP INT=NULL,
	@UserIP INT=NULL,
	@RequestMethod VARCHAR(6)=NULL, 
	@RequestBody NVARCHAR(MAX)=NULL,
  @ResponseJson TEXT=NULL,
	@HttpStatusCode SMALLINT=NULL, 
	@DurationMsecs INT=NULL, 

	@ServerName VARCHAR(20)=NULL,
	@PathAndQuery VARCHAR(256)=NULL,
	@RequestTemplate VARCHAR(256)=NULL,
	@UserAgent VARCHAR(255)=NULL,
	@ErrorType VARCHAR(255)=NULL,
	@ErrorMessage VARCHAR(MAX)=NULL,
	
	@ServerID SMALLINT=NULL OUTPUT,
	@ApiLogPathID INT=NULL OUTPUT ,
	@ApiLogTemplateID SMALLINT=NULL OUTPUT, 
	@UserAgentID INT=NULL OUTPUT
) AS
	SET NOCOUNT ON
  SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	
	IF ISNULL(@UserAgentID,0)=0
		EXEC @UserAgentID=spUserAgentGet @UserAgent

	IF ISNULL(@ApiLogTemplateID,0)=0
		EXEC @ApiLogTemplateID=spApiLogTemplateGet @RequestTemplate

	IF ISNULL(@ApiLogPathID,0)=0
		EXEC @ApiLogPathID=spApiLogPathGet @PathAndQuery

	IF ISNULL(@ServerID,0)=0
		EXEC @ServerID=spServerGet @ServerName

	INSERT INTO ApiLog(
			SiteID,LogDate,VisitID,UserID,ClientIP,UserIP,
			RequestMethod,RequestSize,RequestBody,
			ResponseSize,ResponseJson,HttpStatusCode,DurationMsecs,
			ErrorType,ErrorMessage,
			ApiLogPathID,ApiLogTemplateID,UserAgentID,ServerID)
		VALUES (
			@SiteID,GETDATE(),@VisitID,@UserID,@ClientIP,@UserIP,
			SUBSTRING(@RequestMethod,1,1),LEN(@RequestBody),@RequestBody,
			DATALENGTH(@ResponseJson),@ResponseJson,@HttpStatusCode,@DurationMsecs,
			@ErrorType,@ErrorMessage,
			@ApiLogPathID,@ApiLogTemplateID,@UserAgentID,@ServerID)

GO
