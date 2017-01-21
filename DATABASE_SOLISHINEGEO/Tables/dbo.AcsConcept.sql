CREATE TABLE [dbo].[AcsConcept]
(
[AcsConceptID] [int] NOT NULL,
[ConceptCode] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ConceptName] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AcsConcept] ADD CONSTRAINT [PK_AcsConcept] PRIMARY KEY CLUSTERED  ([AcsConceptID]) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AcsConcept] ON [dbo].[AcsConcept] ([ConceptCode]) INCLUDE ([AcsConceptID], [ConceptName]) ON [PRIMARY]
GO
