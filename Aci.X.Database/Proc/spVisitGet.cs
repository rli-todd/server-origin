using System.Data;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spVisitGet : MyStoredProc
  {
    public spVisitGet(DbConnection conn)
      : base(strProcName: "spVisitGet", conn: conn)
    {
      Parameters.Add("@VisitID", SqlDbType.Int);
    }
    public DBVisit Execute(int intVisitID)
    {
      Parameters["@VisitID"].Value = intVisitID;
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBVisit[] tokens = reader.GetResults<DBVisit>();
        return tokens.Length > 0 ? tokens[0] : null;
      }
    }
  }
}



