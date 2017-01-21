using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spSiteGet : MyStoredProc
  {
    public spSiteGet(DbConnection conn)
      : base(strProcName: "spSiteGet", conn: conn)
    {
    }
    public DBSite[] Execute(byte[] tSiteIDs)
    {
      Parameters.Clear();
      Parameters.Add(new SqlParameter("@SiteKeys", SqlDbType.Structured)
      {
        TypeName = "dbo.ID_TABLE",
        Value = new IDTable(tSiteIDs)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBSite>();
      }
    }
  }
}



