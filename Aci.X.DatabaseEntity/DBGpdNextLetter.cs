using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBGpdNextLetters : MySqlResult
  {
    public string NextLetters;

    public override void Read()
    {
      NextLetters = Value<string>("NextLetters");
    }
  }
}
