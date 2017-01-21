using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class SiteOrderCacheByExternalID : SiteEntityCacheBase<DBOrder, int>
  {
    private static ConcurrentDictionary<byte, SiteOrderCacheByExternalID> _dictCaches = new ConcurrentDictionary<byte, SiteOrderCacheByExternalID>();

    public SiteOrderCacheByExternalID(byte tSiteID)
      : base(tSiteID, "OrderByExternalID")
    {
    }

    public static SiteOrderCacheByExternalID ForSite(byte tSiteID)
    {
      SiteOrderCacheByExternalID retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new SiteOrderCacheByExternalID(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }

    protected override DBOrder[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        var ids = db.spOrderSearch(_tSiteID, intExternalOrderIDs: keys);
        return db.spOrderGet(_tSiteID, ids);
      }
    }

    protected override int GetKey(DBOrder t)
    {
      return t.OrderExternalID;
    }
  }
}
