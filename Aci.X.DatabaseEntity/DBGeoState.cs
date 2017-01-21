using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBGeoState: MySqlResult
  {
    public byte StateFips;
    public string StateAbbr;
    public string StateName;

    public override void Read()
    {
      StateFips = Value<byte>("StateFips");
      StateAbbr = Value<string>("StateAbbr");
      StateName = Value<string>("StateName");
    }
  }
}
