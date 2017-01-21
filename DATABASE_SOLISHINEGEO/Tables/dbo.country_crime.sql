CREATE TABLE [dbo].[country_crime]
(
[year] [float] NULL,
[population] [float] NULL,
[crime_violent] [float] NULL,
[crime_violent_100k] [float] NULL,
[crime_murder] [float] NULL,
[crime_murder_100k] [float] NULL,
[crime_rape] [float] NULL,
[crime_rape_100k] [float] NULL,
[crime_robbery] [float] NULL,
[crime_robbery_100k] [float] NULL,
[crime_assault] [float] NULL,
[crime_assault_100k] [float] NULL,
[crime_property] [float] NULL,
[crime_property_100k] [float] NULL,
[crime_burglary] [float] NULL,
[crime_burglary_100k] [float] NULL,
[crime_larceny] [float] NULL,
[crime_larceny_100k] [float] NULL,
[crime_vehicle] [float] NULL,
[crime_vehicle_100k] [float] NULL,
[F21] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F22] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
