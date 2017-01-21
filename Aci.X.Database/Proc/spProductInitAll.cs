using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spProductInitAll : MyStoredProc
  {
    public spProductInitAll(DbConnection conn)
      : base(strProcName: "spProductInitAll", conn: conn)
    {
    }

    public void Execute()
    {
      ExecuteNonQuery();
    }
  }
}



