using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spSkuGet : MyStoredProc
  {
    public spSkuGet(DbConnection conn)
      : base(strProcName: "spSkuGet", conn: conn)
    {
    }

    public DBSku[] Execute(int intSiteID, int[] intSkuIDs = null)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.Add(new SqlParameter("@SkuKeys", SqlDbType.Structured)
      {
        TypeName = "dbo.ID_TABLE",
        Value = new IDTable(intSkuIDs)
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        var Skus = reader.GetResults<DBSku>();
        return Skus;
      }
    }
  }
}



