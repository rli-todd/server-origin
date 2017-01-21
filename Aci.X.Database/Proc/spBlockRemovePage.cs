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
  public class spBlockRemovePage : MyStoredProc
  {
    public spBlockRemovePage(DbConnection conn)
      : base(strProcName: "spBlockRemovePage", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@BlockID", System.Data.SqlDbType.Int);
      Parameters.Add("@PageID", System.Data.SqlDbType.Int);
    }

    public void Execute(int intAuthorizedUserID, int intPageID, int intBlockID)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@BlockID"].Value = intBlockID;
      Parameters["@PageID"].Value = intPageID;
      base.ExecuteNonQuery();
    }
  }
}



