using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spUserSearch : MyStoredProc
  {
    public spUserSearch(DbConnection conn)
      : base(strProcName: "spUserSearch", conn: conn)
    {
    }

    public int Execute(
      int intSiteID,
      string strEmailAddress)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@EmailAddress", strEmailAddress);

      using (MySqlDataReader reader = ExecuteReader())
      {
        var results = reader.GetIntResults();
        return results.Length > 0 ? results[0] : 0;
      }
    }
  }
}



