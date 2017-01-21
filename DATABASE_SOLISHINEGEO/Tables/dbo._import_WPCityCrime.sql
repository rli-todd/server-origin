CREATE TABLE [dbo].[_import_WPCityCrime]
(
[id] [float] NULL,
[agency] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[state] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[state_name] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[city] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[fips] [float] NULL,
[population] [float] NULL,
[violent_crime_rate] [float] NULL,
[murder] [float] NULL,
[rape] [float] NULL,
[robbery] [float] NULL,
[assault] [float] NULL,
[property] [float] NULL,
[burglary] [float] NULL,
[larceny] [float] NULL,
[vehicular] [float] NULL,
[year] [float] NULL
) ON [PRIMARY]
GO
