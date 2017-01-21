CREATE TABLE [dbo].[Alias]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[FirstNameID] [int] NULL,
[MiddleNameID] [int] NULL,
[LastNameID] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Alias] ADD CONSTRAINT [PK_Alias] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Alias] ON [dbo].[Alias] ([FirstNameID], [MiddleNameID], [LastNameID]) INCLUDE ([ID]) ON [PRIMARY]
GO
