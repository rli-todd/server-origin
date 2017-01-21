CREATE TABLE [dbo].[GeoStateCityOffenses2012]
(
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[Population] [float] NULL,
[ViolentCrime] [float] NULL,
[MurderAndNonNegligentManslaughter] [float] NULL,
[ForcibleRape] [float] NULL,
[Robbery] [float] NULL,
[AggravatedAssault] [float] NULL,
[PropertyCrime] [float] NULL,
[Burglary] [float] NULL,
[LarcenyTheft] [float] NULL,
[MotorVehicleTheft] [float] NULL,
[Arson] [float] NULL
) ON [PRIMARY]
GO
