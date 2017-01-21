SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Product] (
		[SiteID]                [tinyint] NOT NULL,
		[ID]                    [int] IDENTITY(1, 1) NOT NULL,
		[SkuID]                 [int] NOT NULL,
		[ProductExternalID]     [int] NOT NULL,
		[Price]                 [money] NOT NULL,
		[DiscountAmount]        [money] NULL,
		[RecurringPrice]        [money] NULL,
		[ProductName]           [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[RequireQueryID]        [bit] NOT NULL,
		[RequireState]          [bit] NOT NULL,
		[RequireProfileID]      [bit] NOT NULL,
		[ReportTypeCode]        [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[ProductToken]          [varchar](255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[IsPublic]              [bit] NOT NULL
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
	CONSTRAINT [DF_Product_IsPublic]
	DEFAULT ((1)) FOR [IsPublic]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_Price]
	DEFAULT ((0)) FOR [Price]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_ProductExternalID]
	DEFAULT ((0)) FOR [ProductExternalID]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_ReportTypeCode]
	DEFAULT ('PL') FOR [ReportTypeCode]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_RequireProfileID]
	DEFAULT ((0)) FOR [RequireProfileID]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_RequireQueryID]
	DEFAULT ((0)) FOR [RequireQueryID]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_RequireState]
	DEFAULT ((0)) FOR [RequireState]
GO
ALTER TABLE [dbo].[Product]
	ADD
	CONSTRAINT [DF_Product_SkuID]
	DEFAULT ((1)) FOR [SkuID]
GO
ALTER TABLE [dbo].[Product]
	WITH CHECK
	ADD CONSTRAINT [FK_Product_ReportType]
	FOREIGN KEY ([SiteID], [ReportTypeCode]) REFERENCES [dbo].[ReportType] ([SiteID], [TypeCode])
ALTER TABLE [dbo].[Product]
	CHECK CONSTRAINT [FK_Product_ReportType]

GO
ALTER TABLE [dbo].[Product]
	WITH CHECK
	ADD CONSTRAINT [FK_Product_Sku]
	FOREIGN KEY ([SiteID], [SkuID]) REFERENCES [dbo].[Sku] ([SiteID], [ID])
ALTER TABLE [dbo].[Product]
	CHECK CONSTRAINT [FK_Product_Sku]

GO
ALTER TABLE [dbo].[Product] SET (LOCK_ESCALATION = TABLE)
GO
