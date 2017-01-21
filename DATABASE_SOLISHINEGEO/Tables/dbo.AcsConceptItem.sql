CREATE TABLE [dbo].[AcsConceptItem]
(
[AcsConceptItemID] [int] NOT NULL,
[AcsConceptID] [int] NOT NULL,
[ItemCode] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsMarginOfError] [bit] NULL,
[SubDescription] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[SubDescription2] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
