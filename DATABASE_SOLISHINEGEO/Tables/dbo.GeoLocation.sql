CREATE TABLE [dbo].[GeoLocation]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[GeoCityID] [int] NULL,
[Latitude] [real] NULL,
[Longitude] [real] NULL,
[AreaCode] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DmaCode] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PostalCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[TimeZone] [smallint] NULL,
[DaylightSavings] [bit] NULL,
[StateFips] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CountyFips] [varchar] (3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeoLocation] ADD CONSTRAINT [PK_GeoLocation] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_GeoLocation_PostalCode] ON [dbo].[GeoLocation] ([PostalCode], [GeoCityID]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GeoLocation] ADD CONSTRAINT [FK_GeoLocation_GeoCity] FOREIGN KEY ([GeoCityID]) REFERENCES [dbo].[GeoCity] ([ID])
GO
