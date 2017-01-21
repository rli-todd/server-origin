SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityPrisonComplexesTopNInit](@RowsPerCity INT=5) AS
	SET NOCOUNT ON;
		DROP TABLE GeoStateCityPrisonComplexes
		SELECT 
				x.StateFips,
				x.CityFips,
				ISNULL(CONVERT(VARCHAR(50),[Prison Complex]),'')'PrisonComplex',
				ISNULL(CONVERT(VARCHAR(50),[Prison Facility]),'')'PrisonFacility',
				ISNULL(CONVERT(VARCHAR(255),[FBP URL]),'')'URL',
				ISNULL(CONVERT(VARCHAR(2000),Blurb),'')'Blurb',
				ISNULL(CONVERT(VARCHAR(255),[address 1]),'')'Address1',
				ISNULL(CONVERT(VARCHAR(255),[address 2]),'')'Address2',
				ISNULL(CONVERT(VARCHAR(50),city),'')'City',
				ISNULL(CONVERT(VARCHAR(2),state),'')'State',
				ISNULL(CONVERT(VARCHAR(10),[zip code]),'')'Zip',
				ISNULL(CONVERT(VARCHAR(20),phone),'')'Phone',
				ISNULL(CONVERT(VARCHAR(50),Operator),'')'Operator',
				ISNULL(CONVERT(VARCHAR(255),[Operator Url]),'')'OperatorUrl',
				ISNULL(CONVERT(VARCHAR(255),[Operator Facility Url]),'')'OperatorFacilityUrl',
				ISNULL(CONVERT(INT,Population),0)'Population',
				ISNULL(CONVERT(VARCHAR(50),[Population Type]),'')'PopulationType',
				ISNULL(CONVERT(VARCHAR(50),[Population Gender]),'')'PopulationGender',
				ISNULL(CONVERT(VARCHAR(100),[Inmate Mail Name]),'')'InmateMailName',
				ISNULL(CONVERT(VARCHAR(100),[Inmate Mail Address 1]),'')'InmateMailAddress1',
				ISNULL(CONVERT(VARCHAR(100),[Inmate Mail Address 2]),'')'InmateMailAddress2',
				ISNULL(CONVERT(VARCHAR(100),[Inmate Mail Address 3]),'')'InmateMailAddress3',
				ISNULL(CONVERT(VARCHAR(100),[Inmate Mail Address 4]),'')'InmateMailAddress4',
				ISNULL(CONVERT(VARCHAR(100),[Inmate Mail City]),'')'InmateMailCity',
				ISNULL(CONVERT(VARCHAR(2),[Inmate Mail State]),'')'InmateMailState',
				ISNULL(CONVERT(VARCHAR(10),[Inmate Mail Zip Code]),'')'InmateMailZip',
				dbo.fnNiceTimeRange([Visiting Hours Sun: Start],[Visiting Hours Sun: End])'VisitingHoursSun',
				dbo.fnNiceTimeRange([Visiting Hours Mon: Start],[Visiting Hours Mon: End])'VisitingHoursMon',
				dbo.fnNiceTimeRange([Visiting Hours Tue: Start],[Visiting Hours Tue: End])'VisitingHoursTue',
				dbo.fnNiceTimeRange([Visiting Hours Wed: Start],[Visiting Hours Wed: End])'VisitingHoursWed',
				dbo.fnNiceTimeRange([Visiting Hours Thu: Start],[Visiting Hours Thu: End])'VisitingHoursThu',
				dbo.fnNiceTimeRange([Visiting Hours Fri: Start],[Visiting Hours Fri: End])'VisitingHoursFri',
				dbo.fnNiceTimeRange([Visiting Hours Sat: Start],[Visiting Hours Sat: End])'VisitingHoursSat',
				dbo.fnNiceTimeRange([Visiting Hours Hol: Start],[Visiting Hours Hol: End])'VisitingHoursHol'

			INTO GeoStateCityPrisonComplexes
			FROM _import_PrisonComplexes i
			JOIN xvFips x
				ON x.StateAbbr=i.State
				AND x.CityName=i.City

	DROP TABLE GeoStateCityPrisonComplexesTopN;

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
			JOIN GeoStateCityPrisonComplexes cr
				ON d.ToStateFips=cr.StateFips
				AND d.ToCityFips=cr.CityFips
			JOIN xvFips xT
				ON xT.StateFips=ToStateFips
				AND xT.CityFips=ToCityFips
	)
		SELECT FromStateFips,FromCityFips,ToStateFips,ToCityFips,
						StateName,StateAbbr,CityName,Miles,RowNum,
						PrisonComplex,PrisonFacility,URL,Blurb,Address1,Address2,City,State,Zip,Phone,
						Operator,OperatorUrl,OperatorFacilityUrl,Population,PopulationType,PopulationGender,
						InmateMailName,InmateMailAddress1,InmateMailAddress2,InmateMailAddress3,InmateMailAddress4,
						InmateMailCity,InmateMailState,InmateMailZip,
						VisitingHoursSun,VisitingHoursMon,VisitingHoursTue,VisitingHoursWed,VisitingHoursThu,VisitingHoursFri,VisitingHoursSat,VisitingHoursHol
			INTO GeoStateCityPrisonComplexesTopN
		FROM cte WHERE RowNum<=@RowsPerCity

	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityPrisonComplexesTopN ON GeoStateCityPrisonComplexesTopN(FromStateFips,FromCityFips,ToStateFips,ToCityFips,RowNum)
GO
