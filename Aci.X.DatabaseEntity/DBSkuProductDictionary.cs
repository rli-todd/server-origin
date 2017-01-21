using System.Collections.Generic;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBSkuProductDictionary : Dictionary<int, List<DBProduct>>
  {
    public DBSkuProductDictionary()
    {
    }

    public DBSkuProductDictionary(DBCategory[] categories)
    {
      foreach (var category in categories)
      {
        foreach (var sku in category.Skus)
        {
          foreach (var product in sku.Products)
          {
            if (!ContainsKey(sku.SkuID))
            {
              this[sku.SkuID] = new List<DBProduct>();
            }
            this[sku.SkuID].Add(product);
          }
        }
      }
    }
  }
}
