CREATE TABLE [dbo].[xZipCode]
(
[ZipCode] [char] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Latitude] [real] NULL,
[Longitude] [real] NULL,
[CityID] [int] NULL,
[TimeZone] [smallint] NOT NULL,
[HasDaylightSavingsTime] [bit] NULL
) ON [PRIMARY]
GO
