using System;
using System.Data.Common;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchStatsGetAll2 : MyStoredProc
  {
    public spSearchStatsGetAll2(DbConnection conn)
      : base(strProcName: "spSearchStatsGetAll2", conn: conn)
    {
      Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@LastName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@State", System.Data.SqlDbType.VarChar);
      Parameters.Add("@FirstNameID", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.InputOutput;
      Parameters.Add("@LastNameID", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.InputOutput;
    }

    public DBSearchStats[] Execute(
      string strFirstName,
      string strLastName,
      string strState,
      ref int? intFirstNameID,
      ref int? intLastNameID,
      int intTimeoutSecs = 30)
    {
      if (intTimeoutSecs < 0)
      {
        throw new TimeoutException("Timeout < 0");
      }
      CommandTimeoutSecs = intTimeoutSecs;
      Parameters["@FirstName"].Value = strFirstName;
      Parameters["@LastName"].Value = strLastName;
      Parameters["@State"].Value = strState;
      Parameters["@FirstNameID"].Value = intFirstNameID;
      Parameters["@LastNameID"].Value = intLastNameID;

      using (MySqlDataReader reader = ExecuteReader())
      {
        DBSearchStats[] retVal = reader.GetResults<DBSearchStats>();
        var oFirstNameID = Parameters["@FirstNameID"].Value;
        var oLastNameID = Parameters["@LastNameID"].Value;
        intFirstNameID = oFirstNameID is DBNull ? null : (int?)oFirstNameID;
        intLastNameID = oLastNameID is DBNull ? null : (int?)oLastNameID;
        return retVal;
      }
    }
  }
}
