using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;

namespace Aci.X.Business.Cache
{
  /// <summary>
  /// Maintains a cache of VisitGuids to a ulong "key" which consists of
  /// a SiteID and a UserID
  /// </summary>
  public class VisitCache : EntityCacheBase<DBVisit,string>
  {
    public VisitCache() : base("Visit")
    {
    }

    /*
     * Note that we will only ever expect a single key for this call,
     * and as a result, will only ever look at the first entry in the 
     * keys array
     */
    protected override DBVisit[] GetFromDB(string[] keys)
    {
      DBVisit[] retVal = new DBVisit[0];
      using (var db = new AciXDB())
      {
        DBVisit visit = db.spVisitGetByToken(keys[0]);
        if (visit != null)
        {
          retVal = new DBVisit[] {visit};
        }
      }
      return retVal;
    }

    protected override string GetKey(DBVisit t)
    {
      return t.VisitGuid.ToString();
    }
  }
}
