CREATE TABLE [dbo].[SearchResults]
(
[ID] [int] NOT NULL,
[SearchType] [smallint] NULL,
[FirstNameID] [int] NULL,
[LastNameID] [int] NULL,
[MiddleInitial] [varchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[State] [varchar] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DirectoryType] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DirectoryID] [int] NULL,
[VisitID] [int] NULL,
[NumResults] [smallint] NULL,
[QueryDurationMsecs] [int] NULL,
[DateCreated] [datetime] NULL,
[ApiSource] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ResultsAreEmpty] [bit] NULL,
[DateConsumed] [date] NULL,
[ListProfileIDs] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[DateCached] [datetime] NULL,
[FileSize] [int] NULL
) ON [SEARCH_RESULTS] TEXTIMAGE_ON [SEARCH_RESULTS]
GO
ALTER TABLE [dbo].[SearchResults] ADD CONSTRAINT [PK_SearchResults] PRIMARY KEY CLUSTERED  ([ID]) WITH (FILLFACTOR=80, PAD_INDEX=ON) ON [SEARCH_RESULTS]
GO

CREATE NONCLUSTERED INDEX [IX_SearchResults] ON [dbo].[SearchResults] ([FirstNameID], [LastNameID], [MiddleInitial], [State], [SearchType]) INCLUDE ([ID]) ON [SEARCH_RESULTS]
GO
