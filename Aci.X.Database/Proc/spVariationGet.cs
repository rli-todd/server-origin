using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spVariationGet : MyStoredProc
  {
    public spVariationGet(DbConnection conn)
      : base(strProcName: "spVariationGet", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@VariationID", System.Data.SqlDbType.Int);
      Parameters.Add("@SectionID", System.Data.SqlDbType.Int);
    }

    public DBVariation[] Execute(int intAuthorizedUserID, int? intVariationID = null, int? intSectionID = null)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@VariationID"].Value = intVariationID;
      Parameters["@SectionID"].Value = intSectionID;
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBVariation>();
      }
    }
  }
}



