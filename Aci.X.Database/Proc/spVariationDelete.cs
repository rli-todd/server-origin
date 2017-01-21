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
  public class spVariationDelete : MyStoredProc
  {
    public spVariationDelete(DbConnection conn)
      : base(strProcName: "spVariationDelete", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@VariationID", System.Data.SqlDbType.Int);
    }

    public void Execute(int intAuthorizedUserID, int intVariationID)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@VariationID"].Value = intVariationID;
      base.ExecuteNonQuery();
    }
  }
}



