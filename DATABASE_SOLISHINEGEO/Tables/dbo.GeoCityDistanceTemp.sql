CREATE TABLE [dbo].[GeoCityDistanceTemp]
(
[FromStateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FromCityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToStateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToCityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Miles] [float] NULL
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_GeoCityDistanceTemp] ON [dbo].[GeoCityDistanceTemp] ([FromStateFips], [FromCityFips], [ToStateFips], [ToCityFips], [Miles]) ON [PRIMARY]
GO
