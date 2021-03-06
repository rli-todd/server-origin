CREATE TABLE [dbo].[GeoStateCityPrisonComplexesTopN]
(
[FromStateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FromCityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToStateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToCityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Miles] [float] NULL,
[RowNum] [bigint] NULL,
[PrisonComplex] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PrisonFacility] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[URL] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Blurb] [varchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Address1] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Address2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[City] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[State] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Zip] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Phone] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Operator] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OperatorUrl] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[OperatorFacilityUrl] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Population] [int] NOT NULL,
[PopulationType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PopulationGender] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InmateMailName] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InmateMailAddress1] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InmateMailAddress2] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InmateMailAddress3] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InmateMailAddress4] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InmateMailCity] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InmateMailState] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[InmateMailZip] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[VisitingHoursSun] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VisitingHoursMon] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VisitingHoursTue] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VisitingHoursWed] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VisitingHoursThu] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VisitingHoursFri] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VisitingHoursSat] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[VisitingHoursHol] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_GeoStateCityPrisonComplexesTopN] ON [dbo].[GeoStateCityPrisonComplexesTopN] ([FromStateFips], [FromCityFips], [ToStateFips], [ToCityFips], [RowNum]) ON [PRIMARY]
GO
