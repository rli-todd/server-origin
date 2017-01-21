SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROC spVariationCreate(
  @AuthorizedUserID INT,
  @SectionID INT, 
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
  DECLARE @VariationID INT

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
      FROM Section
      WHERE ID=@SectionID
  )
  BEGIN
    RAISERROR('NotFound: Section not found',11,1);
    RETURN;
  END

  INSERT INTO Variation(SectionID,Description,MultirowPrefix,MultirowSuffix,MultirowDelimiter,
                          HeaderTemplate,BodyTemplate,ViewName,ViewFieldNames,IsEnabled) VALUES
                        (@SectionID,@Description,@MultirowPrefix,@MultirowSuffix,@MultirowDelimiter,
                          @HeaderTemplate,@BodyTemplate,@ViewName,@ViewFIeldNames,@IsEnabled)
  SET @VariationID=SCOPE_IDENTITY()
  RETURN @VariationID;
GO
