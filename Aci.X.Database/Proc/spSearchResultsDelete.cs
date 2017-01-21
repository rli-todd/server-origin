using System;
using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchResultsDelete : MyStoredProc
  {
    public spSearchResultsDelete(DbConnection conn)
      : base("spSearchResultsDelete", conn)
    {
      Parameters.Clear();
      Parameters.Add("@QueryID", System.Data.SqlDbType.Int);
    }

    public void Execute(int intQueryID)
    {
      Parameters["@QueryID"].Value = intQueryID;
      base.ExecuteNonQuery();
    }
  }
}