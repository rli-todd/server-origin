SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Cart] (
		[SiteID]           [tinyint] NULL,
		[ID]               [int] IDENTITY(1, 1) NOT NULL,
		[VisitID]          [int] NULL,
		[OrderID]          [int] NULL,
		[ProductID]        [int] NULL,
		[QueryID]          [int] NULL,
		[ProfileID]        [char](11) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[State]            [char](2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[DateCreated]      [datetime] NULL,
		[DateModified]     [datetime] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Cart] SET (LOCK_ESCALATION = TABLE)
GO
