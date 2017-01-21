using System;
using System.Data.Common;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spOptOutGetUnrefreshed : MyStoredProc
  {
    public spOptOutGetUnrefreshed(DbConnection conn)
      : base(strProcName: "spOptOutGetUnrefreshed", conn: conn)
    {
    }

    public DBOptOut[] Execute()
    {
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetResults<DBOptOut>();
      }
    }
  }
}
