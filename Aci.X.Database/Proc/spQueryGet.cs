using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spQueryGet : MyStoredProc
  {
    public spQueryGet(DbConnection conn)
      : base(strProcName: "spQueryGet", conn: conn)
    {
    }

    public DBQuery[] Execute(int intQueryID)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@QueryID", intQueryID);
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBQuery>();
      }
    }
  }
}



