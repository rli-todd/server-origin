SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Report] (
		[SiteID]                  [tinyint] NOT NULL,
		[ID]                      [int] NOT NULL,
		[UserID]                  [int] NOT NULL,
		[OrderItemID]             [int] NOT NULL,
		[ReportTypeCode]          [varchar](3) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[QueryID]                 [int] NOT NULL,
		[ProfileID]               [varchar](11) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[JsonLen]                 [int] NOT NULL,
		[HtmlLen]                 [int] NOT NULL,
		[CompressedJson]          [varbinary](max) NOT NULL,
		[CompressedHtml]          [varbinary](max) NOT NULL,
		[ReportDate]              [datetime] NOT NULL,
		[ReportCreationMsecs]     [int] NOT NULL,
		[State]                   [char](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Report]
	ADD
	CONSTRAINT [PK_Report]
	PRIMARY KEY
	CLUSTERED
	([SiteID], [ID])
	ON [PRIMARY]
GO
ALTER TABLE [dbo].[Report]
	WITH CHECK
	ADD CONSTRAINT [FK_Report_OrderItem]
	FOREIGN KEY ([SiteID], [OrderItemID]) REFERENCES [dbo].[OrderItem] ([SiteID], [ID])
ALTER TABLE [dbo].[Report]
	CHECK CONSTRAINT [FK_Report_OrderItem]

GO
ALTER TABLE [dbo].[Report]
	WITH CHECK
	ADD CONSTRAINT [FK_Report_ReportType]
	FOREIGN KEY ([SiteID], [ReportTypeCode]) REFERENCES [dbo].[ReportType] ([SiteID], [TypeCode])
ALTER TABLE [dbo].[Report]
	CHECK CONSTRAINT [FK_Report_ReportType]

GO
ALTER TABLE [dbo].[Report] SET (LOCK_ESCALATION = TABLE)
GO
