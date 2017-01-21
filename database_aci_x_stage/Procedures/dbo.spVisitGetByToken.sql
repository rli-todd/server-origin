SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spVisitGetByToken]( @VisitGuid UNIQUEIDENTIFIER)
AS
  DECLARE @VisitID INT, @SiteID TINYINT
  SELECT @VisitID=ID,@SiteID=SiteID
    FROM Visit
    WHERE VisitGuid=@VisitGuid

  EXEC spVisitGet @SiteID=@SiteID,@VisitID=@VisitID

GO
