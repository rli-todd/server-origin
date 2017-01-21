CREATE TABLE [dbo].[GeoCountry]
(
[ID] [tinyint] NOT NULL IDENTITY(1, 1),
[CountryCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountryName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeoCountry] ADD CONSTRAINT [PK_GeoCountry] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
