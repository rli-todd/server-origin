using System.Collections.Generic;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBProductSkuDictionary : Dictionary<int, List<DBSku>>
  {
    public DBProductSkuDictionary()
    {
    }

    public DBProductSkuDictionary(DBProduct[] products)
    {
      foreach (var product in products)
      {
        foreach (var sku in product.Skus)
        {
          if (!ContainsKey(product.ProductID))
          {
            this[product.ProductID] = new List<DBSku>();
          }
          this[product.ProductID].Add(sku);
        }
      }
    }
  }
}
