SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnDateHour](@D DATETIME) RETURNS DATETIME AS
BEGIN
	RETURN DATEADD(hour,DATEPART(hour,@D),CONVERT(DATETIME,CONVERT(DATE,@D)))
END
GO
