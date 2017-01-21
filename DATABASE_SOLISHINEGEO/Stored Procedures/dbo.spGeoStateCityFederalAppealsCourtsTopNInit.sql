SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityFederalAppealsCourtsTopNInit](@RowsPerCity INT=2) AS
	SET NOCOUNT ON;
		DROP TABLE GeoStateCityFederalAppealsCourts
		SELECT 
				x.StateFips,
				x.CityFips,
				ISNULL(CONVERT(VARCHAR(50),[Name]),'')'CourtName',
				ISNULL(CONVERT(VARCHAR(50),[Court_Level]),'')'CourtLevel',
				ISNULL(CONVERT(VARCHAR(2),[Circuit]),'')'Circuit',
				CASE WHEN ISNULL(Main_Office,'')='1' THEN 'Yes' ELSE 'No' END'MainOffice',
				ISNULL(CONVERT(VARCHAR(255),[Address 1]),'')'LocationAddress1',
				ISNULL(CONVERT(VARCHAR(255),[Address 2]),'')'LocationAddress2',
				ISNULL(CONVERT(VARCHAR(50),City),'')'LocationCity',
				ISNULL(CONVERT(VARCHAR(2),State),'')'LocationState',
				ISNULL(CONVERT(VARCHAR(10),[Zip Code]),'')'LocationZip',
				ISNULL(CONVERT(VARCHAR(255),[Address 11]),'')'MailingAddress1',
				ISNULL(CONVERT(VARCHAR(255),[Address 21]),'')'MailingAddress2',
				ISNULL(CONVERT(VARCHAR(50),[Address 3]),'')'MailingAddress3',
				ISNULL(CONVERT(VARCHAR(50),[City1]),'')'MailingCity',
				ISNULL(CONVERT(VARCHAR(2),[State1]),'')'MailingState',
				ISNULL(CONVERT(VARCHAR(10),[Zip Code1]),'')'MailingZip',
				ISNULL(CONVERT(VARCHAR(20),[Main Phone]),'')'Phone',
				ISNULL(CONVERT(VARCHAR(255),Url),'')'Url'

			INTO GeoStateCityFederalAppealsCourts
			FROM _import_FederalAppealsCourts i
			JOIN xvFips x
				ON x.StateAbbr=i.State
				AND x.CityName=i.City

	DROP TABLE GeoStateCityFederalAppealsCourtsTopN;

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
			JOIN GeoStateCityFederalAppealsCourts cr
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
						MailingAddress1,MailingAddress2,MailingAddress3,MailingCity,MailingState,MailingZip,
						Phone,Url
			INTO GeoStateCityFederalAppealsCourtsTopN
		FROM cte WHERE RowNum<=@RowsPerCity
		ORDER BY FromStateFips,FromCityFips,RowNum

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityFederalAppealsCourtsTopN ON GeoStateCityFederalAppealsCourtsTopN(StateFips,CityFips,ToStateFips,ToCityFips,RowNum)
GO
