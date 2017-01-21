using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spCartUpdate : MyStoredProc
  {
    public spCartUpdate(DbConnection conn)
      : base(strProcName: "spCartUpdate", conn: conn)
    {
    }

    public void Execute(
      int intSiteID,
      int intVisitID,
      int? intOrderID=null,
      int? intProductID=null,
      int? intQueryID=null,
      string strProfileID=null,
      string strStateAbbr=null)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@VisitID", intVisitID);
      Parameters.AddWithValue("@OrderID", intOrderID);
      Parameters.AddWithValue("@ProductID", intProductID);
      Parameters.AddWithValue("@QueryID", intQueryID);
      Parameters.AddWithValue("@ProfileID", strProfileID);
      Parameters.AddWithValue("@State", strStateAbbr);

      ExecuteNonQuery();
    }
  }
}



