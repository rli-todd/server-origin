CREATE TABLE [dbo].[FullName]
(
[FirstNameID] [int] NULL,
[LastNameID] [int] NULL,
[MiddleInitial] [varchar] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Hits] [int] NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_FullName] ON [dbo].[FullName] ([FirstNameID], [LastNameID], [MiddleInitial]) ON [PRIMARY]
GO
