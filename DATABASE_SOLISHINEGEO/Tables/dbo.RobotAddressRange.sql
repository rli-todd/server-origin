CREATE TABLE [dbo].[RobotAddressRange]
(
[ID] [int] NOT NULL IDENTITY(1, 1),
[RobotID] [int] NOT NULL,
[IPAddressStart] [int] NOT NULL,
[IPAddressEnd] [int] NOT NULL,
[Note] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RobotAddressRange] ADD CONSTRAINT [PK_RobotIpAddressRange] PRIMARY KEY NONCLUSTERED  ([ID]) ON [PRIMARY]
GO
CREATE UNIQUE CLUSTERED INDEX [IX_RobotAddressRange_Range] ON [dbo].[RobotAddressRange] ([IPAddressStart], [IPAddressEnd]) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RobotAddressRange] ADD CONSTRAINT [FK_RobotAddressRange_Robot] FOREIGN KEY ([RobotID]) REFERENCES [dbo].[Robot] ([ID])
GO
