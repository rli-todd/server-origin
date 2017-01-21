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
  public class spPageUpdate : MyStoredProc
  {
    public spPageUpdate(DbConnection conn)
      : base(strProcName: "spPageUpdate", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@PageID", System.Data.SqlDbType.Int);
      Parameters.Add("@PageCode", System.Data.SqlDbType.VarChar);
      Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
    }

    public void Execute(int intAuthorizedUserID, int intPageID, string strPageCode, string strDescription)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@PageID"].Value = intPageID;
      Parameters["@PageCode"].Value = strPageCode;
      Parameters["@Description"].Value = strDescription;
      base.ExecuteNonQuery();
    }
  }
}



