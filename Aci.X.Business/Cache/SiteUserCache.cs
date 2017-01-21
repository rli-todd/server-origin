using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class SiteUserCache : SiteEntityCacheBase<DBUser,int>
  {
    private static ConcurrentDictionary<byte, SiteUserCache> _dictCaches = new ConcurrentDictionary<byte, SiteUserCache>();
    private SiteUserCache(byte tSiteID) : base(tSiteID, "SiteUser")
    { }

    public static SiteUserCache ForSite(byte tSiteID)
    {
      SiteUserCache retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new SiteUserCache(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }


    protected override DBUser[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        return db.spUserGet(_tSiteID, keys);
      }
    }

    protected override int GetKey(DBUser t)
    {
      return t.UserID;
    }
  }
}
