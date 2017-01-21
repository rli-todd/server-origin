using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spCacheGet : MyStoredProc
  {
    public spCacheGet(DbConnection conn)
      : base(strProcName: "spCacheGet", conn: conn)
    {
      Parameters.Add("@Key", System.Data.SqlDbType.VarChar);
    }

    public DBCacheItem Execute(string strKey)
    {
      Parameters["@Key"].Value = strKey;
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBCacheItem[] items = reader.GetResults<DBCacheItem>();
        return items.Length > 0 ? items[0] : null;
      }
    }
  }
}



