CREATE TABLE [dbo].[GeoCityDistance]
(
[FromStateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FromCityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToStateFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ToCityFips] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Miles] [float] NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_GeoCityDistance] ON [dbo].[GeoCityDistance] ([FromStateFips], [FromCityFips], [ToStateFips], [ToCityFips]) ON [PRIMARY]
GO
