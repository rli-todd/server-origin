using System;
using System.Collections.Generic;
using System.Linq;
using Aci.X.Business.Cache;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.IwsLib;
using Aci.X.ServerLib;

namespace Aci.X.Business
{
  public class Product
  {
    public static ClientLib.Product Render(DBProduct dbProduct)
    {
      return new ClientLib.Product
      {
        ProductID = dbProduct.ProductID,
        SkuID = dbProduct.SkuID,
        ProductName = dbProduct.ProductName,
        Price = dbProduct.Price,
        DiscountAmount = dbProduct.DiscountAmount,
        RecurringPrice = dbProduct.RecurringPrice,
        RequireQuery = dbProduct.RequireQueryID,
        RequireState = dbProduct.RequireState,
        RequireProfile = dbProduct.RequireProfileID
      };
    }

    public static ClientLib.OrderItem GetOrderItem(CallContext context, DBProduct product, string strProfileID=null, string strState=null, int intQueryID=0)
    {
      var orderItem = new ClientLib.OrderItem
      {
        ProductID = product.ProductID,
        ProductExternalID = product.ProductExternalID,
        ProductToken = product.ProductToken,
        ProductName = product.ProductName,
        Quantity = 1,
        RegularPrice = product.Price + product.DiscountAmount,
        DiscountAmount = product.DiscountAmount,
        Price = product.Price,
        RecurringPrice = product.RecurringPrice,
        QueryID = intQueryID,
        ProfileID = strProfileID,
        State = strState,
        SkuID = product.SkuID,
        ReportTypeCode = product.ReportTypeCode
      };

      if (product.RequireQueryID || context.DBVisit.CurrentQueryID != 0)
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

    public static ClientLib.Product[] Render(DBProduct[] dbProducts)
    {
      return (from p in dbProducts select Render(p)).ToArray();
    }

    public static void RefreshCatalog(CallContext context)
    {
      IwsCatalogClient catalogCli = new IwsCatalogClient();
      using (var db = new AciXDB())
      {
        db.spProductInitAll();
        var catalog = CatalogCache.Singleton.Get(context.SiteID, boolForceRefresh: true);
        List<int> listProductIDs = new List<int>();
        foreach (var category in catalog.Categories)
        {
          listProductIDs.Add(category.ExternalID);
          foreach (var sku in category.Skus)
          {
            listProductIDs.Add(sku.ProductExternalID);
            foreach (var product in sku.Products)
            {
              listProductIDs.Add(product.ProductExternalID);
            }
          }
        }
        int[] intExternalProductIDs = listProductIDs.Distinct().ToArray();
        string strXml = catalogCli.GetProductsXml(intExternalProductIDs);
        db.spProductRefresh(context.SiteID, strXml);
        db.spProductInitAll(); // Sets the RequireQueryID and RequireProfileID bits where appropriate
        // Clear out the global cache so that the next reference will reload current data.
      }
    }

  }
}
