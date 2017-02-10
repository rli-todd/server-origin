using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class SiteCategoryCacheByExternalID : SiteEntityCacheBase<DBCategory, int>
  {
    private static ConcurrentDictionary<byte, SiteCategoryCacheByExternalID> _dictCaches = new ConcurrentDictionary<byte, SiteCategoryCacheByExternalID>();
    private SiteCategoryCacheByExternalID(byte tSiteID)
      : base(tSiteID, "SiteCategoryByExternalID")
    { }

    public static SiteCategoryCacheByExternalID ForSite(byte tSiteID)
    {
      SiteCategoryCacheByExternalID retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new SiteCategoryCacheByExternalID(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }


    protected override DBCategory[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        var ids = db.spCategorySearch(_tSiteID, intExternalCategoryIDs: keys);
        return db.spCategoryGet(_tSiteID, ids);
      }
    }

    protected override int GetKey(DBCategory t)
    {
      return t.ProductExternalID;
    }
  }
}
