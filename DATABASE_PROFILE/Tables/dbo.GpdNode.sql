CREATE TABLE [dbo].[GpdNode]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[StateFips] [tinyint] NOT NULL,
[CityFips] [int] NOT NULL,
[LastName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FirstName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_GeoPersonDirectory] ON [dbo].[GpdNode] ([StateFips], [CityFips], [LastName], [FirstName]) ON [PRIMARY]
GO
