SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[Orders] (
		[SiteID]             [tinyint] NOT NULL,
		[ID]                 [int] NOT NULL,
		[ExternalID]         [int] NULL,
		[OrderDate]          [datetime] NULL,
		[UserExternalID]     [int] NULL,
		[UserID]             [int] NULL,
		[VisitID]            [int] NULL,
		[OrderTotal]         [money] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders]
	ADD
	CONSTRAINT [PK_Orders]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Order_ExternalID]
	ON [dbo].[Orders] ([SiteID], [ExternalID])
	INCLUDE ([ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Orders] SET (LOCK_ESCALATION = TABLE)
GO
