SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC spVariationUpdate(
  @AuthorizedUserID INT,
  @VariationID INT,
  @Description VARCHAR(MAX)=NULL,
  @MultirowPrefix VARCHAR(MAX)=NULL,
  @MultirowSuffix VARCHAR(MAX)=NULL,
  @MultirowDelimiter VARCHAR(MAX)=NULL,
  @HeaderTemplate VARCHAR(MAX)=NULL,
  @BodyTemplate VARCHAR(MAX)=NULL,
  @ViewName VARCHAR(255)=NULL,
  @ViewFieldNames VARCHAR(MAX)=NULL,
  @IsEnabled BIT=0)
AS 
  SET NOCOUNT ON

  IF NOT EXISTS (
    SELECT 1
      FROM Users
      WHERE ID=@AuthorizedUserID
      AND IsBackofficeWriter=1
  )
  BEGIN
    RAISERROR('Unauthorized',11,1);
    RETURN;
  END

  IF NOT EXISTS (
    SELECT 1
      FROM Variation
      WHERE ID=@VariationID
  )
  BEGIN
    RAISERROR('NotFound: Variation not found',11,1);
    RETURN;
  END

  UPDATE Variation SET
    Description=ISNULL(@Description,Description),
    MultirowPrefix=ISNULL(@MultirowPrefix,MultirowPrefix),
    MultirowSuffix=ISNULL(@MultirowSuffix,MultirowSuffix),
    MultirowDelimiter=ISNULL(@MultirowDelimiter,MultirowDelimiter),
    HeaderTemplate=ISNULL(@HeaderTemplate,HeaderTemplate),
    BodyTemplate=ISNULL(@BodyTemplate,BodyTemplate),
    ViewName=ISNULL(@ViewName,ViewName),
    ViewFieldNames=ISNULL(@ViewFieldNames,ViewFieldNames),
    IsEnabled=ISNULL(@IsEnabled,ISNULL(IsEnabled,0))
    WHERE ID=@VariationID
GO
