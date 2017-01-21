using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spMiddleNameGetID : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter("@ReturnVal", System.Data.SqlDbType.Int);

    public spMiddleNameGetID(DbConnection conn)
      : base("spMiddleNameGetID", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@MiddleName", System.Data.SqlDbType.VarChar);
    }

    public int Execute(string strMiddleName)
    {
      Parameters["@MiddleName"].Value = strMiddleName;
      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
