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
  public class spBlockUpdate : MyStoredProc
  {
    public spBlockUpdate(DbConnection conn)
      : base(strProcName: "spBlockUpdate", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@BlockID", System.Data.SqlDbType.Int);
      Parameters.Add("@BlockName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@BlockType", System.Data.SqlDbType.VarChar);
      Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit);
    }

    public void Execute(int intAuthorizedUserID, int intBlockID, string strBlockName, string strBlockType, bool isEnabled)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@BlockID"].Value = intBlockID;
      Parameters["@BlockName"].Value = strBlockName;
      Parameters["@BlockType"].Value = strBlockType;
      Parameters["@IsEnabled"].Value = isEnabled;
      base.ExecuteNonQuery();
    }
  }
}



