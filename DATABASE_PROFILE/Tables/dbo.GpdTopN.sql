CREATE TABLE [dbo].[GpdTopN]
(
[GpdNodeID] [int] NULL,
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PersonCount] [int] NULL
) ON [PRIMARY]
GO
CREATE CLUSTERED INDEX [IX_GeoPersonDirectoryTop100] ON [dbo].[GpdTopN] ([GpdNodeID], [PersonCount] DESC) ON [PRIMARY]
GO
