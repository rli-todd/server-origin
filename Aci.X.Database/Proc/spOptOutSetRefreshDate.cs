using System;
using System.Data.Common;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spOptOutSetRefreshDate : MyStoredProc
  {
    public spOptOutSetRefreshDate(DbConnection conn)
      : base(strProcName: "spOptOutSetRefreshDate", conn: conn)
    {
    }

    public void Execute(string strFirstName, string strLastName, string strState)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@FirstName", strFirstName);
      Parameters.AddWithValue("@LastName", strLastName);
      Parameters.AddWithValue("@State", strState);
      base.ExecuteNonQuery();
    }
  }
}
