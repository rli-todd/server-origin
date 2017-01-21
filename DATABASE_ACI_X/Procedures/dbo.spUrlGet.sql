SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spUrlGet]( 
  @Url VARCHAR(255), 
  @UrlTypeCode CHAR(1), 
  @SiteID INT=NULL OUTPUT,
  @ReferCode VARCHAR(20)=NULL OUTPUT) AS
  SET NOCOUNT ON
  DECLARE 
    @UrlHash BINARY(20)=HASHBYTES('SHA1',ISNULL(@Url,'')),
    @Scheme VARCHAR(10),
    @SubdomainName VARCHAR(255),
    @DomainName VARCHAR(255),
    @Path VARCHAR(255),
    @QueryString VARCHAR(255),
    @UrlID INT

	EXEC spParseUrlClr
    @Url=@Url,
    @Scheme=@Scheme OUT,
    @ServerName=@SubdomainName OUT,
    @DomainName=@DomainName OUT,
    @Path=@Path OUT,
    @QueryString=@QueryString OUT

  IF @SiteID IS NULL
  BEGIN
    SELECT @SiteID=ID
      FROM Site
      WHERE SiteName <> ''
			AND @DomainName LIKE '%' + SiteName + '%'
	  OR AlternateSiteName IS NOT NULL AND @DomainName LIKE '%' + ALternateSiteName + '%'

    IF @SiteID IS NULL
    BEGIN
			INSERT INTO NLog(LogDate,Level,Logger,Message)
				SELECT GETDATE(),'Error','spUrlGet','No site found from URL: '  + ISNULL(@Url,'NULL')
      INSERT INTO Site(SiteName)VALUES(@DomainName)
      SET @SiteID=SCOPE_IDENTITY()
    END
  END

  /*
  ** TODO: Determine which ReferCode to use
  */
  SELECT @ReferCode=ISNULL(SeoReferCode,ISNULL(SemReferCode,'2306'))
    FROM Site
    WHERE ID=@SiteID

  SELECT @UrlID=ID
    FROM Url
    WHERE SiteID=@SiteID
		AND UrlTypeCode=@UrlTypeCode
    AND UrlHash=@UrlHash

  IF @UrlID IS NULL
  BEGIN
    DECLARE 
      @DomainID INT,
      @SubdomainID INT
    EXEC @DomainID=spDomainGet @DomainName
    EXEC @SubdomainID=spDomainGet @SubdomainName

  INSERT INTO Url(SiteID,UrlTypeCode,UrlHash,IsHttps,DomainID,SubdomainID,Path,QueryString)
      VALUES (@SiteID,@UrlTypeCode,@UrlHash,
              CASE WHEN @Scheme='https' THEN 1 ELSE 0 END,
              @DomainID,@SubdomainID,@Path,@QueryString)
    SET @UrlID=SCOPE_IDENTITY()
  END
  RETURN @UrlID



GO
