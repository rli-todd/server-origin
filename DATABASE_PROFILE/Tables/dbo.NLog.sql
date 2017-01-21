CREATE TABLE [dbo].[NLog]
(
[LogDate] [datetime] NOT NULL,
[ID] [int] NOT NULL IDENTITY(1, 1),
[Level] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Logger] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Message] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ClientIP] [varchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ServerName] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[NLog] ADD CONSTRAINT [PK_NLog] PRIMARY KEY CLUSTERED  ([LogDate], [ID]) WITH (FILLFACTOR=80, PAD_INDEX=ON) ON [PRIMARY]
GO
