SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vGeoStateCityJail] AS
  SELECT StateName,CityName,j.*
    FROM GeoStateCityJail j
    JOIN GeoCity gc
      ON gc.StateFips=j.StateFips
      AND gc.CityFips=j.CityFips
    JOIN GeoState gs
      ON gs.StateFips=gc.StateFips
GO
