SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[vGeoLocationUSState] AS
SELECT * FROM vGeoLocation WHERE CityName='Any' AND CountryCode='US' AND RegionCode IS NOT NULL
GO
