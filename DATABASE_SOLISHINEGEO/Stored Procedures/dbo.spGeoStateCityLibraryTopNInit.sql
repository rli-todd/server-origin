SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGeoStateCityLibraryTopNInit](@RowsPerCity INT=10) AS
SET NOCOUNT ON
DROP TABLE GeoStateCityLibraryTopN;
WITH cte AS
(
  SELECT d.FromStateFips,d.FromCityFips,d.ToStateFips,d.ToCityFips,
      xt.StateName,xt.StateAbbr,xt.CityName,
      l.*,Miles,
      ROW_NUMBER() OVER(PARTITION BY d.FromStateFips,d.FromCityFips ORDER BY Miles )'RowNum'
    FROM xvFips xf
    JOIN GeoCityDistance d
      ON d.FromStateFips=xF.StateFips
      AND d.FromCityFips=xF.CityFips
    JOIN GeoStateCityLibrary l
      ON d.ToStateFips=l.StateFips
      AND d.ToCityFips=l.CityFips
    JOIN xvFips xT
      ON xT.StateFips=ToStateFips
      AND xT.CityFips=ToCityFips
)
    SELECT FromStateFips'StateFips',FromCityFips'CityFips',ToStateFips,ToCityFips,
          StateName,StateAbbr,CityName,
          LibraryName,Address,Zip,Phone,Miles,RowNum
			INTO GeoStateCityLibraryTopN
		  FROM cte WHERE RowNum<=@RowsPerCity
			ORDER BY FromStateFips,FromCityFips,RowNum


	CREATE UNIQUE CLUSTERED INDEX IX_GeoStateCityLibraryTopN ON GeoStateCityLibraryTopN(StateFips,CityFips,ToStateFips,ToCityFips,RowNum)
GO
