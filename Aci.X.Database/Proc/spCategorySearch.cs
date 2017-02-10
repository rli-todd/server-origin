using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spCategorySearch : MyStoredProc
  {
    public spCategorySearch(DbConnection conn)
      : base(strProcName: "spCategorySearch", conn: conn)
    {
    }

    public int[] Execute(
      int intSiteID,
      int[] intExternalCategoryIDs)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.Add(new SqlParameter("@ExternalCategoryIDs", SqlDbType.Structured)
      {
        TypeName = "dbo.ID_TABLE",
        Value = new IDTable(intExternalCategoryIDs)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetIntResults();
      }
    }
  }
}



