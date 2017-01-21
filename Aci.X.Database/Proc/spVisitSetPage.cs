using System.Linq;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spVisitSetPage : MyStoredProc
  {
    public spVisitSetPage(DbConnection conn)
      : base(strProcName: "spVisitSetPage", conn: conn)
    {
      Parameters.Add("@VisitID", System.Data.SqlDbType.Int);
      Parameters.Add("@PageCode", System.Data.SqlDbType.VarChar);
    }
    public void Execute(int intVisitID, string strPageCode)
    {
      Parameters["@VisitID"].Value = intVisitID;
      Parameters["@PageCode"].Value = strPageCode;
      ExecuteNonQuery();
    }
  }
}

