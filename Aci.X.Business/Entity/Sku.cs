using System.Linq;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;
using Aci.X.Database;

namespace Aci.X.Business
{
  public class Sku
  {
    public static ClientLib.Sku[] Render(DBSku[] dbSkus)
    {
      return (from s in dbSkus select Render(s)).ToArray();
    }

    public static ClientLib.Sku Render(DBSku dbSku)
    {
      return new ClientLib.Sku
      {
        SkuID = dbSku.SkuID,
        Products = (from p in dbSku.Products select Product.Render(p)).ToArray()
      };
    }

    public static ClientLib.Sku[] Get(CallContext context, int[] intSkuIDs)
    {
      var skus = Cache.SiteSkuCache.ForSite(context.SiteID).Get(intSkuIDs);
      return Render(skus);
    }

    //public static int[] Search(
    //  CallContext context,
    //  int[] intSkuIDs=null,
    //  int[] intExternalSkuIDs=null)
    //{
    //  using (var db = new AciXDB())
    //  {
    //    var skuIDs= db.spSkuSearch(
    //      intSiteID: context.SiteID,
    //      intSkuIDs: intSkuIDs,
    //      intExternalSkuIDs: intExternalSkuIDs);
    //    return Get(context, skuIDs);
    //  }
    //}

  }
}
