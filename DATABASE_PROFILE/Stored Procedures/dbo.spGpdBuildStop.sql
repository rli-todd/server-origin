SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE PROC [dbo].[spGpdBuildStop] AS 
  SET NOCOUNT ON
  DECLARE @SQL NVARCHAR(MAX)=''
  SELECT @SQL=@SQL +'
  EXEC msdb..sp_stop_job @job_name=''' + REPLACE(Name,'''','''''') + ''''
    FROM vGpdBuildJobs
    WHERE StopDate IS NULL

  EXEC (@SQL)
  SET @SQL=''
  SELECT @SQL=@SQL +'
  EXEC msdb..sp_delete_job @job_name=''' + REPLACE(Name,'''','''''') + ''''
    FROM vGpdBuildJobs
  EXEC (@SQL)

GO
