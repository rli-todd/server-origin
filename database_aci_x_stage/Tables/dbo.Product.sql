SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product] (
		[SiteID]             [tinyint] NOT NULL,
		[ID]                 [int] IDENTITY(1, 1) NOT NULL,
		[CategoryID]         [int] NOT NULL,
		[ProductToken]       [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[Price]              [money] NOT NULL,
		[DiscountAmount]     [money] NOT NULL,
		[RecurringPrice]     [money] NULL,
		[ReportTypeCode]     [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[ProductCode]        [varchar](20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[MSRP]               [money] NOT NULL,
		[IsDefault]          [bit] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [PK_Product]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_Price]
	DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_ReportTypeCode]
	DEFAULT ('PL') FOR [ReportTypeCode]
GO
ALTER TABLE [dbo].[Product]
	WITH CHECK
	ADD CONSTRAINT [FK_Product_Category]
	FOREIGN KEY ([SiteID], [CategoryID]) REFERENCES [dbo].[Category] ([SiteID], [ID])
ALTER TABLE [dbo].[Product]
	CHECK CONSTRAINT [FK_Product_Category]

GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Product_ProductCode]
	ON [dbo].[Product] ([SiteID], [ProductCode])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Product] SET (LOCK_ESCALATION = TABLE)
GO
