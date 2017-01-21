SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [dbo].[spVisitSetPage]( @VisitID INT, @PageCode VARCHAR(30)) AS
	SET NOCOUNT ON
	DECLARE 
		@SiteID TINYINT,
		@ConversionPathCode VARCHAR(255),
		@COnversionPathID INT;

	SELECT 
			@SiteID=v.SiteID,
			@ConversionPathCode=CASE WHEN ISNULL(ConversionPathCode,'')='' THEN @PageCode ELSE ConversionPathCode+','+@PageCode END
		FROM Visit v
		LEFT JOIN ConversionPath cp
			ON cp.SiteID=v.SiteID
			AND cp.ID=v.ConversionPathID
		WHERE v.ID=@VisitID;

	EXEC @ConversionPathID=spConversionPathGet @SiteID=@SiteID, @COnversionPathCode=@ConversionPathCode
	UPDATE Visit SET ConversionPathID=@ConversionPathID, DateModified=GETDATE()
		WHERE SiteID=@SiteID
		AND ID=@VisitID

GO
