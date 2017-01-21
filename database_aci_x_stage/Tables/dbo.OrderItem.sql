SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderItem] (
		[SiteID]                  [tinyint] NOT NULL,
		[ID]                      [int] IDENTITY(1, 1) NOT NULL,
		[OrderID]                 [int] NOT NULL,
		[ExternalID]              [int] NULL,
		[ProductExternalID]       [int] NULL,
		[ProductID]               [int] NULL,
		[ReferenceID]             [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Quantity]                [smallint] NULL,
		[Price]                   [money] NULL,
		[DiscountAmount]          [money] NULL,
		[DiscountDescription]     [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[QueryID]                 [int] NULL,
		[ProfileID]               [char](11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[State]                   [char](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderItem]
	ADD
	CONSTRAINT [PK_OrderItem_1]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[OrderItem]
	WITH CHECK
	ADD CONSTRAINT [FK_OrderItem_Orders]
	FOREIGN KEY ([SiteID], [OrderID]) REFERENCES [dbo].[Orders] ([SiteID], [ID])
ALTER TABLE [dbo].[OrderItem]
	CHECK CONSTRAINT [FK_OrderItem_Orders]

GO
ALTER TABLE [dbo].[OrderItem]
	WITH CHECK
	ADD CONSTRAINT [FK_OrderItem_Product]
	FOREIGN KEY ([SiteID], [ProductID]) REFERENCES [dbo].[Product] ([SiteID], [ID])
ALTER TABLE [dbo].[OrderItem]
	CHECK CONSTRAINT [FK_OrderItem_Product]

GO
ALTER TABLE [dbo].[OrderItem] SET (LOCK_ESCALATION = TABLE)
GO
