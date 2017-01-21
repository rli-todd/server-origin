using System;
using Solishine.CommonLib;
using System.Runtime.Serialization;

namespace Aci.X.DatabaseEntity
{
  [DataContract(Namespace = "")]
  public class DBProductSku : MySqlResult
  {
    [DataMember(EmitDefaultValue = false)] public int ProductSkuID;
    [DataMember(EmitDefaultValue = false)] public int ProductID;
    [DataMember(EmitDefaultValue = false)] public string ProductType;
    [DataMember(EmitDefaultValue = false)] public string ProductSkuName;
    [DataMember(EmitDefaultValue = false)] public int ProductExternalID;
    [DataMember(EmitDefaultValue = false)] public Decimal Price;
    [DataMember(EmitDefaultValue = false)] public Decimal DiscountAmount;
    [DataMember(EmitDefaultValue = false)] public Decimal RecurringPrice;
    [DataMember(EmitDefaultValue = false)] public bool RequireQueryID;
    [DataMember(EmitDefaultValue = false)] public bool RequireProfileID;

    public override void Read()
    {
      ProductSkuID = Value<int>("ID");
      ProductID = Value<int>("ProductID");
      ProductSkuID = Value<int>("ProductSkuID");
      ProductType = Value<string>("ProductType");
      ProductSkuName = Value<string>("ProductSkuName");
      ProductExternalID = Value<int>("ProductExternalID");
      Price = Value<Decimal>("Price");
      DiscountAmount = Value<Decimal>("DiscountAmount");
      RecurringPrice = Value<Decimal>("RecurringPrice");
      RequireQueryID = Value<bool>("RequireQueryID");
      RequireProfileID = Value<bool>("RequireProfileID");
    }
  }
}
