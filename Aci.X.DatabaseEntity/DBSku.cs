using System;
using Solishine.CommonLib;
using System.Runtime.Serialization;

namespace Aci.X.DatabaseEntity
{
  [DataContract(Namespace = "")]
  public class DBSku : MySqlResult
  {
    [DataMember(EmitDefaultValue = false)] public int SkuID;
    [DataMember(EmitDefaultValue = false)] public int ProductID;
    [DataMember(EmitDefaultValue = false)] public string ProductName;
    [DataMember(EmitDefaultValue = false)] public int ProductExternalID;
    [DataMember(EmitDefaultValue = false)] public string ProductToken;
    [DataMember(EmitDefaultValue = false)] public string OfferToken;
    [DataMember(EmitDefaultValue = false)] public Decimal DiscountAmount;
    [DataMember(EmitDefaultValue = false)] public Decimal RecurringPrice;
    [DataMember(EmitDefaultValue = false)] public Decimal MSRP;
    [DataMember(EmitDefaultValue = false)] public Decimal Price;
    [DataMember(EmitDefaultValue = false)] public string ReportTypeCode;
    [DataMember(EmitDefaultValue = false)] public string ProductCode;
    [DataMember(EmitDefaultValue = false)] public string SkuCode;
    [DataMember(EmitDefaultValue = false)] public bool RequireQueryID;
    [DataMember(EmitDefaultValue = false)] public bool RequireState;
    [DataMember(EmitDefaultValue = false)] public bool RequireProfileID;
    [DataMember(EmitDefaultValue = false)] public bool IsDefault;
    [DataMember(EmitDefaultValue = false)] public string SubscriptionDiscountCode;
    [DataMember(EmitDefaultValue = false)] public string SubscriptionDiscountType;
    [DataMember(EmitDefaultValue = false)] public int SubscriptionQuantityRemaining; // for active subscriptions
    [DataMember(EmitDefaultValue = false)] public int SubscriptionOrderID; // for active subscriptions

    
    public override void Read()
    {
      SkuID = Value<int>("ID");
      ProductID = Value<int>("ProductID");
      ProductName = Value<string>("ProductName");
      ProductExternalID = Value<int?>("ProductExternalID") ?? 0;
      ProductToken = Value<string>("ProductToken");
      OfferToken = Value<string>("OfferToken");
      MSRP = Value<Decimal>("MSRP");
      Price = Value<Decimal>("Price");
      DiscountAmount = Value<Decimal>("DiscountAmount");
      RecurringPrice = Value<Decimal>("RecurringPrice");
      ReportTypeCode = Value<string>("ReportTypeCode");
      ProductCode = Value<string>("ProductCode");
      SkuCode = Value<string>("SkuCode");
      IsDefault = Value<bool>("IsDefault");
      RequireQueryID = Value<bool>("RequireQueryID");
      RequireState = Value<bool>("RequireState");
      RequireProfileID = Value<bool>("RequireProfileID");
      SubscriptionDiscountCode = Value<string>("SubscriptionDiscountCode");
      SubscriptionDiscountType = Value<string>("SubscriptionDiscountType");
      SubscriptionQuantityRemaining = Value<int>("SubscriptionQuantityRemaining");
      SubscriptionOrderID = Value<int>("SubscriptionOrderID");
    }
  }
}
