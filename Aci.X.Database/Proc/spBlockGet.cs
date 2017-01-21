using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spBlockGet : MyStoredProc
  {
    public spBlockGet(DbConnection conn)
      : base(strProcName: "spBlockGet", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@BlockID", System.Data.SqlDbType.Int);
      Parameters.Add("@PageID", System.Data.SqlDbType.Int);
    }

    public DBBlock[] Execute(int intAuthorizedUserID, int? intPageID=null, int? intBlockID=null)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@BlockID"].Value = intBlockID;
      Parameters["@PageID"].Value = intPageID;
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBBlock>();
      }
    }
  }
}



