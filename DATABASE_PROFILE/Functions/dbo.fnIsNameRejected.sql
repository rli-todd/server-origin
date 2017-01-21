SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
CREATE FUNCTION [dbo].[fnIsNameRejected]( @FirstName VARCHAR(30), @LastName VARCHAR(30)) RETURNS BIT AS
BEGIN
  DECLARE @RetVal BIT=0
  IF LEN(ISNULL(@FirstName,''))<1
  OR LEN(ISNULL(@LastName,''))<2
  OR ( -- first 3 letters all the same
      LEN(@FirstName)>2
      AND SUBSTRING(@FirstName,1,1)=SUBSTRING(@FirstName,2,1)
      AND SUBSTRING(@FirstName,1,1)=SUBSTRING(@FirstName,3,1)
  )
  OR ( -- first 3 letters all the same
      LEN(@LastName)>2
      AND SUBSTRING(@LastName,1,1)=SUBSTRING(@LastName,2,1)
      AND SUBSTRING(@LastName,1,1)=SUBSTRING(@LastName,3,1)
  )
  OR EXISTS (SELECT 1 FROM RejectedName WHERE @FirstName LIKE RejectedName)
  OR EXISTS (SELECT 1 FROM RejectedName WHERE @LastName LIKE RejectedName)
    SET @RetVal=1
  RETURN @RetVal
END
GO
