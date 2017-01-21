using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spSkuSearch : MyStoredProc
  {
    public spSkuSearch(DbConnection conn)
      : base(strProcName: "spSkuSearch", conn: conn)
    {
    }

    public int[] Execute(
      int intSiteID,
      int[] intExternalSkuIDs)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.Add(new SqlParameter("@ExternalSkuIDs", SqlDbType.Structured)
      {
        TypeName = "dbo.ID_TABLE",
        Value = new IDTable(intExternalSkuIDs)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetIntResults();
      }
    }
  }
}



