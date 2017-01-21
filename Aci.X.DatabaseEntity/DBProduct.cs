using System;
using Solishine.CommonLib;
using System.Runtime.Serialization;

namespace Aci.X.DatabaseEntity
{
  [DataContract(Namespace = "")]
  public class DBProduct : MySqlResult
  {
    [DataMember(EmitDefaultValue = false)] public int ProductID;
    [DataMember(EmitDefaultValue = false)] public int SkuID;
    [DataMember(EmitDefaultValue = false)] public int ProductExternalID;
    [DataMember(EmitDefaultValue = false)] public string ProductToken;
    [DataMember(EmitDefaultValue = false)] public Decimal Price;
    [DataMember(EmitDefaultValue = false)] public Decimal DiscountAmount;
    [DataMember(EmitDefaultValue = false)] public Decimal RecurringPrice;
    [DataMember(EmitDefaultValue = false)] public string ProductName;
    [DataMember(EmitDefaultValue = false)] public bool RequireQueryID;
    [DataMember(EmitDefaultValue = false)] public bool RequireState;
    [DataMember(EmitDefaultValue = false)] public bool RequireProfileID; 
    [DataMember(EmitDefaultValue = false)] public string ReportTypeCode;
    
    public override void Read()
    {
      ProductID = Value<int>("ID");
      SkuID = Value<int>("SkuID");
      ProductExternalID = Value<int>("ProductExternalID");
      ProductToken = Value<string>("ProductToken");
      Price = Value<Decimal>("Price");
      DiscountAmount = Value<Decimal>("DiscountAmount");
      RecurringPrice = Value<Decimal>("RecurringPrice");
      ProductName = Value<string>("ProductName");
      RequireQueryID = Value<bool>("RequireQueryID");
      RequireState = Value<bool>("RequireState");
      RequireProfileID = Value<bool>("RequireProfileID");
      ReportTypeCode = Value<string>("ReportTypeCode");
    }
  }
}
