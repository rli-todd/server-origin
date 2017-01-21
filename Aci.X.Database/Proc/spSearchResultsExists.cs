using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("ProfileDB")]
  public class spSearchResultsExists : MyStoredProc
  {
    public spSearchResultsExists(DbConnection conn)
      : base(strProcName: "spSearchResultsExists", conn: conn)
    {
    }

    public bool Execute(
      short shSearchType,
      string strFirstName,
      string strMiddleName,
      string strLastName,
      string strState)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SearchType",shSearchType);
      Parameters.AddWithValue("@FirstName",strFirstName);
      Parameters.AddWithValue("@MiddleName",strMiddleName);
      Parameters.AddWithValue("@LastName",strLastName);
      Parameters.AddWithValue("@State",strState);
      Parameters.Add(new SqlParameter("@ReturnValue", SqlDbType.Int)
      {
        Direction = ParameterDirection.ReturnValue
      });

      using (MySqlDataReader reader = ExecuteReader())
      {
        return ((int)Parameters["@ReturnValue"].Value) != 0;
      }
    }
  }
}
