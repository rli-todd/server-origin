using System;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("AciXDB")]
  public class spVisitCreate : MyStoredProc
  {
    public spVisitCreate(DbConnection conn)
      : base(strProcName: "spVisitCreate", conn: conn)
    {
    }

    public DBVisit Execute(
      string strUserAgent,
      string strIpAddress,
      string strAcceptLanguage,
      string strRefererUrl,
      string strLandingUrl,
      string strWebServerName,
      string strApiServerName,
      Guid? userGuid=null,
      bool isReadOnly=false)
    {
      Parameters.Clear();
      Parameters.AddWithValue("@UserAgent", strUserAgent);
      Parameters.AddWithValue("@IpAddress", strIpAddress);
      Parameters.AddWithValue("@AcceptLanguage", strAcceptLanguage);
      Parameters.AddWithValue("@LandingUrl", strLandingUrl);
      Parameters.AddWithValue("@RefererUrl", strRefererUrl);
      Parameters.AddWithValue("@WebServerName", strWebServerName);
      Parameters.AddWithValue("@ApiServerName", strApiServerName);
      Parameters.AddWithValue("@UserGuid", userGuid);
      Parameters.AddWithValue("@ReadOnly", isReadOnly);
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBVisit[] tokens = reader.GetResults<DBVisit>();
        return tokens[0];
      }
    }
  }
}



