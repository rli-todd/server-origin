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
  public class spStatsUpdate : MyStoredProc
  {
    public spStatsUpdate(DbConnection conn)
      : base(strProcName: "spStatsUpdate", conn: conn)
    {
      Parameters.Add("@StatsXml", System.Data.SqlDbType.Xml);
    }

    public void Execute(string strStatsXml)
    {
      Parameters["@StatsXml"].Value = strStatsXml;
      ExecuteNonQuery();;
    }
  }
}



