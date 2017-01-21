using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spPageGet : MyStoredProc
  {
    public spPageGet(DbConnection conn)
      : base(strProcName: "spPageGet", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@PageID", System.Data.SqlDbType.Int);
      Parameters.Add("@BlockID", System.Data.SqlDbType.Int);
    }
    
    public DBPage[] Execute(int intAuthorizedUserID, int? intPageID=null, int? intBlockID=null)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@PageID"].Value = intPageID;
      Parameters["@BlockID"].Value = intBlockID;
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBPage>();
      }
    }
  }
}



