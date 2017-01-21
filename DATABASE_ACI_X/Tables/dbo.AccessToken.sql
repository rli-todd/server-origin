SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AccessToken] (
		[AccessToken]            [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[AccessTokenExpiry]      [datetime] NOT NULL,
		[RefreshToken]           [varchar](128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[RefreshTokenExpiry]     [datetime] NOT NULL,
		[DateCreated]            [datetime] NOT NULL,
		[UserID]                 [int] NOT NULL
) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [CIX_AccessToken]
	ON [dbo].[AccessToken] ([AccessToken])
	ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AccessToken_RefreshToken]
	ON [dbo].[AccessToken] ([RefreshToken], [RefreshTokenExpiry])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccessToken] SET (LOCK_ESCALATION = TABLE)
GO
