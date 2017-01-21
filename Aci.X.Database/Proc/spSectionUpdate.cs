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
  public class spSectionUpdate : MyStoredProc
  {
    public spSectionUpdate(DbConnection conn)
      : base(strProcName: "spSectionUpdate", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@SectionID", System.Data.SqlDbType.Int);
      Parameters.Add("@SectionName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@SectionType", System.Data.SqlDbType.VarChar);
      Parameters.Add("@IsEnabled", System.Data.SqlDbType.Bit);
    }

    public void Execute(int intAuthorizedUserID, int intSectionID, string strSectionName, string strSectionType, bool isEnabled)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@SectionID"].Value = intSectionID;
      Parameters["@SectionName"].Value = strSectionName;
      Parameters["@SectionType"].Value = strSectionType;
      Parameters["@IsEnabled"].Value = isEnabled;
      base.ExecuteNonQuery();
    }
  }
}



