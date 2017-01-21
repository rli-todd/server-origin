using System.Linq;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spOrderValidateAccess : MyStoredProc
  {
    public spOrderValidateAccess(DbConnection conn)
      : base(strProcName: "spOrderValidateAccess", conn: conn)
    {
    }

    public int[] Execute(int intAuthorizedUserID, int intSiteID, int[] intKeys)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@AuthorizedUserID", intAuthorizedUserID);
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.Add(new SqlParameter
      {
        ParameterName = "@Keys",
        SqlDbType = SqlDbType.Structured,
        Value = new IDTable(intKeys)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetIntResults();
      }
    }
  }
}



