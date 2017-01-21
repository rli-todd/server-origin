using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spOrderSearch : MyStoredProc
  {
    public spOrderSearch(DbConnection conn)
      : base(strProcName: "spOrderSearch", conn: conn)
    {
    }

    public int[] Execute(
      int intSiteID, 
      int intUserID=0, 
      int? intSelectedUserID=null,
      string strFirstName=null, 
      string strLastName=null,
      string strState=null,
      string strProfileID=null,
      int[] intExternalOrderIDs=null)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@AuthorizedUserID", intUserID);
      Parameters.AddWithValue("@SelectedUserID", intSelectedUserID);
      Parameters.AddWithValue("@FirstName", strFirstName);
      Parameters.AddWithValue("@LastName", strLastName);
      Parameters.AddWithValue("@State", strState);
      Parameters.AddWithValue("@ProfileID", strProfileID);
      if (intExternalOrderIDs != null)
      {
        Parameters.Add(new SqlParameter("@ExternalOrderIDs", SqlDbType.Structured)
        {
          TypeName = "dbo.ID_TABLE",
          Value = new IDTable(intExternalOrderIDs)
        });
      }
      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetIntResults();
      }
    }
  }
}



