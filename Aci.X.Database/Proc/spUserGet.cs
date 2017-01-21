using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spUserGet : MyStoredProc
  {
    public spUserGet(DbConnection conn)
      : base(strProcName: "spUserGet", conn: conn)
    {
    }
    public DBUser[] Execute(byte tSiteID, int[] intUserIDs)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", tSiteID);
      Parameters.Add(new SqlParameter("@UserKeys", SqlDbType.Structured)
      {
        TypeName = "dbo.ID_TABLE",
        Value = new IDTable(intUserIDs)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBUser>();
      }
    }
  }
}



