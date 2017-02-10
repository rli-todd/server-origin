using System;
using System.Collections.Generic;
using System.Linq;
using Aci.X.Business.Cache;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.IwsLib;
using Aci.X.ServerLib;
using Aci.X.IwsLib.Storefront;

namespace Aci.X.Business
{
  public class Sku
  {
    public static ClientLib.Sku Render(DBSku dbSku)
    {
      return new ClientLib.Sku
      {
        SkuID = dbSku.SkuID,
        ProductID = dbSku.ProductID,
        ProductName = dbSku.ProductName,
        DiscountAmount = dbSku.DiscountAmount,
        Price = dbSku.Price,
        RecurringPrice = dbSku.RecurringPrice,
        MSRP = dbSku.MSRP,
        ReportTypeCode = dbSku.ReportTypeCode,
        ProductCode = dbSku.ProductCode,
        SkuCode = dbSku.SkuCode,
        RequireQueryID = dbSku.RequireQueryID,
        RequireState = dbSku.RequireState,
        RequireProfileID = dbSku.RequireProfileID,
        IsDefault = dbSku.IsDefault,
        SubscriptionQuantityRemaining = dbSku.SubscriptionQuantityRemaining,
        SubscriptionOrderID = dbSku.SubscriptionOrderID
      };
    }

    public static ClientLib.OrderItem GetOrderItem(CallContext context, DBSku sku, string strProfileID=null, string strState=null, int intQueryID=0)
    {
      var orderItem = new ClientLib.OrderItem
      {
        ProductID = sku.SkuID,
        ProductExternalID = sku.ProductExternalID,
        ProductName = sku.ProductName,
        ProductCode = sku.ProductCode,
        SkuCode = sku.SkuCode,
        ProductToken = sku.ProductToken,
        OfferToken = sku.OfferToken,
        Quantity = 1,
        RegularPrice = sku.Price + sku.DiscountAmount,
        DiscountAmount = sku.DiscountAmount,
        Price = sku.Price,
        RecurringPrice = sku.RecurringPrice,
        QueryID = intQueryID,
        ProfileID = strProfileID,
        State = strState,
        SkuID = sku.SkuID,
        ReportTypeCode = sku.ReportTypeCode,
        SubscriptionQuantityRemaining = sku.SubscriptionQuantityRemaining,
        SubscriptionOrderID = sku.SubscriptionOrderID
      };

      if (sku.RequireQueryID || context.DBVisit.CurrentQueryID != 0)
      {
        using (var db = new AciXDB())
        {
          var query = db.spQueryGet(context.DBVisit.CurrentQueryID).FirstOrDefault();
          if (query != null)
          {
            orderItem.FirstName = query.FirstName;
            orderItem.MiddleInitial = query.MiddleInitial;
            orderItem.LastName = query.LastName;
            orderItem.State = strState ?? query.State;
            orderItem.ProductName = orderItem.ProductName +
                                    " for " + query.FirstName +
                                    (String.IsNullOrEmpty(orderItem.MiddleInitial.Trim()) ? "" : " " + orderItem.MiddleInitial) +
                                    " " + orderItem.LastName +
                                    (String.IsNullOrEmpty(orderItem.State) ? "" : " in " + GeneralPurposeCache.Singleton.StateByAbbr(orderItem.State).StateName);
          }
        }
      }
      return orderItem;
    }

    public static ClientLib.Sku[] Render(DBSku[] dbSkus)
    {
      return (from p in dbSkus select Render(p)).ToArray();
    }

    public static void RefreshCatalog(CallContext context)
    {
      //SiteSkuCache.ForSite(context.SiteID).Flush();
      IwsCatalogClient catalogCli = new IwsCatalogClient();
      var storefrontCli = new StorefrontClient(context);
      using (var db = new AciXDB())
      {
        db.spProductInitAll();
        var products = db.spProductGet(
          intSiteID: context.SiteID,
          boolReturnAllSkus: true,
          intUserID: null,
          intProductIDs: null);
        //var catalog = CatalogCache.Singleton.Get(context.SiteID, boolForceRefresh: true);
        var intExternalProductIDs = (from p in products select p.ProductExternalID).Distinct().ToArray();
        string strXml = catalogCli.GetProductsXml(intExternalProductIDs);
        db.spProductRefresh(context.SiteID, strXml);

        /*
         * Now we need to get product tokens for the discounted versions of all skus
         * that are discounts of a regular product sku
         */
        products = db.spProductGet(
          intSiteID: context.SiteID,
          boolReturnAllSkus: true,
          intUserID: null,
          intProductIDs: null);

        foreach (var product in products)
        {
          foreach (var sku in product.Skus)
          {
            if(sku.SubscriptionDiscountCode != null)
            {
              var strOfferToken = storefrontCli.ApplyPromoItem(
                strProductToken: sku.ProductToken,
                strPromoCode: sku.SubscriptionDiscountCode);
              if (strOfferToken != null)
              {
                db.spSkuSetOfferToken(context.SiteID, sku.SkuID, strOfferToken);
              }
            }
          }
        }

       //SiteSkuCache.ForSite(context.SiteID).Flush();
      }
    }

  }
}
