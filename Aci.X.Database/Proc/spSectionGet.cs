using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spSectionGet : MyStoredProc
  {
    public spSectionGet(DbConnection conn)
      : base(strProcName: "spSectionGet", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@SectionID", System.Data.SqlDbType.Int);
      Parameters.Add("@BlockID", System.Data.SqlDbType.Int);
    }

    public DBSection[] Execute(int intAuthorizedUserID, int? intSectionID = null, int? intBlockID=null)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@SectionID"].Value = intSectionID;
      Parameters["@BlockID"].Value = intBlockID;
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBSection>();
      }
    }
  }
}



