SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

--CREATE TABLE ConversionPath(SiteID TINYINT, ID INT IDENTITY(1,1), ConversionPathCode VARCHAR(50))
--CREATE UNIQUE NONCLUSTERED INDEX IX_ConversionPath ON ConversionPath(SiteID,ConversionPathCode)

CREATE PROC spCOnversionPathGet(@SiteID TINYINT, @ConversionPathCode VARCHAR(255)) AS
	SET NOCOUNT ON
	DECLARE @ConversionPathID INT
	SELECT @ConversionPathID=ID
		FROM ConversionPath
		WHERE SiteiD=@SiteID
		AND ISNULL(ConversionPathCode,'')=ISNULL(@ConversionPathCode,'')

	IF @ConversionPathID IS NULL BEGIN
		INSERT INTO ConversionPath(SiteID,ConversionPathCode)VALUES(@SiteID,@ConversionPathCode)
		SET @ConversionPathID=SCOPE_IDENTITY()
	END
	RETURN @ConversionPathID
GO
