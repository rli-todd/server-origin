SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROC [dbo].[spSynonymCreateAll]( @ServerName VARCHAR(50)) AS
	SET NOCOUNT ON
	EXEC spSynonymCreate @ServerName=@ServerName,	@DatabaseName='SolishineGeo', @Prefix='sg',	@ObjectName='spVisitInfoGet'
	EXEC spSynonymCreate @ServerName=@ServerName,	@DatabaseName='SolishineGeo', @Prefix='sg',	@ObjectName='spVisitInfoGet2'
	EXEC spSynonymCreate @ServerName=@ServerName,	@DatabaseName='SolishineGeo', @Prefix='sg',	@ObjectName='GeoLocation'
	EXEC spSynonymCreate @ServerName=@ServerName,	@DatabaseName='SolishineGeo', @Prefix='sg',	@ObjectName='GeoCity'
	EXEC spSynonymCreate @ServerName=@ServerName,	@DatabaseName='SolishineGeo', @Prefix='sg',	@ObjectName='GeoRegion'

  EXEC spSynonymCreate @ServerName=@ServerName, @DatabaseName='ACI_X', @Prefix='ax', @ObjectName='Visit'
GO
