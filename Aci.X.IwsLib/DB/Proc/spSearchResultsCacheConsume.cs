using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spSearchResultsCacheConsume : MyStoredProc
  {
    private SqlParameter _returnCode = new SqlParameter("@ReturnVal", System.Data.SqlDbType.Int);

    public spSearchResultsCacheConsume(DbConnection conn)
      : base("spSearchResultsCacheConsume", conn)
    {
      _returnCode.Direction = System.Data.ParameterDirection.ReturnValue;
      Parameters.Clear();
      Parameters.Add(_returnCode);
      Parameters.Add("@ListSearchResultsIDs", System.Data.SqlDbType.VarChar);
    }

    public int Execute(string strListSearchResultsIDs)
    {
      Parameters["@ListSearchResultsIDs"].Value = strListSearchResultsIDs;
      base.ExecuteNonQuery();
      return (int)_returnCode.Value;
    }
  }
}
