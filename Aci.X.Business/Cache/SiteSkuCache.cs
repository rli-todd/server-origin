using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class SiteSkuCache : SiteEntityCacheBase<DBSku, int>
  {
    private static ConcurrentDictionary<byte, SiteSkuCache> _dictCaches = new ConcurrentDictionary<byte, SiteSkuCache>();
    private SiteSkuCache(byte tSiteID)
      : base(tSiteID, "SiteSku")
    { }

    public static SiteSkuCache ForSite(byte tSiteID)
    {
      SiteSkuCache retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new SiteSkuCache(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }


    protected override DBSku[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        return db.spSkuGet(_tSiteID, keys);
      }
    }

    protected override int GetKey(DBSku t)
    {
      return t.SkuID;
    }
  }
}
