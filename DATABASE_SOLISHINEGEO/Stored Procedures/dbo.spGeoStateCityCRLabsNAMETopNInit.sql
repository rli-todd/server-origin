SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityCRLabsNAMETopNInit](@RowsPerCity INT=5) AS
	SET NOCOUNT ON;
		DROP TABLE GeoStateCityCRLabsNAME
		SELECT DISTINCT
				x.StateFips,
				x.CityFips,
				x.StateName,
				x.StateAbbr,
				CONVERT(VARCHAR(30),'OrganizationType')'OrganizationType',
				CONVERT(VARCHAR(255),name)'Name',
				CONVERT(VARCHAR(255),address1)'Address1',
				CONVERT(VARCHAR(255),[address 2])'Address2',
				CONVERT(VARCHAR(50),city)'City',
				CONVERT(VARCHAR(2),state)'State',
				CONVERT(VARCHAR(10),zip)'Zip',
				CONVERT(VARCHAR(20),phone)'Phone',
				CONVERT(VARCHAR(20),fax)'Fax',
				CONVERT(VARCHAR(100),[Primary contact])'PrimaryContact',
				CONVERT(VARCHAR(100),Email)'Email'
			INTO GeoStateCityCRLabsNAME
			FROM _import_CRLabs_NAME i
			JOIN xvFips x
				ON x.StateAbbr=i.State
				AND x.CityName=i.City

	DROP TABLE GeoStateCityCRLabsNAMETopN;

	WITH cte AS
	(
		SELECT d.FromStateFips,d.FromCityFips,d.ToStateFips,d.ToCityFips,
				xt.StateName,xt.StateAbbr,xt.CityName,Miles,
				ROW_NUMBER() OVER(PARTITION BY d.FromStateFips,d.FromCityFips ORDER BY Miles )'RowNum',
				cr.OrganizationType,cr.Name,cr.Address1,cr.Address2,cr.City,cr.State,cr.Zip,cr.Phone,cr.Fax,cr.PrimaryContact,cr.Email
			FROM xvFips xf
			JOIN GeoCityDistance d
				ON d.FromStateFips=xF.StateFips
				AND d.FromCityFips=xF.CityFips
			JOIN GeoStateCityCRLabsNAME cr
				ON d.ToStateFips=cr.StateFips
				AND d.ToCityFips=cr.CityFips
			JOIN xvFips xT
				ON xT.StateFips=ToStateFips
				AND xT.CityFips=ToCityFips
	)
		SELECT FromStateFips'StateFips',FromCityFips'CityFips',ToStateFips,ToCityFips,
						StateName,StateAbbr,CityName,Miles,RowNum,
						OrganizationType,Name,Address1,Address2,City,State,Zip,Phone,Fax,PrimaryContact,Email
			INTO GeoStateCityCRLabsNAMETopN
		FROM cte WHERE RowNum<=@RowsPerCity
		ORDER BY FromStateFips,FromCityFips,RowNum

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityCRLabsNAMETopN ON GeoStateCityCRLabsNAMETopN(StateFips,CityFips,ToStateFips,ToCityFips,RowNum)
GO
