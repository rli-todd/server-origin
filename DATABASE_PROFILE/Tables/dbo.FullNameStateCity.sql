CREATE TABLE [dbo].[FullNameStateCity]
(
[FirstNameID] [int] NULL,
[MiddleNameID] [int] NULL,
[LastNameID] [int] NULL,
[StateFips] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Hits] [int] NULL
) ON [PRIMARY]
GO
