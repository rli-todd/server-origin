using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace IwsCacheMigrator2
{
  public class spIwsCacheCountUnmigrated: MyStoredProc
  {
    public spIwsCacheCountUnmigrated(DbConnection conn)
      : base(strProcName: "_spIwsCacheCountUnmigrated", conn: conn)
    {
      Parameters.Add("@CurrentQueryID", System.Data.SqlDbType.Int);
    }

    public int Execute(int? intCurrentQueryID)
    {
      Parameters["@CurrentQueryID"].Value = intCurrentQueryID;
      using (MySqlDataReader reader = ExecuteReader())
      {
        int[] vals = reader.GetIntResults();
        return vals[0];
      }
    }
  }
}
