using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;

namespace Aci.X.Business.Cache
{
  public class SiteCache : EntityCacheBase<DBSite, byte>
  {
    public SiteCache()
      : base("Site")
    {
    }

    /*
     * Note that we will only ever expect a single key for this call,
     * and as a result, will only ever look at the first entry in the 
     * keys array
     */
    protected override DBSite[] GetFromDB(byte[] keys)
    {
      DBSite[] retVal = new DBSite[0];
      using (var db = new AciXDB())
      {
        retVal = db.spSiteGet(keys);
      }
      return retVal;
    }

    protected override byte GetKey(DBSite t)
    {
      return t.SiteID;
    }
  }
}
