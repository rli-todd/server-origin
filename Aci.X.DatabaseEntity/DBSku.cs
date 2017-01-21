using System;
using Solishine.CommonLib;
using System.Runtime.Serialization;

namespace Aci.X.DatabaseEntity
{
  [DataContract(Namespace = "")]
  public class DBSku : MySqlResult
  {
    [DataMember(EmitDefaultValue = false)] public int SkuID;
    [DataMember(EmitDefaultValue = false)] public int CategoryID;
    [DataMember(EmitDefaultValue = false)] public int ProductExternalID;
    [DataMember(EmitDefaultValue = false)] public string ProductType;
    [DataMember(EmitDefaultValue = false)] public Decimal Price;
    [DataMember(EmitDefaultValue = false)] public Decimal RecurringPrice;
    [DataMember(EmitDefaultValue = false)] public DBProduct[] Products;

    public override void Read()
    {
      SkuID = Value<int>("ID");
      CategoryID = Value<int>("CategoryID");
      ProductExternalID = Value<int>("ProductExternalID");
      ProductType = Value<string>("ProductType");
      Price = Value<Decimal>("Price");
      RecurringPrice = Value<Decimal>("RecurringPrice");
    }
  }
}
