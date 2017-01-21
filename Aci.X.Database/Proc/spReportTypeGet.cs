using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spReportTypeGet : MyStoredProc
  {
    public spReportTypeGet(DbConnection conn)
      : base(strProcName: "spReportTypeGet", conn: conn)
    {
    }

    public DBReportType[] Execute(int intSiteID, string[] intKeys)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.Add(new SqlParameter
      {
        ParameterName = "@Keys",
        SqlDbType = SqlDbType.Structured,
        Value = new StringTable(intKeys)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBReportType>();
      }
    }
  }
}



