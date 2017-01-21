using System;
using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchResultsSetPersonIDs : MyStoredProc
  {
    public spSearchResultsSetPersonIDs(DbConnection conn)
      : base("spSearchResultsSetProfileIDs", conn)
    {
      Parameters.Clear();
      Parameters.Add("@QueryID", System.Data.SqlDbType.Int);
      Parameters.Add("@ListProfileIDs", System.Data.SqlDbType.VarChar);
    }

    public void Execute(int intQueryID, string strListPersonIDs)
    {
      Parameters["@QueryID"].Value = intQueryID;
      Parameters["@ListProfileIDs"].Value = strListPersonIDs;
      base.ExecuteNonQuery();
    }
  }
}