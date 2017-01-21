using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBGeoCity : MySqlResult
  {
    public byte StateFips;
    public int CityFips;
    public string CityName;

    public override void Read()
    {
      StateFips = Value<byte>("StateFips");
      CityFips = Value<int>("CityFips");
      CityName = Value<string>("CityName");
    }
  }
}
