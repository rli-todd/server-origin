using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class DBID: MySqlResult
  {
    public int ID;

    public override void Read()
    {
      ID= Value<int>("ID");
    }
  }
}
