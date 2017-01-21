using System.Net;
using Aci.X.Business.Cache;
using Aci.X.ClientLib;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;
using Aci.X.IwsLib.Storefront;

namespace Aci.X.Business
{
  public class Visit
  {
    public static VisitCache Cache = new VisitCache();

    public static VisitToken Render(DBVisit dbToken)
    {
      return new VisitToken
      {
        Token = dbToken.VisitGuid,
        ClientIP = dbToken.ClientIP,
        UserGuid = dbToken.UserGuid,
        IsBlocked = dbToken.IsBlocked,
        CountryName = dbToken.CountryName,
        CityName = dbToken.CityName,
        StateName = dbToken.RegionName,
        CityFips = dbToken.CityFips,
        StateFips = dbToken.StateFips,
        Longitude = dbToken.Longitude,
        Latitude = dbToken.Latitude,
        GeoLocationID = dbToken.GeoLocationID
      };
    }

    public static DBVisit Get(string strVisitGuid)
    {
      return Cache.Get(strVisitGuid);
    }

    public static VisitToken CreateVisit(CallContext context, VisitParams visitParams)
    {
      using (var db = new AciXDB())
      {
        var dbVisit = db.spVisitCreate(
          strUserAgent: visitParams.UserAgent,
          strIpAddress: context.UserIP,
          strAcceptLanguage: visitParams.AcceptLanguage,
          strRefererUrl: visitParams.RefererUrl,
          strLandingUrl: visitParams.LandingUrl,
          strWebServerName: visitParams.WebServerName,
          strApiServerName: Dns.GetHostName(),
          userGuid: visitParams.UserGuid);
        context.DBVisit = dbVisit;
        Cache.Set(dbVisit);

        // Record the visit asynchronously
        new System.Threading.Tasks.Task(() => { RecordVisit(context); }).Start();
        // DEBUG
        var v = Cache.Get(dbVisit.VisitGuid.ToString());
        return Render(dbVisit);
      }
    }

    private static void RecordVisit(CallContext context)
    {
      if (context.DBVisit.RobotID == 0)
      {
        StorefrontClient sfCli = new StorefrontClient(context);
        sfCli.RecordVisit();
      }
    }

    public static void SetStateAndQueryID(CallContext context, string strState, int intQueryID)
    {
      context.DBVisit.CurrentQueryState = strState;
      context.DBVisit.CurrentQueryID = intQueryID;
      Cache.Set(context.DBVisit);
    }

    public static VisitToken Ping(CallContext context)
    {
      using (var db = new AciXDB())
      {
        var dbVisit = db.spVisitCreate(
          strUserAgent: null,
          strIpAddress: context.UserIP,
          strAcceptLanguage: null,
          strRefererUrl: null,
          strLandingUrl: null,
          strWebServerName: null,
          strApiServerName: Dns.GetHostName(),
          userGuid: null,
          isReadOnly: true);
        return Render(dbVisit);
      }
    }
  }
}
