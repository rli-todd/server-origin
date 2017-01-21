using System.Linq;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spGeoCountyGet : MyStoredProc
  {
    public spGeoCountyGet(DbConnection conn)
      : base(strProcName: "spGeoCountyGet", conn: conn)
    {
      Parameters.Add("@StateFips", System.Data.SqlDbType.TinyInt);
    }
    public GeoCounty[] Execute(byte bStateFips)
    {
      Parameters["@StateFips"].Value = bStateFips;
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBGeoCounty[] dbCounties = reader.GetResults<DBGeoCounty>();
        GeoCounty[] retVal = (
          from c
            in dbCounties
          select new GeoCounty { StateFips = c.StateFips, CountyFips = c.CountyFips, CountyName = c.CountyName }).ToArray();
        return retVal;
      }
    }
  }
}



