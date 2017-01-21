using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class SiteSkuCacheByExternalID : SiteEntityCacheBase<DBSku, int>
  {
    private static ConcurrentDictionary<byte, SiteSkuCacheByExternalID> _dictCaches = new ConcurrentDictionary<byte, SiteSkuCacheByExternalID>();
    private SiteSkuCacheByExternalID(byte tSiteID)
      : base(tSiteID, "SiteSkuByExternalID")
    { }

    public static SiteSkuCacheByExternalID ForSite(byte tSiteID)
    {
      SiteSkuCacheByExternalID retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new SiteSkuCacheByExternalID(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }


    protected override DBSku[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        var ids = db.spSkuSearch(_tSiteID, intExternalSkuIDs: keys);
        return db.spSkuGet(_tSiteID, ids);
      }
    }

    protected override int GetKey(DBSku t)
    {
      return t.ProductExternalID;
    }
  }
}
