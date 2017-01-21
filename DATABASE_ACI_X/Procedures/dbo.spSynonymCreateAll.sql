SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spSynonymCreateAll]( @ServerName VARCHAR(50)) AS
	SET NOCOUNT ON
	EXEC spSynonymCreate 
    @ServerName=@ServerName,	
    @DatabaseName='SolishineGeo', 
    @Prefix='sg',	
    @ObjectName='spVisitInfoGet2'

	EXEC spSynonymCreate 
    @ServerName=@ServerName,	
    @DatabaseName='SolishineGeo', 
    @Prefix='sg',	
    @ObjectName='vGeoLocation'

	EXEC spSynonymCreate 
    @ServerName=@ServerName,	
    @DatabaseName='SolishineGeo', 
    @Prefix='sg',	
    @ObjectName='Robot'

	EXEC spSynonymCreate
		@ServerName=@ServerName,
		@DatabaseName='Profile',
		@Prefix='p',
		@ObjectName='SearchResults'

	EXEC spSynonymCreate
		@ServerName=@ServerName,
		@DatabaseName='Profile',
		@Prefix='p',
		@ObjectName='FirstName'

	EXEC spSynonymCreate
		@ServerName=@ServerName,
		@DatabaseName='Profile',
		@Prefix='p',
		@ObjectName='LastName'
GO
