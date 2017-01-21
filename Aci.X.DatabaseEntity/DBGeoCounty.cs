using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBGeoCounty : MySqlResult
  {
    public byte StateFips;
    public short CountyFips;
    public string CountyName;

    public override void Read()
    {
      StateFips = Value<byte>("StateFips");
      CountyFips = Value<short>("CountyFips");
      CountyName = Value<string>("CountyName");
    }
  }
}
