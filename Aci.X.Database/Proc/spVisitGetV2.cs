using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spVisitGetV2 : MyStoredProc
  {
    public spVisitGetV2(DbConnection conn)
      : base(strProcName: "spVisitGetV2", conn: conn)
    {
    }
    public DBVisit[] Execute(byte tSiteID, int[] intUserIDs)
    {
      Parameters.Add(new SqlParameter("@VisitKeys", SqlDbType.Structured)
      {
        TypeName = "dbo.ID_TABLE",
        Value = new IDTable(intUserIDs)
      });
      
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBVisit>();
      }
    }
  }
}



