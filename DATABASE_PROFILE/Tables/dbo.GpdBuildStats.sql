CREATE TABLE [dbo].[GpdBuildStats]
(
[StateFips] [tinyint] NOT NULL,
[CityFips] [int] NOT NULL,
[DateStarted] [datetime] NULL,
[DateCompleted] [datetime] NULL,
[TopNPersonCount] [int] NULL,
[NextLetterPersonCount] [int] NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GpdBuildStats] ADD CONSTRAINT [PK_GpdBuildStats] PRIMARY KEY CLUSTERED  ([StateFips], [CityFips]) ON [PRIMARY]
GO
