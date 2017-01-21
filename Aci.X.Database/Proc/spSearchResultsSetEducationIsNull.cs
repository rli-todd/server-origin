using System;
using System.Data.Common;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchResultsSetEducationIsNull : MyStoredProc
  {
    public spSearchResultsSetEducationIsNull(DbConnection conn)
      : base(strProcName: "spSearchResultsSetEducationIsNull", conn: conn)
    {
    }

    public void Execute(
      int intQueryID,
      bool boolEducationIsNull)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@QueryID", intQueryID);
      Parameters.AddWithValue("@IsEducationNull", boolEducationIsNull);
      ExecuteNonQuery();
    }
  }
}
