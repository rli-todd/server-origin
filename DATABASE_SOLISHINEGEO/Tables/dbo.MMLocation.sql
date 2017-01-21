CREATE TABLE [dbo].[MMLocation]
(
[GeoNameID] [bigint] NULL,
[LocaleCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Continent] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ContinentName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountryIsoCode] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountryName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
