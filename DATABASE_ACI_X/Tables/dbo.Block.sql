SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Block] (
		[ID]              [int] IDENTITY(1, 1) NOT NULL,
		[BlockName]       [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[BlockType]       [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[IsEnabled]       [bit] NOT NULL,
		[KeyTemplate]     [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Block]
	ADD
	CONSTRAINT [PK_Block]
	PRIMARY KEY
	CLUSTERED
	([ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Block] SET (LOCK_ESCALATION = TABLE)
GO
