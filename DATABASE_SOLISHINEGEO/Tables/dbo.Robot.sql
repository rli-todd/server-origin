CREATE TABLE [dbo].[Robot]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[RobotName] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[UserAgentLike] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Block] [bit] NULL,
[Note] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[HostnameLike] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
CREATE NONCLUSTERED INDEX [IX_Robot_Name] ON [dbo].[Robot] ([RobotName]) INCLUDE ([ID]) ON [PRIMARY]

GO
ALTER TABLE [dbo].[Robot] ADD CONSTRAINT [PK_Robot] PRIMARY KEY CLUSTERED  ([ID]) ON [PRIMARY]
GO
