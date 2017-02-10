using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spUserSubscriptionItemGet : MyStoredProc
  {
    public spUserSubscriptionItemGet(DbConnection conn)
      : base(strProcName: "spUserSubscriptionItemGet", conn: conn)
    {
    }

    public DBUserSubscriptionItem[] Execute(int intSiteID, int intUserID)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@UserID", intUserID);
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBUserSubscriptionItem>();
      }
    }
  }
}



