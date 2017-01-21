using System.Linq;
using Client = Aci.X.ClientLib;
using TRAN = Aci.X.IwsLib.Commerce.v2_1.Transaction;
using DB = Aci.X.DatabaseEntity;
using Aci.X.Database;

namespace Aci.X.Business
{
  public class Credit
  {
    public static Client.Credit[] Render(byte tSiteID, TRAN.Credit[] credits)
    {
      using (var db = new AciXDB())
      {
        var externalSkuIDs = (from c in credits select c.PackageID).ToArray();
        var skuCache = Cache.SiteSkuCacheByExternalID.ForSite(tSiteID);
        var skus = skuCache.Get(externalSkuIDs);

        var externalOrderIDs = (from c in credits select c.TxnID).ToArray();
        var orderCache = Cache.SiteOrderCacheByExternalID.ForSite(tSiteID);
        var orders = orderCache.Get(externalOrderIDs);

        var retVal = new Client.Credit[credits.Length];
        bool boolInvalidCreditsFound = false;
        for (int idx = 0; idx < credits.Length; idx++)
        {
          var c = credits[idx];
          var sku = skuCache.Get(c.PackageID);
          var order = orderCache.Get(c.TxnID);
          if (sku == null || order == null)
          {
            boolInvalidCreditsFound = true;
            continue;
          }
          retVal[idx] = new Client.Credit
          {
            SkuID = sku.SkuID,
            OrderID = order.OrderID,
            OrderExternalID = order.OrderExternalID,
            Quantity = c.Units,
            QuantityLeft = c.UnitsLeft,
            QuantityUsed = c.UnitsUsed,
            ProductName = sku.Products[0].ProductName,
            ReportTypeCode = sku.Products[0].ReportTypeCode
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
