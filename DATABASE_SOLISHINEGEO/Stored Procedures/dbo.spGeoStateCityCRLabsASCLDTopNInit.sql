SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityCRLabsASCLDTopNInit](@RowsPerCity INT=5) AS
	SET NOCOUNT ON;
		DROP TABLE GeoStateCityCRLabsASCLD
		SELECT 
				x.StateFips,
				x.CityFips,
				CONVERT(VARCHAR(255),accreditation)'Accreditation',
				CONVERT(VARCHAR(255),name)'Name',
				CONVERT(VARCHAR(255),address1)'Address1',
				CONVERT(VARCHAR(255),[address 2])'Address2',
				CONVERT(VARCHAR(50),city)'City',
				CONVERT(VARCHAR(2),state)'State',
				CONVERT(VARCHAR(10),zip)'Zip'
			INTO GeoStateCityCRLabsASCLD
			FROM _import_CRLabs_ASCLD i
			JOIN xvFips x
				ON x.StateAbbr=i.State
				AND x.CityName=i.City

	DROP TABLE GeoStateCityCRLabsASCLDTopN;

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
			JOIN GeoStateCityCRLabsASCLD cr
				ON d.ToStateFips=cr.StateFips
				AND d.ToCityFips=cr.CityFips
			JOIN xvFips xT
				ON xT.StateFips=ToStateFips
				AND xT.CityFips=ToCityFips
	)
		SELECT FromStateFips'StateFips',FromCityFips'CityFips',ToStateFips,ToCityFips,
						StateName,StateAbbr,CityName,Miles,RowNum,
						Accreditation,Name,Address1,Address2,City,State,Zip
			INTO GeoStateCityCRLabsASCLDTopN
		FROM cte WHERE RowNum<=@RowsPerCity
		ORDER BY FromStateFips,FromCityFips,RowNum

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityCRLabsASCLDTopN ON GeoStateCityCRLabsASCLDTopN(StateFips,CityFips,ToStateFips,ToCityFips,RowNum)

GO
