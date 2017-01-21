using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spIwsCacheDeleteNull : MyStoredProc
  {
    public spIwsCacheDeleteNull(DbConnection conn)
      : base("spIwsCacheDeleteNull", conn)
    {
      Parameters.Clear();
      Parameters.Add("@QueryID", System.Data.SqlDbType.Int);
    }

    public void Execute(int intQueryID)
    {
      Parameters["@QueryID"].Value = intQueryID;
      base.ExecuteNonQuery();
    }
  }
}
