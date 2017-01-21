SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sku] (
		[SiteID]                [tinyint] NOT NULL,
		[ID]                    [int] IDENTITY(1, 1) NOT NULL,
		[CategoryID]            [int] NULL,
		[ProductExternalID]     [int] NULL,
		[ProductType]           [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[IsActive]              [bit] NOT NULL,
		[Price]                 [money] NULL,
		[RecurringPrice]        [money] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sku]
	ADD
	CONSTRAINT [PK_Sku]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Sku]
	ADD
	CONSTRAINT [DF_Sku_IsEnabled]
	DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Sku]
	WITH CHECK
	ADD CONSTRAINT [FK_Sku_Category]
	FOREIGN KEY ([SiteID], [CategoryID]) REFERENCES [dbo].[Category] ([SiteID], [ID])
ALTER TABLE [dbo].[Sku]
	CHECK CONSTRAINT [FK_Sku_Category]

GO
ALTER TABLE [dbo].[Sku] SET (LOCK_ESCALATION = TABLE)
GO
