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
  public class spBlockCreate : MyStoredProc
  {
    public spBlockCreate(DbConnection conn)
      : base(strProcName: "spBlockCreate", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@BlockName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@BlockType", System.Data.SqlDbType.VarChar);
      Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit);
      Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
      Parameters["@ReturnValue"].Direction = System.Data.ParameterDirection.ReturnValue;
    }

    public int Execute(int intAuthorizedUserID, string strBlockName, string strBlockType, bool isEnabled)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@BlockName"].Value = strBlockName;
      Parameters["@BlockType"].Value = strBlockType;
      Parameters["@IsEnabled"].Value = isEnabled;
      base.ExecuteNonQuery();
      return (int)Parameters["@ReturnValue"].Value;
    }
  }
}



