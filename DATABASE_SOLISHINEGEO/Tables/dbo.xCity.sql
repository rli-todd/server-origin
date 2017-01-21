CREATE TABLE [dbo].[xCity]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[StateID] [tinyint] NULL,
[CityName] [varchar] (40) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
