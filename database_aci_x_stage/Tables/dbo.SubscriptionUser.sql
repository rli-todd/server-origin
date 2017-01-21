SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubscriptionUser] (
		[SiteID]                    [tinyint] NOT NULL,
		[UserID]                    [int] NOT NULL,
		[SubscriptionProductID]     [int] NOT NULL,
		[OrderID]                   [int] NULL,
		[Status]                    [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[DateCreated]               [datetime] NULL,
		[DateModified]              [datetime] NULL,
		[IsActive]                  [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SubscriptionUser]
	ADD
	CONSTRAINT [PK_SubscriptionUser]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [UserID], [SubscriptionProductID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[SubscriptionUser]
	WITH CHECK
	ADD CONSTRAINT [FK_SubscriptionUser_Orders]
	FOREIGN KEY ([SiteID], [OrderID]) REFERENCES [dbo].[Orders] ([SiteID], [ID])
ALTER TABLE [dbo].[SubscriptionUser]
	CHECK CONSTRAINT [FK_SubscriptionUser_Orders]

GO
ALTER TABLE [dbo].[SubscriptionUser]
	WITH CHECK
	ADD CONSTRAINT [FK_SubscriptionUser_Product]
	FOREIGN KEY ([SiteID], [SubscriptionProductID]) REFERENCES [dbo].[Product] ([SiteID], [ID])
ALTER TABLE [dbo].[SubscriptionUser]
	CHECK CONSTRAINT [FK_SubscriptionUser_Product]

GO
ALTER TABLE [dbo].[SubscriptionUser] SET (LOCK_ESCALATION = TABLE)
GO
