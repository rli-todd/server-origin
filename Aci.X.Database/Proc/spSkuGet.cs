using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spSkuGet : MyStoredProc
  {
    public spSkuGet(DbConnection conn)
      : base(strProcName: "spSkuGet", conn: conn)
    {
    }
    public DBSku[] Execute(byte tSiteID, int[] intSkuIDs)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", tSiteID);
      Parameters.Add(new SqlParameter("@SkuKeys", SqlDbType.Structured)
      {
        TypeName = "dbo.ID_TABLE",
        Value = new IDTable(intSkuIDs)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
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
        return skus;
      }
    }
  }
}



