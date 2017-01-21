SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityCRLabsCACTopNInit](@RowsPerCity INT=5) AS
	SET NOCOUNT ON;
		DROP TABLE GeoStateCityCRLabsCAC
		SELECT 
				x.StateFips,
				x.CityFips,
				CONVERT(VARCHAR(30),'Classification')'Classification',
				CONVERT(VARCHAR(255),name)'Name',
				CONVERT(VARCHAR(255),address1)'Address1',
				CONVERT(VARCHAR(255),[address 2])'Address2',
				CONVERT(VARCHAR(50),city)'City',
				CONVERT(VARCHAR(2),state)'State',
				CONVERT(VARCHAR(10),zip)'Zip',
				CONVERT(VARCHAR(20),phone)'Phone',
				CONVERT(VARCHAR(20),fax)'Fax',
				CONVERT(VARCHAR(100),Email)'Email',
				CONVERT(VARCHAR(100),Url)'Url'
			INTO GeoStateCityCRLabsCAC
			FROM _import_CRLabs_NAME i
			JOIN xvFips x
				ON x.StateAbbr=i.State
				AND x.CityName=i.City

	DROP TABLE GeoStateCityCRLabsCACTopN;

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
			JOIN GeoStateCityCRLabsCAC cr
				ON d.ToStateFips=cr.StateFips
				AND d.ToCityFips=cr.CityFips
			JOIN xvFips xT
				ON xT.StateFips=ToStateFips
				AND xT.CityFips=ToCityFips
	)
		SELECT FromStateFips'StateFips',FromCityFips'CityFips',ToStateFips,ToCityFips,
						StateName,StateAbbr,CityName,Miles,RowNum,
						CLassification,Name,Address1,Address2,City,State,Zip,Phone,Fax,Email,Url
			INTO GeoStateCityCRLabsCACTopN
		FROM cte WHERE RowNum<=@RowsPerCity
		ORDER BY FromStateFips,FromCityFips,RowNum

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityCRLabsCACTopN ON GeoStateCityCRLabsCACTopN(StateFips,CityFips,ToStateFips,ToCityFips,RowNum)
GO
