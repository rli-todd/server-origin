using System.Linq;
using Aci.X.Business.Cache;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;

namespace Aci.X.Business
{
  public class Category
  {
    public static ClientLib.Category Render(DBCategory dbCategory)
    {
      return new ClientLib.Category
      {
        CategoryID = dbCategory.CategoryID,
        CategoryCode = dbCategory.CategoryCode,
        CategoryName = dbCategory.CategoryName,
        SKUs = (from sku in dbCategory.Skus select Sku.Render(sku)).ToArray()
      };
    }

    public static ClientLib.Category[] GetAll(CallContext context)
    {
      var dbCatagories = CatalogCache.Singleton.Get(context.SiteID).Categories;
      return (from c in dbCatagories select Render(c)).ToArray();
    }
  }
}
