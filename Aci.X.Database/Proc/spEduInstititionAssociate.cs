using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("GeoDB")]
  public class spEduInstitutionAssociate : MyStoredProc
  {
    public spEduInstitutionAssociate(DbConnection conn)
      : base(strProcName: "spEduInstitutionAssociate", conn: conn)
    {
    }
    public int Execute(int intInstutitionID, string strSearchTerm)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@InstitutionID", intInstutitionID);
      Parameters.AddWithValue("@SearchTerm", strSearchTerm);
      return ExecuteNonQuery();
    }
  }
}



