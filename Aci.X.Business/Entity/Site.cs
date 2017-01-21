using Aci.X.Business.Cache;
using Aci.X.DatabaseEntity;

namespace Aci.X.Business
{
  public class Site
  {
    public static SiteCache Cache = new SiteCache();

    public static DBSite Get(byte tSiteID)
    {
      return Cache.Get(tSiteID);
    }
  }
}
