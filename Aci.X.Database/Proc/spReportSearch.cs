using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spReportSearch : MyStoredProc
  {
    public spReportSearch(DbConnection conn)
      : base(strProcName: "spReportSearch", conn: conn)
    {
    }

    public int[] Execute(
      int intSiteID,
      int intUserID,
      string strFirstName = null,
      string strLastName = null,
      string strState = null,
      string strProfileID = null,
      int? intOrderID = null)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@AuthorizedUserID", intUserID);
      Parameters.AddWithValue("@FirstName", strFirstName);
      Parameters.AddWithValue("@LastName", strLastName);
      Parameters.AddWithValue("@State", strState);
      Parameters.AddWithValue("@ProfileID", strProfileID);
      Parameters.AddWithValue("@OrderID",intOrderID);

      using (MySqlDataReader reader = ExecuteReader())
      {
        return reader.GetIntResults();
      }
    }
  }
}



