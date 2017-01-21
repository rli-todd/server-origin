using System.Linq;
using System.Data.Common;
using Solishine.CommonLib;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.Database
{
  public class spGeoCityGet : MyStoredProc
  {
    public spGeoCityGet(DbConnection conn)
      : base(strProcName: "spGeoCityGet", conn: conn)
    {
      Parameters.Add("@StateFips", System.Data.SqlDbType.TinyInt);
      Parameters.Add("@CountyFips", System.Data.SqlDbType.SmallInt);
    }
    public GeoCity[] Execute(byte bStateFips, short? shCountyFips=null)
    {
      Parameters["@StateFips"].Value = bStateFips;
      Parameters["@CountyFips"].Value = shCountyFips;
      using (MySqlDataReader reader = ExecuteReader())
      {
        DBGeoCity[] dbCities = reader.GetResults<DBGeoCity>();
        GeoCity[] retVal = (
          from c
            in dbCities
          select new GeoCity { StateFips = c.StateFips, CityFips = c.CityFips, CityName = c.CityName }).ToArray();
        return retVal;
      }
    }
  }
}



