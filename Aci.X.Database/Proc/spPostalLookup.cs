using System;
using System.Data.Common;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("GeoDB")]
  public class spPostalLookup : MyStoredProc
  {
    public spPostalLookup(DbConnection conn)
      : base(strProcName: "spPostalLookup", conn: conn)
    {
    }

    public DBPostal Execute(string strPostalCode)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@PostalCode", strPostalCode);

      using (MySqlDataReader reader = ExecuteReader())
      {
        var results = reader.GetResults<DBPostal>();
        return results.Length > 0 ? results[0] : null;
      }
    }
  }
}
