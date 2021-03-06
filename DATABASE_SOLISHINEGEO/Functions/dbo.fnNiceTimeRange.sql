SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[fnNiceTimeRange](@D1 DATETIME, @D2 DATETIME) RETURNS VARCHAR(20) AS
BEGIN
	RETURN CASE
		WHEN @D1 IS NULL AND @D2 IS NULL 
			THEN ''
		WHEN @D1 IS NULL AND @D2 IS NOT NULL
			THEN 'Until ' + dbo.fnNiceTime(@D2)
		WHEN @D1 IS NOT NULL AND @D2 IS NULL
			THEN dbo.fnNiceTIme(@D1)
		ELSE
			dbo.fnNiceTime(@D1) + ' to ' + dbo.fnNiceTime(@D2)
	END
END
GO
