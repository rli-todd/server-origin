CREATE TABLE [dbo].[School]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[SchoolName] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_School] ON [dbo].[School] ([SchoolName]) INCLUDE ([ID]) ON [PRIMARY]
GO
