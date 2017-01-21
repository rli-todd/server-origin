CREATE TABLE [dbo].[usa_libraries]
(
[state_abbr] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[library_name] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[library_address] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[library_city] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[library_zip] [float] NULL,
[library_county] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[library_phone] [float] NULL
) ON [PRIMARY]
GO
