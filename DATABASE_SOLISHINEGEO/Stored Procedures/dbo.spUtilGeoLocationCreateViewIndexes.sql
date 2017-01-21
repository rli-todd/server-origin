SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROC [dbo].[spUtilGeoLocationCreateViewIndexes] AS
	SET NOCOUNT ON
	IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[vGeoLocation]') 
		AND name = N'PK_vGeoLocation')
	EXEC sp_ExecuteSQL N'CREATE UNIQUE CLUSTERED INDEX PK_vGeoLocation ON vGeoLocation(GeoLocationID)'	

	IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[vGeoLocation]') 
		AND name = N'IX_vGeoLocation_CityRegion')
	EXEC sp_ExecuteSQL N'CREATE NONCLUSTERED INDEX IX_vGeoLocation_CityRegion ON vGeoLocation(CityName,RegionCode)'	

	IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[vGeoLocation]') 
		AND name = N'IX_vGeoLocation_PostalCode')
	EXEC sp_ExecuteSQL N'CREATE NONCLUSTERED INDEX IX_vGeoLocation_PostalCode ON vGeoLocation(PostalCode)'	


	IF  NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[vGeoLocationSelector]') 
		AND name = N'IX_vGeoLocationSelector')
	EXEC sp_ExecuteSQL N'CREATE UNIQUE CLUSTERED INDEX IX_vGeoLocationSelectorUS ON vGeoLocationSelectorUS(Location)'	
	
GO
