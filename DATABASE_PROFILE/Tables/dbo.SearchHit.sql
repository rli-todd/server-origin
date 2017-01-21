CREATE TABLE [dbo].[SearchHit]
(
[SearchResultsID] [int] NOT NULL,
[HitDate] [date] NULL,
[NumHits] [smallint] NULL
) ON [SEARCH_HIT]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_SearchHit] ON [dbo].[SearchHit] ([HitDate], [SearchResultsID]) ON [SEARCH_HIT]
GO
