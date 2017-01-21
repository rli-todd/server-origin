SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spApiLogViewStatus](@Mins INT=60) AS
	SET NOCOUNT ON
	EXEC spApiLogView @Mins=@Mins, @ByTemplate=1, @ShowUnauthenticatedUsers=1
	EXEC spApiLogView @Mins=@Mins, @Summary=1, @ShowUnauthenticatedUsers=1
	EXEC spApiLogView @Mins=@Mins, @HttpStatusCode=500, @ShowUnauthenticatedUsers=1
	SELECT TOP 20 Logger,Level,Message,COUNT(*)
		FROM NLog
		WHERE LogDate>=DATEADD(minute,-@Mins,GETDATE())
		AND Logger NOT LIKE '%daemon%'
		GROUP BY Logger,Level,Message
		ORDER BY COUNT(*) DESC
	SELECT TOP 100 *
		FROM NLog
		WHERE LogDate>=DATEADD(minute,-@Mins,GETDATE())
		AND Level='Error'
		AND Logger NOT LIKE '%daemon%'
		ORDER BY LogDate DESC

GO
