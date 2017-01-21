using System;
using System.Collections.Generic;
using System.Linq;
using Solishine.CommonLib;
using System.Runtime.Serialization;

namespace Aci.X.DatabaseEntity
{
  [DataContract(Namespace = "")]
  public class DBProductItem : MySqlResult
  {
    [DataMember(EmitDefaultValue = false)] public int ProductID;
    [DataMember(EmitDefaultValue = false)] public string ProductCode;
    [DataMember(EmitDefaultValue = false)] public int ProductSkuID;
    [DataMember(EmitDefaultValue = false)] public int ProductItemID;
    [DataMember(EmitDefaultValue = false)] public string ProductName;
    [DataMember(EmitDefaultValue = false)] public string ProductType;
    [DataMember(EmitDefaultValue = false)] public string ProductItemName;
    [DataMember(EmitDefaultValue = false)] public int ProductExternalID;
    [DataMember(EmitDefaultValue = false)] public int ProductItemExternalID;
    [DataMember(EmitDefaultValue = false)] public Decimal Price;
    [DataMember(EmitDefaultValue = false)] public Decimal DiscountAmount;
    [DataMember(EmitDefaultValue = false)] public Decimal RecurringPrice;
    [DataMember(EmitDefaultValue = false)] public bool RequireQueryID;
    [DataMember(EmitDefaultValue = false)] public bool RequireState;
    [DataMember(EmitDefaultValue = false)] public bool RequireProfileID;
    
    public override void Read()
    {
      ProductID = Value<int>("ProductID");
      ProductCode = Value<string>("ProductCode");
      ProductSkuID = Value<int>("ProductSkuID");
      ProductItemID = Value<int>("ProductItemID");
      ProductName = Value<string>("ProductName");
      ProductType = Value<string>("ProductType");
      ProductItemName = Value<string>("ProductItemName");
      ProductExternalID = Value<int>("ProductExternalID");
      ProductItemExternalID = Value<int>("ProductItemExternalID");
      Price = Value<Decimal>("Price");
      DiscountAmount = Value<Decimal>("DiscountAmount");
      RecurringPrice = Value<Decimal>("RecurringPrice");
      RequireQueryID = Value<bool>("RequireQueryID");
      RequireState = Value<bool>("RequireState");
      RequireProfileID = Value<bool>("RequireProfileID");
    }
  }

  public class DBProductItemDictionary : Dictionary<int,DBProductItem>
  {
    public DBProductItemDictionary(DBProductItem[] dbProductItems) :
      base( dbProductItems.ToDictionary(k=>k.ProductItemID))
    {
    }
  }

  public class DBProductSkuDictionary : Dictionary<int,List<DBProductItem>>
  {
    public DBProductSkuDictionary(DBProductItem[] dbProductItems)
    {
      foreach (DBProductItem item in dbProductItems)
      {
        if (!ContainsKey(item.ProductSkuID))
        {
          this[item.ProductSkuID] = new List<DBProductItem>();
        }
        this[item.ProductSkuID].Add(item);
      }
    }
  }
}
