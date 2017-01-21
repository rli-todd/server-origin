CREATE TABLE [dbo].[xGeoZipTimezone]
(
[ZipCode] [int] NULL,
[City] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[State] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Latitude] [real] NULL,
[Longitude] [real] NULL,
[timezone] [smallint] NULL,
[Dst] [tinyint] NULL
) ON [PRIMARY]
GO
