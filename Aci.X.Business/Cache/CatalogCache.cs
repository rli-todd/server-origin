using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;

namespace Aci.X.Business.Cache
{
  public class CatalogCache : EntityCacheBase<DBCatalog, int>
  {
    public static CatalogCache Singleton = new CatalogCache();

    private CatalogCache()
      : base("Catalog")
    { }

    protected override DBCatalog[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        return new DBCatalog[] {db.spCatalogGet(keys[0])};
      }
    }

    protected override int GetKey(DBCatalog t)
    {
      return t.SiteID;
    }
  }
}
