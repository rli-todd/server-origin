using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class ReportTypeCache : SiteEntityCacheBase<DBReportType, string>
  {
    private static ConcurrentDictionary<byte, ReportTypeCache> _dictCaches = new ConcurrentDictionary<byte, ReportTypeCache>();

    public ReportTypeCache(byte tSiteID)
      : base(tSiteID, "ReportType")
    {
    }

    public static ReportTypeCache ForSite(byte tSiteID)
    {
      ReportTypeCache retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new ReportTypeCache(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }

    protected override DBReportType[] GetFromDB(string[] keys)
    {
      using (var db = new AciXDB())
      {
        return db.spReportTypeGet(_tSiteID, keys);
      }
    }

    protected override string GetKey(DBReportType t)
    {
      return t.TypeCode;
    }
  }
}
