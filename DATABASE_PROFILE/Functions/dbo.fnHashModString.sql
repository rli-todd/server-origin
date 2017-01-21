SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[fnHashModString](@Divisor INT, @String VARCHAR(MAX)) RETURNS INT AS
BEGIN
  RETURN ABS(CONVERT(INT,HASHBYTES('SHA1',@String))) % @Divisor
END
GO
