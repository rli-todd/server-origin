using System.Data;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spVisitGetByToken : MyStoredProc
  {
    public spVisitGetByToken(DbConnection conn)
      : base(strProcName: "spVisitGetByToken", conn: conn)
    {
      Parameters.Add("@VisitGuid", SqlDbType.VarChar);
    }

    public DBVisit Execute( string strVisitGuid) 
    {
      Parameters["@VisitGuid"].Value = strVisitGuid;
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBVisit[] tokens = reader.GetResults<DBVisit>();
        return tokens.Length > 0 ? tokens[0] : null;
      }
    }
  }
}



