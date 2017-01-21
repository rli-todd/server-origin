using System.Linq;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  [MySpGroup("GeoDB")]
  public class spGeoStateGet : MyStoredProc
  {
    public spGeoStateGet(DbConnection conn)
      : base(strProcName: "spGeoStateGet", conn: conn)
    {
    }
    public GeoState[] Execute()
    {
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBGeoState[] dbStates = reader.GetResults<DBGeoState>();
        GeoState[] retVal = (
          from s
            in dbStates
          select new GeoState { StateFips = s.StateFips, StateAbbr = s.StateAbbr, StateName = s.StateName }).ToArray();
        return retVal;
      }
    }
  }
}



