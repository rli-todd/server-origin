using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spSchoolGetID : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter("@ReturnVal", System.Data.SqlDbType.Int);

    public spSchoolGetID(DbConnection conn)
      : base("spSchoolGetID", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@SchoolName", System.Data.SqlDbType.NVarChar);
    }

    public int Execute(string strSchoolName)
    {
      Parameters["@SchoolName"].Value = strSchoolName;
      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
