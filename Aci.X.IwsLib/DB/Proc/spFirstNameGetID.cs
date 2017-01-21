using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spFirstNameGetID : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter("@ReturnVal", System.Data.SqlDbType.Int);

    public spFirstNameGetID(DbConnection conn)
      : base("spFirstNameGet", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar);
    }

    public int Execute(string strFirstName)
    {
      Parameters["@FirstName"].Value = strFirstName;
      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
