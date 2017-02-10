using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spUserSubscriptionItemDebit : MyStoredProc
  {
    public spUserSubscriptionItemDebit(DbConnection conn)
      : base(strProcName: "spUserSubscriptionItemDebit", conn: conn)
    {
    }

    public void Execute(int intSiteID, int intUserID, int intItemSkuID)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@UserID", intUserID);
      Parameters.AddWithValue("@ItemSkuID", intItemSkuID);
      ExecuteNonQuery();
    }
  }
}



