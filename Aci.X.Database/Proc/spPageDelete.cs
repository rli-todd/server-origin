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
  public class spPageDelete : MyStoredProc
  {
    public spPageDelete(DbConnection conn)
      : base(strProcName: "spPageDelete", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@PageID", System.Data.SqlDbType.Int);
    }

    public void Execute(int intAuthorizedUserID, int intPageID)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@PageID"].Value = intPageID;
      base.ExecuteNonQuery();
    }
  }
}



