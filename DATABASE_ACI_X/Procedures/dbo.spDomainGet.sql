SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spDomainGet]( @DomainName VARCHAR(50)) AS
  SET NOCOUNT ON
  DECLARE @DomainID INT
  SELECT @DomainID=ID
    FROM Domain
    WHERE DomainName=@DomainName

  IF @DomainID IS NULL BEGIN
    INSERT INTO Domain(DomainName)VALUES(@DomainName)
    SET @DomainID=SCOPE_IDENTITY()
  END
  RETURN @DomainID

GO
