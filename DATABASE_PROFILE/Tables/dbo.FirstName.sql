CREATE TABLE [dbo].[FirstName]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[FirstName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsPartial] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[FirstName] ADD CONSTRAINT [PK_FirstName] PRIMARY KEY CLUSTERED  ([ID]) WITH (FILLFACTOR=70) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_FirstName] ON [dbo].[FirstName] ([FirstName]) INCLUDE ([ID], [IsPartial]) ON [PRIMARY]
GO
