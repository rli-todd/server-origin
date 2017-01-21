CREATE TABLE [dbo].[PersonCompany]
(
[PersonID] [int] NOT NULL,
[CompanyID] [int] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[PersonCompany] ADD CONSTRAINT [PK_PersonCompany] PRIMARY KEY CLUSTERED  ([PersonID], [CompanyID]) ON [PRIMARY]
GO
