CREATE TABLE [dbo].[macro_geo_info_metro_2008]
(
[county_type] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[county_type_abbr] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[population_range] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[population] [float] NULL,
[crime_violent] [float] NULL,
[crime_murder2] [float] NULL,
[crime_rape] [float] NULL,
[crime_robbery2] [float] NULL,
[crime_assault] [float] NULL,
[crime_property] [float] NULL,
[crime_burglary] [float] NULL,
[crime_larceny] [float] NULL,
[crime_vehicle] [float] NULL,
[crime_arson] [float] NULL,
[agency_count] [float] NULL
) ON [PRIMARY]
GO
