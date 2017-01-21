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
  public class spIwsCacheGet : MyStoredProc
  {
    public spIwsCacheGet(DbConnection conn)
      : base(strProcName: "_spIwsCacheGet", conn: conn)
    {
      Parameters.Add("@QueryID", System.Data.SqlDbType.Int);
    }

    public IwsCacheInfo Execute( int intQueryID)    
    {
      Parameters["@QueryID"].Value = intQueryID;
      using (MySqlDataReader reader = ExecuteReader())
      {
        IwsCacheInfo[] rows = reader.GetResults<IwsCacheInfo>();
        return rows.Length > 0 ? rows[0] : null;
      }
    }
  }
}
