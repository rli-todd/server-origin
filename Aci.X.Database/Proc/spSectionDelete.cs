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
  public class spSectionDelete : MyStoredProc
  {
    public spSectionDelete(DbConnection conn)
      : base(strProcName: "spSectionDelete", conn: conn)
    {
      Parameters.Add("@AuthorizedUserID", System.Data.SqlDbType.Int);
      Parameters.Add("@SectionID", System.Data.SqlDbType.Int);
    }

    public void Execute(int intAuthorizedUserID, int intSectionID)
    {
      Parameters["@AuthorizedUserID"].Value = intAuthorizedUserID;
      Parameters["@SectionID"].Value = intSectionID;
      base.ExecuteNonQuery();
    }
  }
}



