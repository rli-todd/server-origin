using System.Data.Common;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spProductRefresh : MyStoredProc
  {
    public spProductRefresh(DbConnection conn)
      : base(strProcName: "spProductRefresh", conn: conn)
    {
    }

    public void Execute(int intSiteID, string strExternalProductsXml)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@ExternalProductsXml", strExternalProductsXml);
      base.ExecuteNonQuery();
    }
  }
}



