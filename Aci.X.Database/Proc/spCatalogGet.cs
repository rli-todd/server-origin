using System.Collections.Generic;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spCatalogGet : MyStoredProc
  {
    public spCatalogGet(DbConnection conn)
      : base(strProcName: "spCatalogGet", conn: conn)
    {
    }

    public DBCatalog Execute(int intSiteID)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      using (MySqlDataReader reader = ExecuteReader())
      {
        var categories = reader.GetResults<DBCategory>();
        var skus = reader.GetResults<DBSku>();
        var products = reader.GetResults<DBProduct>();

        Dictionary<int, List<DBSku>> dictCategorySkus = new Dictionary<int, List<DBSku>>();
        Dictionary<int, List<DBProduct>> dictSkuProducts = new Dictionary<int, List<DBProduct>>();
        foreach (var product in products)
        {
          if (!dictSkuProducts.ContainsKey(product.SkuID))
            dictSkuProducts[product.SkuID] = new List<DBProduct>();
          dictSkuProducts[product.SkuID].Add(product);
        }
        foreach (var sku in skus)
        {
          if (!dictCategorySkus.ContainsKey(sku.CategoryID))
            dictCategorySkus[sku.CategoryID] = new List<DBSku>();
          dictCategorySkus[sku.CategoryID].Add(sku);
          sku.Products = dictSkuProducts.ContainsKey(sku.SkuID)
            ? dictSkuProducts[sku.SkuID].ToArray()
            : new DBProduct[0];
        }
        foreach (var category in categories)
        {
          category.Skus = dictCategorySkus.ContainsKey(category.CategoryID)
            ? dictCategorySkus[category.CategoryID].ToArray()
            : new DBSku[0];
        }
        return new DBCatalog
        {
          SiteID = intSiteID,
          Categories = categories,
          DictSkuProducts = new DBSkuProductDictionary(categories)

        };
      }
    }
  }
}



