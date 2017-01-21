SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
CREATE PROC [dbo].[spProductInitAll] WITH RECOMPILE AS
	SET NOCOUNT ON

	DECLARE 
		@CR_SiteID TINYINT=1,
    @PR_SiteID TINYINT=2

	DECLARE 
    @Category TABLE(
      SiteID TINYINT,
      CategoryName VARCHAR(50),
      CategoryCode VARCHAR(20),
      ExternalProductID INT,
      RequireQueryID BIT,
      RequireState BIT,
      RequireProfileID BIT,
      CategoryType VARCHAR(20),
      IsActive BIT)

  INSERT INTO @Category(
    SiteID,CategoryType,CategoryName,CategoryCode,IsActive,RequireQueryID,RequireState,RequireProfileID,ExternalProductID)
    VALUES
    (@CR_SiteID,'AlaCarte','People Lookup','PL',1,1,0,1,1247),
    (@CR_SiteID,'AlaCarte','Statewide Criminal Check','SCC',1,1,1,1,1248),
    (@CR_SiteID,'AlaCarte','Nationwide Criminal Check','NCC',1,1,0,1,1249),
    /* CR Subscriptions */
    (@CR_SiteID,'Subscription','People Lookup Subscription','PL_Sub',1,0,0,0,1290),
    (@CR_SiteID,'Subscription','Statewide Criminal Check Subscription','SCC_Sub',1,0,0,0,1291)
    
  INSERT INTO Category(
    SiteID,CategoryType,CategoryName,CategoryCode,IsActive,RequireQueryID,RequireState,RequireProfileID,ProductExternalID)
    SELECT SiteID,CategoryTYpe,CategoryName,CategoryCode,IsActive,RequireQueryID,RequireState,RequireProfileID,ExternalProductID
      FROM @Category ct
      WHERE NOT EXISTS(
        SELECT 1
          FROM Category c
          WHERE c.SiteID=ct.SiteID
          AND c.CategoryCode=ct.CategoryCode
      )

  DECLARE
		@DefaultProducts TABLE(
      SiteID TINYINT,
			CategoryCode VARCHAR(10),
      ProductCode VARCHAR(20),
      MSRP MONEY,
      DiscountAmount MONEY,
      Price MONEY,
      RecurringPrice MONEY,
      ReportTypeCode VARCHAR(3),
      IsDefault BIT); -- IsDefault indicates that this product will show for logged out users

	INSERT INTO @DefaultProducts(
			SiteID,CategoryCode,ProductCode,ReportTypeCode,MSRP,DiscountAmount,Price,RecurringPrice,IsDefault) VALUES
		(@CR_SiteID,'PL','PL','PL',2,0,2,NULL,1),
		(@CR_SiteID,'SCC','SCC','SCC',20,0,20,NULL,1),
		(@CR_SiteID,'NCC','NCC','NCC',40,0,40,NULL,1),
    (@CR_SiteID,'PL_Sub','PL_Sub','PL',9.99,0,9.99,9.99,1),
    (@CR_SiteID,'PL','PL_Free_10','PL',2,2,0,NULL,0),
    (@CR_SiteID,'SCC','SCC_Disc_30','SCC',20,6,14,NULL,0),
    (@CR_SiteID,'NCC','NCC_Disc_30','NCC',40,12,28,NULL,0),
    (@CR_SiteID,'SCC_Sub','SCC_Sub','SCC',19.99,0,19.99,19.99,1),
    (@CR_SiteID,'SCC','SCC_Free_3','SCC',20,20,0,NULL,0),
    (@CR_SiteID,'SCC','SCC_Disc_50','SCC',20,10,10,NULL,0),
    (@CR_SiteID,'NCC','NCC_Disc_50','NCC',40,20,20,NULL,0)

  INSERT INTO Product(SiteID,CategoryID,ProductCode,ReportTypeCode,MSRP,DiscountAmount,Price,RecurringPrice,IsDefault)
    SELECT df.SiteID,c.ID,df.ProductCode,df.ReportTypeCode,df.MSRP,df.DiscountAmount,df.Price,df.RecurringPrice,df.IsDefault
      FROM @DefaultProducts df
      JOIN Category c
        ON c.SiteID=df.SiteID
        AND c.CategoryCode=df.CategoryCode
      WHERE NOT EXISTS (
        SELECT 1
          FROM Product p
          WHERE p.SiteID=df.SiteID
          AND p.ProductCode=df.ProductCode
      )

  DECLARE 
    @SubProduct TABLE(
      SiteID TINYINT,
      ParentProductCode VARCHAR(20),
      ChildProductCode VARCHAR(20),
      ItemType VARCHAR(20),
      Quantity INT,
      DiscountCode VARCHAR(20)
    )

  INSERT INTO @SubProduct(SiteID,ParentProductCode,ChildProductCode,ItemType,Quantity,DiscountCode) VALUES
    (@CR_SiteID,'PL_Sub','PL_Free_10','Free',10,NULL),
    (@CR_SiteID,'PL_Sub','SCC_Disc_30','Discounted',NULL,'LCCZYI'),
    (@CR_SiteID,'PL_Sub','NCC_Disc_30','Discounted',NULL,'LCCZYI'),
    (@CR_SiteID,'SCC_Sub','SCC_Free_3','Free',3,NULL),
    (@CR_SiteID,'SCC_Sub','SCC_Disc_50','Discounted',NULL,'LCCZYI'),
    (@CR_SiteID,'SCC_Sub','NCC_Disc_50','Discounted',NULL,'LCCZYI')

  INSERT INTO SubscriptionProduct(SiteID,ParentProductID,ChildProductID,PeriodQuantity,ItemType,DiscountCode)
    SELECT sp.SiteID,pParent.ID,pChild.ID,sp.Quantity,sp.ItemType,sp.DiscountCode
      FROM @SubProduct sp
      JOIN Product pParent
        ON pParent.SiteID=sp.SiteID
        AND pParent.ProductCode=sp.ParentProductCode
      JOIN Product pChild 
        ON pChild.SiteID=sp.SiteID
        AND pChild.ProductCode=sp.ChildProductCode
      WHERE NOT EXISTS (
        SELECT 1
          FROM SubscriptionProduct sp2
          JOIN Product pParent2
            ON pParent2.Siteid=sp2.SiteID
          JOIN Product pChild2
            ON pChild2.SiteID=sp2.SiteID
          WHERE pParent2.SiteID=sp.SiteID
          AND pParent2.ProductCode=sp.ParentProductCode
          AND pChild2.SiteID=sp.SiteID
          AND pChild2.ProductCode=sp.ChildProductCode
    )
GO
