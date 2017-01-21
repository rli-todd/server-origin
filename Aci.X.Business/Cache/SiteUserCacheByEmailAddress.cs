using System.Collections.Concurrent;
using Aci.X.Database;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business.Cache
{
  public class SiteUserCacheByEmailAddress : SiteEntityCacheBase<DBUser, string>
  {
    private static ConcurrentDictionary<byte, SiteUserCacheByEmailAddress> _dictCaches = new ConcurrentDictionary<byte, SiteUserCacheByEmailAddress>();

    public SiteUserCacheByEmailAddress(byte tSiteID)
      : base(tSiteID, "UserByExternalID")
    {
    }

    public static SiteUserCacheByEmailAddress ForSite(byte tSiteID)
    {
      SiteUserCacheByEmailAddress retVal = null;
      if (!_dictCaches.TryGetValue(tSiteID, out retVal))
      {
        retVal = new SiteUserCacheByEmailAddress(tSiteID);
        _dictCaches[tSiteID] = retVal;
      }
      return retVal;
    }

    protected override DBUser[] GetFromDB(string[] keys)
    {
      using (var db = new AciXDB())
      {
        /*
         * NOTE: we only use the first key in the passed array
         */
        var id = db.spUserSearch(_tSiteID, strEmailAddress: keys[0]);
        return db.spUserGet(_tSiteID, new int[] { id });
      }
    }

    protected override string GetKey(DBUser t)
    {
      return t.EmailAddress;
    }
  }
}
