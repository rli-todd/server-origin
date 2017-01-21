SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE VIEW [dbo].[vRobot] AS
SELECT TOP 1000 r.ID'RobotID',RobotName,Block,UserAgentLike,
	rar.ID'RobotAddressRangeID',
	CASE WHEN IpAddressStart  IS NULL THEN NULL ELSE dbo.fnIpAddressToString(IPAddressStart)END'IpAddressStart',
	CASE WHEN IpAddressEnd IS NULL THEN NULL ELSE dbo.fnIpAddressToString(IpAddressEnd)END'IpAddressEnd',
	ISNULL(Note,'')'Note'
FROM Robot r
LEFT JOIN RobotAddressRange rar ON rar.RobotID=r.ID
ORDER BY RobotName,IpAddressStart
GO
