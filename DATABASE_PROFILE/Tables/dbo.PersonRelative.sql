CREATE TABLE [dbo].[PersonRelative]
(
[PersonID] [int] NOT NULL,
[RelatedPersonID] [int] NOT NULL,
[Relationship] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PersonRelative] ADD CONSTRAINT [PK_PersonRelative] PRIMARY KEY CLUSTERED  ([PersonID], [RelatedPersonID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_PersonRelative_RelativePerson] ON [dbo].[PersonRelative] ([RelatedPersonID]) INCLUDE ([PersonID]) ON [PRIMARY]
GO
