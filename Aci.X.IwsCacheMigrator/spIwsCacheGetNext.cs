using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace IwsCacheMigrator
{
  public class spIwsCacheGetNext : MyStoredProc
  {
    public spIwsCacheGetNext(DbConnection conn)
      : base(strProcName: "_spIwsCacheGetNext", conn: conn)
    {
      Parameters.Add("@ModArgument", System.Data.SqlDbType.Int);
      Parameters.Add("@ModResult", System.Data.SqlDbType.Int);
      Parameters.Add("@PreviousQueryID", System.Data.SqlDbType.Int);
    }

    public IwsCacheInfo Execute( int? intModArgument=null, int? intModResult=null, int? intPreviousQueryID=null)    
    {
      Parameters["@ModArgument"].Value = intModArgument;
      Parameters["@ModResult"].Value = intModResult;
      Parameters["@PreviousQueryID"].Value = intPreviousQueryID;
      using (MySqlDataReader reader = ExecuteReader())
      {
        IwsCacheInfo[] rows = reader.GetResults<IwsCacheInfo>();
        return rows.Length > 0 ? rows[0] : null;
      }
    }
  }
}
