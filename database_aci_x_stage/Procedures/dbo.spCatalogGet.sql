SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spCatalogGet]( @SiteID TINYINT, @UserID INT=NULL, @CategoryID INT=NULL, @IsActive BIT=1) AS
  SET NOCOUNT ON

  /*
  ** We want to return a product in each category as follows:
  **
  ** -- Only one product per category
  ** -- If the user has an active subscription, use the subscription product for a given category
  ** -- If the user has an active subscription, don't show any top-level subscription products
  ** -- Assumption is that user may only have a single active subscription
  */

  DECLARE
    @SubscriptionProductID INT,
    @SubscriptionCategoryID INT

  SELECT @SubscriptionCategoryID=p.CategoryID,@SubscriptionProductID=SubscriptionProductID
    FROM SubscriptionUser su
    JOIN Product p
      ON p.SiteID=su.SiteID
      AND p.ID=su.SubscriptionProductID
    WHERE su.SiteID=@SiteID
    AND su.UserID=@UserID
    AND su.IsActive=1

  DECLARE @Category TABLE(CategoryID INT)

  INSERT INTO @Category(CategoryID)
    SELECT ID
      FROM Category
      WHERE SiteID=@SiteID
      AND IsActive=1
      AND (@SubscriptionCategoryID IS NULL OR CategoryType<>'Subscription')
      AND ID=ISNULL(@CategoryID,ID)

  SELECT *
    FROM Category c
    WHERE c.ID IN (SELECT CategoryID FROM @Category)

  /*
  ** Now that we've identified the categories that we'll show, let's get the products
  */

  DECLARE @Product TABLE(CategoryID INT, ProductID INT)

  /*
  ** First get any subscription products
  */
  IF @SubscriptionProductID IS NOT NULL
    INSERT INTO @Product(CategoryID,ProductID)
      SELECT p.CategoryID,ChildProductID
        FROM SubscriptionProduct sp
        JOIN Product p
          ON p.ID=sp.ChildProductID
        JOIN @Category c
          ON c.CategoryID=p.CategoryID
        WHERE sp.SiteID=@SiteID
        AND p.SiteID=@SiteID
        AND sp.ParentProductID=@SubscriptionProductID
      
  /*
  ** Now get the "default" products for categories that aren't already represented
  */
  INSERT INTO @Product(CategoryID,ProductID)
    SELECT p.CategoryID,p.ID
      FROM Product p
      WHERE SiteID=@SiteID
      AND p.IsDefault=1
      AND NOT EXISTS (
        SELECT 1
          FROM @Product p2
          WHERE p2.CategoryID=p.CategoryID
      )

  SELECT c.*
    FROM Category c
    JOIN @Category c2
      ON c2.CategoryID=c.ID

  SELECT p.*
    FROM Product p
    JOIN @Product p2
      ON p2.ProductID=p.ID
GO
