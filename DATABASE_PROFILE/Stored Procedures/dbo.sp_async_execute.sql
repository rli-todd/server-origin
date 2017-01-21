SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
/*
sp_async_execute - asynchronous execution of T-SQL command or stored prodecure
2012 Antonin Foller, Motobit Software, www.motobit.com
*/
CREATE PROCEDURE [dbo].[sp_async_execute](
  @sql varchar(4000), 
  @jobname varchar(200) = null, 
  @database varchar(200)= null, 
  @owner varchar(200) = null ) AS 
BEGIN
	SET NOCOUNT ON;

	declare @id uniqueidentifier
	--Create unique job name if the name is not specified
	if @jobname is null set @jobname= 'async'
	set @jobname = @jobname + '_' + convert(varchar(64),NEWID())

	if @owner is null set @owner = 'sa'

	--Create a new job, get job ID
	execute msdb..sp_add_job @jobname, @owner_login_name=@owner, @job_id=@id OUTPUT

	--Specify a job server for the job
	execute msdb..sp_add_jobserver @job_id=@id

	--Specify a first step of the job - the SQL command
	--(@on_success_action = 3 ... Go to next step)
	execute msdb..sp_add_jobstep @job_id=@id, @step_name='Step1', @command = @sql, 
		@database_name = @database, @on_success_action = 3 

	--Specify next step of the job - delete the job
	declare @deletecommand varchar(200)
	set @deletecommand = 'execute msdb..sp_delete_job @job_name='''+@jobname+''''
	execute msdb..sp_add_jobstep @job_id=@id, @step_name='Step2', @command = @deletecommand

	--Start the job
	execute msdb..sp_start_job @job_id=@id

END
GO
