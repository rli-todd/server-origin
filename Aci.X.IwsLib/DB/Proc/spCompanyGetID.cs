using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spCompanyGetID : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter("@ReturnVal", System.Data.SqlDbType.Int);

    public spCompanyGetID(DbConnection conn)
      : base("spCompanyGetID", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@CompanyName", System.Data.SqlDbType.NVarChar);
    }

    public int Execute(string strCompanyName)
    {
      Parameters["@CompanyName"].Value = strCompanyName;
      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
