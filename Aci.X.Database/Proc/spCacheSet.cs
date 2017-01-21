using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Solishine.CommonLib;
using Aci.X.ClientLib;

namespace Aci.X.Database
{
  public class spCacheSet : MyStoredProc
  {
    public spCacheSet(DbConnection conn)
      : base(strProcName: "spCacheSet", conn: conn)
    {
      Parameters.Add("@Key", System.Data.SqlDbType.VarChar);
      Parameters.Add("@Value", System.Data.SqlDbType.VarChar);
    }

    public void Execute(string strKey, string strValue)
    {
      Parameters["@Key"].Value = strKey;
      Parameters["@Value"].Value = strValue;
      base.ExecuteNonQuery();
    }
  }
}



