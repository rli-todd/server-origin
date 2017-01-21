CREATE TABLE [dbo].[CityLookupSummary]
(
[StateFips] [tinyint] NULL,
[CityFips] [int] NULL,
[PersonCount] [int] NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [PK_CityLookupSummary] ON [dbo].[CityLookupSummary] ([StateFips], [CityFips]) ON [PRIMARY]
GO
