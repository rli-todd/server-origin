using System.Linq;
using Aci.X.Business.Cache;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
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
        ProductName = dbProduct.ProductName,
        IsActive = dbProduct.IsActive,
        ProductCode = dbProduct.ProductCode,
        RequireQuery = dbProduct.RequireQueryID,
        RequireState = dbProduct.RequireState,
        RequireProfile = dbProduct.RequireProfileID,
        ProductType = dbProduct.ProductType,
        ProductExternalID = dbProduct.ProductExternalID,
        Skus = (from s in dbProduct.Skus select Sku.Render(s)).ToArray()
      };
    }

    public static ClientLib.Product[] GetAll(CallContext context)
    {
      using (var db = new AciXDB())
      {
        var dbProducts = db.spProductGet(
          intSiteID: context.SiteID,
          intUserID: context.AuthorizedUserID,
          intProductIDs: null);

        /*
         * If the user is logged in, we need to see if they have an active
         * subscription.
         */
        if (context.AuthorizedUserID != 0)
        {
          var subCli = new IwsLib.IwsSubscriptionClient(context);
          var subs = subCli.GetSubscriptions();
        }
        return (from p in dbProducts select Render(p)).ToArray();
      }
    }
  }
}
