CREATE TABLE [dbo].[Profile]
(
[ProfileID] [char] (11) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ProfileAttributes] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[CompressedJson] [varbinary] (max) NOT NULL,
[DateCached] [datetime] NOT NULL,
[DurationMsecs] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
