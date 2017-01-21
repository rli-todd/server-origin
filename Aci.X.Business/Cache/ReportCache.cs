using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class SiteReportCache : SiteEntityCacheBase<DBReport, int>
  {
    private static ConcurrentDictionary<byte, SiteReportCache> _dictCaches = new ConcurrentDictionary<byte, SiteReportCache>();

    public SiteReportCache(byte tSiteID)
      : base(tSiteID, "Report")
    {
    }

    public static SiteReportCache ForSite(byte tSiteID)
    {
      SiteReportCache retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new SiteReportCache(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }

    protected override DBReport[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        return db.spReportGet(_tSiteID, keys);
      }
    }

    protected override int GetKey(DBReport t)
    {
      return t.ReportID;
    }
  }
}
