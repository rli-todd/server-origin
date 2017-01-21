using System;
using System.Collections.Generic;
using System.Linq;
using Aci.X.ClientLib;
using Aci.X.Database;
using Aci.X.ServerLib;

namespace Aci.X.Business.Cache
{
  public class GeneralPurposeCache : CacheClient
  {
    public static GeneralPurposeCache Singleton = new GeneralPurposeCache();

    public GeneralPurposeCache()
      : base(strRegion: "GeneralPurpose")
    {
    }

    private const string LOG_UNAUTHENTICATED_CALLS_KEY = "LogUnauthenticatedCalls";

    public bool LogUnauthenticatedCalls
    {
      get{ return !String.IsNullOrEmpty(Get(LOG_UNAUTHENTICATED_CALLS_KEY)); }
      set
      {
        if (value)
          Put(LOG_UNAUTHENTICATED_CALLS_KEY, 1);
        else
          Remove(LOG_UNAUTHENTICATED_CALLS_KEY);
      }
    }

    private Dictionary<string, GeoState> StateDict
    {
      get
      {
        var dict = Get<Dictionary<string,GeoState>>("GeoStateDict");
        if (dict == null)
        {
          using (var db = new GeoDB())
          {
            var states = db.spGeoStateGet();
            dict = states.ToDictionary(n => n.StateAbbr.ToUpper());
            StateDict = dict;
          }
        }
        return dict;
      }
      set { Put("GeoStateDict", value); }
    }
    public GeoState StateByAbbr(string strStateAbbr)
    {
      return StateDict[strStateAbbr.ToUpper()];
    }
  }
}
