using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBGpdTop100: MySqlResult
  {
    public string Name;
    public int PersonCount;

    public override void Read()
    {
      Name = Value<string>("Name");
      PersonCount = Value<int?>("PersonCount") ?? 0;
    }
  }
}
