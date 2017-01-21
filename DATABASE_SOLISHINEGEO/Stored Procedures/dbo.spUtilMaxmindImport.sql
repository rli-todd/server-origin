SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spUtilMaxmindImport]( 
	@FolderPath VARCHAR(255)='C:\Temp\', 
	@CountryFilename VARCHAR(255)='GeoIP2-Country-Blocks-IPV4.csv',
	@LocationFilename VARCHAR(255)='GeoIP2-Country-Locations-en.csv') AS
	SET NOCOUNT ON
	CREATE TABLE #MMCountry(
		network VARCHAR(255),
		geoname_id VARCHAR(255),
		registered_country_geoname_id VARCHAR(255),
		represented_country_geoname_id VARCHAR(255),
		is_anonymous_proxy VARCHAR(255),
		is_satellite_provider VARCHAR(255))

	CREATE TABLE #MMLocation(
		geoname_id VARCHAR(255),
		locale_code VARCHAR(255),
		continent VARCHAR(255),
		continent_name VARCHAR(255),
		country_iso_code VARCHAR(255),
		country_name VARCHAR(255))

	DECLARE 
		@WithSQL NVARCHAR(MAX)='
		WITH
		(
		CHECK_CONSTRAINTS,
		FIRSTROW=2,
		FIELDTERMINATOR = '','',
		ROWTERMINATOR = ''' + CHAR(0x0A) + '''
		)'

	EXEC ('BULK INSERT #MMCountry FROM ''' + @FolderPath + @CountryFilename + '''' + @WithSQL);
	EXEC ('BULK INSERT #MMLocation FROM ''' + @FolderPath + @LocationFilename + '''' + @WithSQL);

	TRUNCATE TABLE MMLocation
	INSERT INTO MMLocation(GeoNameID,LocaleCode,Continent,ContinentName,CountryIsoCode,CountryName)
		SELECT
				geoname_id,locale_code,continent,
				REPLACE(continent_name,'"',''),
				country_iso_code,
				REPLACE(country_name,'"','')
			FROM #MMLocation

	TRUNCATE TABLE MMCountryBlock;
	WITH cte AS
	(
		SELECT 
				network,geoname_id,registered_country_geoname_id,
				CASE WHEN represented_country_geoname_id='' THEN NULL ELSE CONVERT(BIGINT,represented_country_geoname_id) END'represented_country_geoname_id',
				is_anonymous_proxy,is_satellite_provider
			FROM #MMCountry
	)
	INSERT INTO MMCountryBlock(	
			Network,IpAddrStart,IpAddrEnd,
			GeoNameID,CountryIsoCode,
			RegisteredCountryGeoNameID,RegisteredCountryIsoCode,
			RepresentedCountryGeoNameID, RepresentedCountryIsoCode,
			IsAnonymousProxy,IsSatelliteProvider)
		SELECT 
				network,dbo.fnCidrToLowInt(network),dbo.fnCidrToHighInt(network),
				geoname_id,l.CountryIsoCode,
				registered_country_geoname_id,r.CountryIsoCode,
				represented_country_geoname_id,p.CountryIsoCode,
				CONVERT(BIT,is_anonymous_proxy),CONVERT(BIT,is_satellite_provider)
			FROM #MMCountry c
			LEFT JOIN MMLocation l
				ON l.GeoNameID=c.geoname_id
			LEFT JOIN MMLocation r
				ON r.GeoNameID=c.registered_country_geoname_id
			LEFT JOIN MMLocation p
				ON p.GeoNameID=c.represented_country_geoname_id

	DROP TABLE #MMCountry


GO
