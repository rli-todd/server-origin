SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spScheduledHourly] AS
	SET NOCOUNT ON
	DECLARE @LogRetentionDays INT=7
	DELETE NLog
		WHERE LogDate<DATEADD(day,-@LogRetentionDays,GETDATE())
	DELETE ApiLog
		WHERE LogDate<DATEADD(day,-@LogRetentionDays,GETDATE())

  DELETE ApiLogPath
    FROM ApiLogPath lp
    WHERE NOT EXISTS (
      SELECT 1
        FROM ApiLog 
        WHERE ApiLogPathID=lp.ID
    )
	
	EXEC spUtilPermissionsGrant
GO
