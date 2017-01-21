CREATE TABLE [dbo].[xGeoPostal]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[PostalCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[GeoCityID] [int] NULL,
[Latitude] [real] NULL,
[Longitude] [real] NULL,
[TimeZone] [smallint] NULL,
[DaylightSavingsTime] [bit] NULL,
[AreaCode] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DmaCode] [char] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
