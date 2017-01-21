SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[vGeoStateCityOffensesByState] AS
  SELECT StateFips,
    SUM(POpulation)'Population',
    SUM(ViolentCrime)'ViolentCrime',
    SUM(MurderAndNonNegligentManslaughter)'MurderAndNonNegligentManslaughter',
    SUM(ForcibleRape)'ForcibleRape',
    SUM(Robbery)'Robbery',
    SUM(AggravatedAssault)'AggravatedAssault',
    SUM(PropertyCrime)'PropertyCrime',
    SUM(Burglary)'Burglary',
    SUM(LarcenyTheft)'LarcenyTheft',
    SUM(MotorVehicleTheft)'MotorVehicleTheft',
    SUM(Arson)'Arson'
    FROM GeoStateCityOffenses2012
    GROUP BY StateFips
GO
