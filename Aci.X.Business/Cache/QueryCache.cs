using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;

namespace Aci.X.Business.Cache
{
  public class QueryCache : EntityCacheBase<DBQuery, int>
  {
    public static QueryCache Singleton = new QueryCache();

    public QueryCache()
      : base("Query")
    {
    }

    /*
     * Note that we will only ever expect a single key for this call,
     * and as a result, will only ever look at the first entry in the 
     * keys array
     */
    protected override DBQuery[] GetFromDB(int[] keys)
    {
      using (var db = new AciXDB())
      {
        return db.spQueryGet(keys[0]);
      }
    }

    protected override int GetKey(DBQuery t)
    {
      return t.QueryID;
    }
  }
}
