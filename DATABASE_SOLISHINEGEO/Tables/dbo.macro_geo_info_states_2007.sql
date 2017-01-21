CREATE TABLE [dbo].[macro_geo_info_states_2007]
(
[state] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[state_abbr] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[census_region] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[census_geo_division] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[population] [float] NULL,
[crime_violent] [float] NULL,
[crime_violent_100k] [float] NULL,
[crime_murder2] [float] NULL,
[crime_murder2_100k] [float] NULL,
[crime_rape] [float] NULL,
[crime_rape_100k] [float] NULL,
[crime_robbery2] [float] NULL,
[crime_robbery2_100k] [float] NULL,
[crime_assault] [float] NULL,
[crime_assault_100k] [float] NULL,
[crime_property] [float] NULL,
[crime_property_100k] [float] NULL,
[crime_burglary] [float] NULL,
[crime_burglary_100k] [float] NULL,
[crime_larceny] [float] NULL,
[crime_larceny_100k] [float] NULL,
[crime_vehicle] [float] NULL,
[crime_vehicle_100k] [float] NULL
) ON [PRIMARY]
GO
