CREATE TABLE [dbo].[LastName]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[LastName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[IsPartial] [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[LastName] ADD CONSTRAINT [PK_LastName] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_LastName_Name] ON [dbo].[LastName] ([LastName]) INCLUDE ([ID], [IsPartial]) ON [PRIMARY]
GO
