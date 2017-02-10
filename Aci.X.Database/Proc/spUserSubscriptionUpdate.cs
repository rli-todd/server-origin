using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spUserSubscriptionUpdate : MyStoredProc
  {
    public spUserSubscriptionUpdate(DbConnection conn)
      : base(strProcName: "spUserSubscriptionUpdate", conn: conn)
    {
    }

    public int[] Execute(int intSiteID, int intUserID, string strSubscriptionXml)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@UserID", intUserID);
      Parameters.AddWithValue("@SubscriptionXml", strSubscriptionXml);
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetIntResults();
      }
    }
  }
}



