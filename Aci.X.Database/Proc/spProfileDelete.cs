using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spProfileDelete : MyStoredProc
  {
    public spProfileDelete(DbConnection conn)
      : base(strProcName: "spProfileDelete", conn: conn)
    {
    }

    public void Execute(string strProfileID)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@ProfileID", strProfileID);
      base.ExecuteNonQuery();
    }
  }
}
