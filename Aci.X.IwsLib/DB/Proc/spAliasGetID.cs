using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spAliasGetID : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter("@ReturnVal", System.Data.SqlDbType.Int);

    public spAliasGetID(DbConnection conn)
      : base("spAliasGetID", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@FirstNameID", System.Data.SqlDbType.Int);
      Parameters.Add("@MiddleNameID", System.Data.SqlDbType.Int);
      Parameters.Add("@LastNameID", System.Data.SqlDbType.Int);
    }

    public int Execute(
      int? intFirstNameID = null,
      int? intMiddleNameID = null,
      int? intLastNameID = null)
    {
      Parameters["@FirstNameID"].Value = intFirstNameID;
      Parameters["@MiddleNameID"].Value = intMiddleNameID;
      Parameters["@LastNameID"].Value = intLastNameID;

      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
