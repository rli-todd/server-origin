CREATE TABLE [dbo].[GpdNextLetter]
(
[GpdNodeID] [int] NULL,
[NextLetter] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[PersonCount] [int] NULL
) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX [IX_GeoPersonDirectoryNextLetter] ON [dbo].[GpdNextLetter] ([GpdNodeID], [NextLetter]) ON [PRIMARY]
GO
