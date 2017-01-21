using System.Linq;
using System.Collections.Generic;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("GeoDB")]
  public class spEduInstitutionGet : MyStoredProc
  {
    public spEduInstitutionGet(DbConnection conn)
      : base(strProcName: "spEduInstitutionGet", conn: conn)
    {
    }
    public DBEduInstitution Execute(int intInstutitionID)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@InstitutionID", intInstutitionID);
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBEduInstitution results = reader.GetResults<DBEduInstitution>().First();
        var dictCampuses = reader.GetResults<DBEduCampus>().ToDictionary(n => n.CampusID);
        DBEduAccreditation[] accreditations = reader.GetResults<DBEduAccreditation>();
        foreach (var accred in accreditations)
        {
          var campus = dictCampuses[accred.CampusID];
          if (campus.Accreditations == null)
          {
            campus.Accreditations = new List<DBEduAccreditation>();
          }
          campus.Accreditations.Add(accred);
        }
        results.Campuses = dictCampuses.Values.OrderBy(n => n.CampusID).ToList();
        return results;
      }
    }
  }
}



