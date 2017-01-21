using System;
using System.Data.Common;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchResultsSaveMinimized : MyStoredProc
  {
    public spSearchResultsSaveMinimized(DbConnection conn)
      : base(strProcName: "spSearchResultsSaveMinimized", conn: conn)
    {
    }

    public void Execute(
      int intQueryID,
      byte[] tCompressedResults)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@QueryID", intQueryID);
      Parameters.AddWithValue("@CompressedResults", tCompressedResults);
      ExecuteNonQuery();
    }
  }
}
