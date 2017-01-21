using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spReportGet : MyStoredProc
  {
    public spReportGet(DbConnection conn)
      : base(strProcName: "spReportGet", conn: conn)
    {
    }

    public DBReport[] Execute(int intSiteID, int[] intKeys)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.Add(new SqlParameter
      {
        ParameterName = "@Keys",
        SqlDbType = SqlDbType.Structured,
        Value = new IDTable(intKeys)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBReport>();
      }
    }
  }
}



