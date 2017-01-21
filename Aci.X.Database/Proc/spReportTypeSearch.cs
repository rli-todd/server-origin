using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spReportTypeSearch : MyStoredProc
  {
    public spReportTypeSearch(DbConnection conn)
      : base(strProcName: "spReportTypeSearch", conn: conn)
    {
    }

    public string[] Execute(int intSiteID)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetStringResults();
      }
    }
  }
}



