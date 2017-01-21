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
  public class spPageCreate : MyStoredProc
  {
    public spPageCreate(DbConnection conn)
      : base(strProcName: "spPageCreate", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@PageCode", System.Data.SqlDbType.VarChar);
      Parameters.Add("@Description", System.Data.SqlDbType.VarChar);
      Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
      Parameters["@ReturnValue"].Direction = System.Data.ParameterDirection.ReturnValue;
    }

    public int Execute(int intAuthorizedUserID, string strPageCode, string strDescription)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@PageCode"].Value = strPageCode;
      Parameters["@Description"].Value = strDescription;
      return (int)Parameters["@ReturnValue"].Value;
    }
  }
}



