CREATE TABLE [dbo].[CityLookup]
(
[StateFips] [tinyint] NOT NULL,
[CityFips] [int] NOT NULL,
[LastName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FirstName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LastNameID] [int] NOT NULL,
[FirstNameID] [int] NOT NULL,
[PersonCount] [int] NOT NULL
) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX [IX_CityLookup] ON [dbo].[CityLookup] ([StateFips], [CityFips], [LastName], [FirstName]) ON [PRIMARY]
GO
