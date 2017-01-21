SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE VIEW [dbo].[xvGeoNationalOffenses2012] AS
  SELECT 
    SUM(Population)'Population',
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
GO
