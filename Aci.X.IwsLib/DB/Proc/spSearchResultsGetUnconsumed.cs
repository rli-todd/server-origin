using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class spSearchResultsGetUnconsumed: MyStoredProc
  {
    public spSearchResultsGetUnconsumed(DbConnection conn)
      : base("spSearchResultsGetUnconsumed", conn)
    {
      Parameters.Clear();
      Parameters.Add("@NumRows", System.Data.SqlDbType.Int);
    }

    public DBID[] Execute(int intNumRows=1000)
    {
      Parameters["@NumRows"].Value = intNumRows;
      
      using (MySqlDataReader reader = base.ExecuteReader())
      {
        return reader.GetResults<DBID>();
      }
    }
  }
}
