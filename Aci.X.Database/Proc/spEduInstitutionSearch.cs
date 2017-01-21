using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("GeoDB")]
  public class spEduInstitutionSearch: MyStoredProc
  {
    public spEduInstitutionSearch(DbConnection conn)
      : base(strProcName: "spEduInstitutionSearch", conn: conn)
    {
    }
    public Dictionary<string,List<DBEduInstitution>> Execute( string strSearchTerm, int intMaxResultsPerSchool=1)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@InstitutionNames", strSearchTerm);
      Parameters.AddWithValue("@MaxResultsPerSchool", intMaxResultsPerSchool);
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBEduInstitution[] results = reader.GetResults<DBEduInstitution>();
        Dictionary<string, List<DBEduInstitution>> dictRet = new Dictionary<string, List<DBEduInstitution>>();
        foreach (var i in results)
        {
          var key = i.SearchTerm.ToLower();
          if (!dictRet.ContainsKey(key))
          {
            dictRet[key] = new List<DBEduInstitution>();
          }
          dictRet[key].Add(i);
        }
        return dictRet;
      }
    }
  }
}



