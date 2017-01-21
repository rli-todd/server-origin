CREATE TABLE [dbo].[_import_PrisonComplexes]
(
[S#No#] [float] NULL,
[Prison Complex] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Prison Facility] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[FBP URL] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Blurb] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address 1] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Address 2] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[City] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[State] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Zip Code] [float] NULL,
[Email] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Phone] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Operator] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Operator URL] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Operator Facility URL] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Population] [float] NULL,
[Population Type] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Population Gender] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Staff Name] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Staff Address 1] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Staff Address 2] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Staff City] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Staff State] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Staff Zip Code] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Inmate Mail Name] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Inmate Mail Address 1] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Inmate Mail Address 2] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Inmate Mail Address 3] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Inmate Mail Address 4] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Inmate Mail City] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Inmate Mail State] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Inmate Mail Zip Code] [float] NULL,
[Visiting Hours Sun: Start] [datetime] NULL,
[Visiting Hours Sun: End] [datetime] NULL,
[Visiting Hours Mon: Start] [datetime] NULL,
[Visiting Hours Mon: End] [datetime] NULL,
[Visiting Hours Tue: Start] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Visiting Hours Tue: End] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Visiting Hours Wed: Start] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Visiting Hours Wed: End] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Visiting Hours Thu: Start] [datetime] NULL,
[Visiting Hours Thu: End] [datetime] NULL,
[Visiting Hours Fri: Start] [datetime] NULL,
[Visiting Hours Fri: End] [datetime] NULL,
[Visiting Hours Sat: Start] [datetime] NULL,
[Visiting Hours Sat: End] [datetime] NULL,
[Visiting Hours Hol: Start] [datetime] NULL,
[Visiting Hours Hol: End] [datetime] NULL,
[F49] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F50] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[F51] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
