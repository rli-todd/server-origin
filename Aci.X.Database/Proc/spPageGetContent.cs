using System.Linq;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spPageGetContent : MyStoredProc
  {
    public spPageGetContent(DbConnection conn)
      : base(strProcName: "spPageGetContent", conn: conn)
    {
      Parameters.Add("@PageCode", System.Data.SqlDbType.VarChar);
      Parameters.Add("@WhereClause", System.Data.SqlDbType.VarChar);
    }
    public DBSectionTemplate[] Execute(string strPageCode, string strWhereClause)
    {
      Parameters["@PageCode"].Value = strPageCode;
      Parameters["@WhereClause"].Value = strWhereClause;
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBSectionTemplateDictionary dictTemplates = new DBSectionTemplateDictionary(reader.GetResults<DBSectionTemplate>());
        DBSectionValues[] sectionValues = null;
        for (int idx=0; idx<dictTemplates.Count; ++idx)
        {
          sectionValues = reader.GetResults<DBSectionValues>();
          if (sectionValues != null && sectionValues.Length>0)
          {
            dictTemplates[sectionValues[0].SectionID].ValueRows = sectionValues;
          }
        }
        return dictTemplates.Values.ToArray();
      }
    }
  }
}

