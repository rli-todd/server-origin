using System.Linq;
using Client = Aci.X.ClientLib;
using TRAN = Aci.X.IwsLib.Commerce.v2_1.Transaction;
using DB = Aci.X.DatabaseEntity;
using Aci.X.Database;

namespace Aci.X.Business
{
  public class Credit
  {
    public static Client.Credit[] Render(byte tSiteID, int? intUserID, TRAN.Credit[] credits)
    {
      using (var db = new AciXDB())
      {
        var externalCategoryIDs = (from c in credits select c.PackageID).ToArray();
        //var skuCache = Cache.SiteCategoryCacheByExternalID.ForSite(tSiteID);
        //var skus = skuCache.Get(externalCategoryIDs);

        var externalOrderIDs = (from c in credits select c.TxnID).ToArray();
        var orderCache = Cache.SiteOrderCacheByExternalID.ForSite(tSiteID);
        var orders = orderCache.Get(externalOrderIDs);

        var retVal = new Client.Credit[credits.Length];
        bool boolInvalidCreditsFound = false;
        for (int idx = 0; idx < credits.Length; idx++)
        {
          var c = credits[idx];
          var product = db.spProductGet(
            intSiteID: tSiteID,
            intUserID: intUserID,
            intProductIDs: new int[] { c.PackageID }).FirstOrDefault();
          var order = orderCache.Get(c.TxnID);
          if (product == null || order == null)
          {
            boolInvalidCreditsFound = true;
            continue;
          }
          retVal[idx] = new Client.Credit
          {
            CategoryID = product.ProductID,
            OrderID = order.OrderID,
            OrderExternalID = order.OrderExternalID,
            Quantity = c.Units,
            QuantityLeft = c.UnitsLeft,
            QuantityUsed = c.UnitsUsed,
            ProductName = product.ProductName,
            ReportTypeCode = product.ReportTypeCode
          };
        }
        if (boolInvalidCreditsFound)
        {
          retVal = (from credit in retVal where credit != null select credit).ToArray();
        }
        return retVal;
      }
    }
  }
}
