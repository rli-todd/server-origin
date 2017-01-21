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
  public class spSectionCreate : MyStoredProc
  {
    public spSectionCreate(DbConnection conn)
      : base(strProcName: "spSectionCreate", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@BlockID", System.Data.SqlDbType.Int);
      Parameters.Add("@SectionName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@SectionType", System.Data.SqlDbType.VarChar);
      Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit);
      Parameters.Add("@ReturnValue", System.Data.SqlDbType.Int);
      Parameters["@ReturnValue"].Direction = System.Data.ParameterDirection.ReturnValue;
    }

    public int Execute(int intAuthorizedUserID, int intBlockID, string strSectionName, string strSectionType, bool isEnabled)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@BlockID"].Value = intBlockID;
      Parameters["@SectionName"].Value = strSectionName;
      Parameters["@SectionType"].Value = strSectionType;
      Parameters["@IsEnabled"].Value = isEnabled;
      base.ExecuteNonQuery();
      return (int)Parameters["@ReturnValue"].Value;
    }
  }
}



