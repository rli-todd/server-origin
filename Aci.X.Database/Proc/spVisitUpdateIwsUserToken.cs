using System.Data;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spVisitUpdateIwsUserToken : MyStoredProc
  {
    public spVisitUpdateIwsUserToken(DbConnection conn)
      : base(strProcName: "spVisitUpdateIwsUserToken", conn: conn)
    {
    }

    public DBVisit Execute(int intSiteID, int intVisitID, int intUserID, string strIwsUserToken, int intIwsUserID,string strStorefrontUserToken)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@VisitID", intVisitID);
      Parameters.AddWithValue("@UserID", intUserID);
      Parameters.AddWithValue("@IwsUserToken", strIwsUserToken);
      Parameters.AddWithValue("@IwsUserID", intIwsUserID);
      Parameters.AddWithValue("@StorefrontUserToken", strStorefrontUserToken);
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBVisit[] tokens = reader.GetResults<DBVisit>();
        return tokens[0];
      }
    }
  }
}



