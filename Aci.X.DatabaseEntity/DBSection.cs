using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBSection : MySqlResult
  {
    public int SectionID;
    public int BlockID;
    public string SectionName;
    public string SectionType;
    public bool IsEnabled;

    public override void Read()
    {
      SectionID = Value<int>("ID");
      BlockID = Value<int>("BlockID");
      SectionName = Value<string>("SectionName");
      SectionType = Value<string>("SectionType");
      IsEnabled = Value<bool>("IsEnabled");
    }
  }
}
