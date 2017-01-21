CREATE TABLE [dbo].[PersonAlias]
(
[PersonID] [int] NOT NULL,
[AliasID] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PersonAlias] ADD CONSTRAINT [PK_PersonAlias] PRIMARY KEY CLUSTERED  ([PersonID], [AliasID]) ON [PRIMARY]
GO
