CREATE TABLE [dbo].[PersonSchool]
(
[PersonID] [int] NOT NULL,
[SchoolID] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PersonSchool] ADD CONSTRAINT [PK_PersonSchool] PRIMARY KEY CLUSTERED  ([PersonID], [SchoolID]) ON [PRIMARY]
GO
