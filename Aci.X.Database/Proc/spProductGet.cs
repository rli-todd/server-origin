using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spProductGet : MyStoredProc
  {
    public spProductGet(DbConnection conn)
      : base(strProcName: "spProductGet", conn: conn)
    {
    }

    public DBProduct[] Execute(
      int intSiteID, 
      int? intUserID=null, 
      bool boolReturnAllSkus=false,
      int[] intProductIDs=null)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@UserID", intUserID);
      Parameters.AddWithValue("@ReturnAllSkus", boolReturnAllSkus);

      Parameters.Add(new SqlParameter("@ProductKeys", SqlDbType.Structured)
      {
        TypeName = "dbo.ID_TABLE",
        Value = new IDTable(intProductIDs)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        var products = reader.GetResults<DBProduct>();
        var skus = reader.GetResults<DBSku>();

        DBProductSkuDictionary dictProductSkus  = new DBProductSkuDictionary();
        foreach (var sku in skus)
        {
          if (!dictProductSkus.ContainsKey(sku.ProductID))
            dictProductSkus[sku.ProductID] = new List<DBSku>();
          dictProductSkus[sku.ProductID].Add(sku);
        }
        foreach (var product in products)
        {
          product.Skus = dictProductSkus.ContainsKey(product.ProductID)
            ? dictProductSkus[product.ProductID].ToArray()
            : new DBSku[0];
        }
        return products;
      }
    }
  }
}



