SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO




CREATE VIEW [dbo].[vGpdBuildJobs] AS
SELECT 
    job.Name, 
    job.job_ID, 
    job.Originating_Server, 
    stop_execution_date'StopDate',
    activity.run_requested_Date, 
    DATEDIFF(second, activity.run_requested_Date, GETDATE()) as Elapsed
  FROM msdb.dbo.sysjobs_view job
  LEFT JOIN msdb.dbo.sysjobactivity activity
    ON (job.job_id = activity.job_id)
  WHERE job.name LIKE 'gpd_build_%'
  --AND run_Requested_date IS NOT NULL
  --AND stop_execution_date IS NULL

GO
