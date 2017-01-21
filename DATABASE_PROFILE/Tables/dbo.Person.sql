CREATE TABLE [dbo].[Person]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[ProfileID] [char] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FirstNameID] [int] NULL,
[LastNameID] [int] NULL,
[MiddleNameID] [int] NULL,
[DateOfBirth] [date] NULL,
[PhoneCount] [tinyint] NULL,
[EmailCount] [tinyint] NULL,
[DateModified] [date] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Person] ADD CONSTRAINT [PK_Person] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Person_ProfileID] ON [dbo].[Person] ([ProfileID]) ON [PRIMARY]
GO
