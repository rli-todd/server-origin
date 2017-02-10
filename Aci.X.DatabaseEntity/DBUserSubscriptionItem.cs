using System;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBUserSubscriptionItem : MySqlResult
  {
    public byte SiteID;
    public int UserID;
    public int SubscriptionSkuID;
    public int ItemSkuID;
    public int QuantityRemaining;

    public override void Read()
    {
      SiteID = Value<byte>("SiteID");
      UserID = Value<int>("ID");
      SubscriptionSkuID = Value<int>("SubscriptionSkuID");
      ItemSkuID = Value<int>("ItemSkuID");
      QuantityRemaining = Value<int>("QuantityRemaining");
    }
  }
}
