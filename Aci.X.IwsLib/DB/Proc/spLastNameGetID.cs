using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spLastNameGetID : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter("@ReturnVal", System.Data.SqlDbType.Int);

    public spLastNameGetID(DbConnection conn)
      : base("spLastNameGet", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@LastName", System.Data.SqlDbType.VarChar);
    }

    public int Execute(string strLastName)
    {
      Parameters["@LastName"].Value = strLastName;
      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
