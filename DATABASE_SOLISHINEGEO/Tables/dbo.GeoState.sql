CREATE TABLE [dbo].[GeoState]
(
[StateFips] [tinyint] NULL,
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LandArea] [real] NULL,
[WaterArea] [real] NULL,
[TotalArea] [real] NULL,
[LandAreaRank] [int] NULL,
[WaterAreaRank] [int] NULL,
[TotalAreaRank] [int] NULL
) ON [PRIMARY]
GO
