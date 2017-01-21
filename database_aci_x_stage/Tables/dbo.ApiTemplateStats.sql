SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApiTemplateStats] (
		[ApiTemplateID]      [smallint] NULL,
		[LogPeriod]          [smalldatetime] NULL,
		[ClientIP]           [int] NULL,
		[Method]             [varchar](10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
		[HttpStatusCode]     [smallint] NULL,
		[Hits]               [int] NULL,
		[TotalMsecs]         [bigint] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ApiTemplateStats] SET (LOCK_ESCALATION = TABLE)
GO
