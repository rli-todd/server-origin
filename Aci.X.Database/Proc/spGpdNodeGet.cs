using System.Linq;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spGpdNodeGet : MyStoredProc
  {
    public spGpdNodeGet(DbConnection conn)
      : base(strProcName: "spGpdNodeGet", conn: conn)
    {
      Parameters.Add("@StateFips", System.Data.SqlDbType.TinyInt);
      Parameters.Add("@CityFips", System.Data.SqlDbType.Int);
      Parameters.Add("@StateName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@CityName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@LastName", System.Data.SqlDbType.VarChar);
      Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar);
    }

    public GpdNode Execute(
      byte? bStateFips=null,
      int? intCityFips=null,
      string strStateName=null,
      string strCityName=null,
      string strLastName=null,
      string strFirstName=null)
    {
      Parameters["@StateFips"].Value = bStateFips;
      Parameters["@CityFips"].Value = intCityFips;
      Parameters["@StateName"].Value = strStateName;
      Parameters["@CityName"].Value = strCityName;
      Parameters["@LastName"].Value = strLastName;
      Parameters["@FirstName"].Value = strFirstName;

      using (MySqlDataReader reader = ExecuteReader())
      {
        string strNextLetters = null;
        DBGpdTop100[] top100 = reader.GetResults<DBGpdTop100>();
        DBGpdNextLetters[] nextLetters = reader.GetResults<DBGpdNextLetters>();
        if (nextLetters != null && nextLetters.Length>0 && nextLetters[0] != null)
        {
          strNextLetters = nextLetters[0].NextLetters;
        }
        GpdNode retVal = new GpdNode
        {
          NextLetters = strNextLetters,
          Top100 = (from n in top100 select n.Name).ToArray()
        };
        return retVal;
      }
    }
  }
}



