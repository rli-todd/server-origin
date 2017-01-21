using System;
using System.Data.Common;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spProfileGet : MyStoredProc
  {
    public spProfileGet(DbConnection conn)
      : base(strProcName: "spProfileGet", conn: conn)
    {
    }

    public DBProfile Execute(string strProfileID, string strProfileAttributes)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@ProfileID", strProfileID);
      Parameters.AddWithValue("@ProfileAttributes", strProfileAttributes);
      using (MySqlDataReader reader = ExecuteReader())
      {
        var results = reader.GetResults<DBProfile>();
        return (results.Length > 0)
          ? results[0]
          : null;
      }
    }
  }
}
