SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityFederalCircuitCourtsTopNInit](@RowsPerCity INT=2) AS
	SET NOCOUNT ON;
		DROP TABLE GeoStateCityFederalCircuitCourts
		SELECT 
				x.StateFips,
				x.CityFips,
				ISNULL(CONVERT(VARCHAR(50),[CourtName]),'')'CourtName',
				ISNULL(CONVERT(VARCHAR(50),[CourtLevel]),'')'CourtLevel',
				ISNULL(CONVERT(VARCHAR(2),[Circuit]),'')'Circuit',
				CASE WHEN ISNULL(Main_Office,'')='1' THEN 'Yes' ELSE 'No' END'MainOffice',
				ISNULL(CONVERT(VARCHAR(255),[LocationAddress1]),'')'LocationAddress1',
				ISNULL(CONVERT(VARCHAR(255),[LocationAddress2]),'')'LocationAddress2',
				ISNULL(CONVERT(VARCHAR(50),LocationCity),'')'LocationCity',
				ISNULL(CONVERT(VARCHAR(2),LocationState),'')'LocationState',
				ISNULL(CONVERT(VARCHAR(10),LocationZip),'')'LocationZip',
				ISNULL(CONVERT(VARCHAR(255),[MailingAddress1]),'')'MailingAddress1',
				ISNULL(CONVERT(VARCHAR(255),[MailingAddress2]),'')'MailingAddress2',
				ISNULL(CONVERT(VARCHAR(50),MailingCity),'')'MailingCity',
				ISNULL(CONVERT(VARCHAR(2),MailingState),'')'MailingState',
				ISNULL(CONVERT(VARCHAR(10),MailingZip),'')'MailingZip',
				ISNULL(CONVERT(VARCHAR(20),phoneMail),'')'Phone',
				ISNULL(CONVERT(VARCHAR(255),Url),'')'Url',
				ISNULL(CONVERT(VARCHAR(255),[ECL Link]),'')'ECLLink'

			INTO GeoStateCityFederalCircuitCourts
			FROM _import_FederalCircuitCourts i
			JOIN xvFips x
				ON x.StateAbbr=i.LocationState
				AND x.CityName=i.LocationCity

	DROP TABLE GeoStateCityFederalCircuitCourtsTopN;

	WITH cte AS
	(
		SELECT d.FromStateFips,d.FromCityFips,d.ToStateFips,d.ToCityFips,
				xt.StateName,xt.StateAbbr,xt.CityName,Miles,
				ROW_NUMBER() OVER(PARTITION BY d.FromStateFips,d.FromCityFips ORDER BY Miles )'RowNum',
				cr.*
			FROM xvFips xf
			JOIN GeoCityDistance d
				ON d.FromStateFips=xF.StateFips
				AND d.FromCityFips=xF.CityFips
			JOIN GeoStateCityFederalCircuitCourts cr
				ON d.ToStateFips=cr.StateFips
				AND d.ToCityFips=cr.CityFips
			JOIN xvFips xT
				ON xT.StateFips=ToStateFips
				AND xT.CityFips=ToCityFips
	)
		SELECT FromStateFips'StateFips',FromCityFips'CityFips',ToStateFips,ToCityFips,
						StateName,StateAbbr,CityName,Miles,RowNum,
						CourtName,COurtLevel,Circuit,MainOffice,
						LocationAddress1,LocationAddress2,LocationCity,LocationState,LocationZip,
						MailingAddress1,MailingAddress2,MailingCity,MailingState,MailingZip,
						Phone,Url,ECLLink
			INTO GeoStateCityFederalCircuitCourtsTopN
		FROM cte WHERE RowNum<=@RowsPerCity
		ORDER BY FromStateFips,FromCityFips,RowNum

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityFederalCircuitCourtsTopN ON GeoStateCityFederalCircuitCourtsTopN(StateFips,CityFips,ToStateFips,ToCityFips,RowNum)
GO
