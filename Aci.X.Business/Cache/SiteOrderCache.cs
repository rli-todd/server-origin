using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class SiteOrderCache : SiteEntityCacheBase<DBOrder, int>
  {
    private static ConcurrentDictionary<byte, SiteOrderCache> _dictCaches = new ConcurrentDictionary<byte, SiteOrderCache>();

    public SiteOrderCache(byte tSiteID) : base(tSiteID, "Order")
    {
    }

    public static SiteOrderCache ForSite(byte tSiteID)
    {
      SiteOrderCache retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new SiteOrderCache(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }

    protected override DBOrder[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        return db.spOrderGet(_tSiteID, keys);
      }
    }

    protected override int GetKey(DBOrder t)
    {
      return t.OrderID;
    }
  }
}
