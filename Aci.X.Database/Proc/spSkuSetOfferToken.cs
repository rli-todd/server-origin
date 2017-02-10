using System.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using Aci.X.DatabaseEntity;
using Solishine.CommonLib;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spSkuSetOfferToken : MyStoredProc
  {
    public spSkuSetOfferToken(DbConnection conn)
      : base(strProcName: "spSkuSetOfferToken", conn: conn)
    {
    }

    public void Execute(int intSiteID, int intSkuID, string strOfferToken)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@SiteID", intSiteID);
      Parameters.AddWithValue("@SkuID", intSkuID);
      Parameters.AddWithValue("@OfferToken", strOfferToken);
      ExecuteNonQuery();
    }
  }
}



