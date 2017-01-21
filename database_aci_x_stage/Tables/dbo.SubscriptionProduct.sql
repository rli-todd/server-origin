SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SubscriptionProduct] (
		[SiteID]              [tinyint] NOT NULL,
		[ID]                  [int] IDENTITY(1, 1) NOT NULL,
		[DiscountCode]        [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ItemType]            [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[ParentProductID]     [int] NOT NULL,
		[ChildProductID]      [int] NULL,
		[PeriodQuantity]      [int] NULL,
		[Description]         [varchar](1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[SubscriptionProduct]
	ADD
	CONSTRAINT [PK_SubscriptionProduct]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[SubscriptionProduct]
	ADD
	CONSTRAINT [DF_SubscriptionItem_PeriodQuantity]
	DEFAULT ((0)) FOR [PeriodQuantity]
GO
ALTER TABLE [dbo].[SubscriptionProduct]
	WITH CHECK
	ADD CONSTRAINT [FK_SubscriptionProduct_Child]
	FOREIGN KEY ([SiteID], [ChildProductID]) REFERENCES [dbo].[Product] ([SiteID], [ID])
ALTER TABLE [dbo].[SubscriptionProduct]
	CHECK CONSTRAINT [FK_SubscriptionProduct_Child]

GO
ALTER TABLE [dbo].[SubscriptionProduct]
	WITH CHECK
	ADD CONSTRAINT [FK_SubscriptionProduct_Parent]
	FOREIGN KEY ([SiteID], [ParentProductID]) REFERENCES [dbo].[Product] ([SiteID], [ID])
ALTER TABLE [dbo].[SubscriptionProduct]
	CHECK CONSTRAINT [FK_SubscriptionProduct_Parent]

GO
EXEC sp_addextendedproperty N'MS_Description', N'Discounted, Free', 'SCHEMA', N'dbo', 'TABLE', N'SubscriptionProduct', 'COLUMN', N'ItemType'
GO
EXEC sp_addextendedproperty N'MS_Description', N'', 'SCHEMA', N'dbo', 'TABLE', N'SubscriptionProduct', 'COLUMN', N'ParentProductID'
GO
ALTER TABLE [dbo].[SubscriptionProduct] SET (LOCK_ESCALATION = TABLE)
GO
