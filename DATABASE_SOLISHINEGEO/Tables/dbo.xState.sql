CREATE TABLE [dbo].[xState]
(
[ID] [tinyint] NOT NULL IDENTITY(1, 1),
[StateAbbr] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[StateName] [varchar] (25) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
