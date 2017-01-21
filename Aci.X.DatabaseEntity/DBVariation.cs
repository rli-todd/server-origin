using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBVariation : MySqlResult
  {
    public int VariationID;
    public int SectionID;
    public string Description;
    public string MultirowPrefix;
    public string MultirowSuffix;
    public string MultirowDelimiter;
    public string HeaderTemplate;
    public string BodyTemplate;
    public string ViewName;
    public string ViewFieldNames;
    public bool IsEnabled;

    public override void Read()
    {
      VariationID = Value<int>("ID");
      SectionID = Value<int>("SectionID");
      Description = Value<string>("Description");
      MultirowPrefix = Value<string>("MultirowPrefix");
      MultirowSuffix = Value<string>("MultirowSuffix");
      MultirowDelimiter = Value<string>("MultirowDelimiter");
      HeaderTemplate = Value<string>("HeaderTemplate");
      BodyTemplate = Value<string>("BodyTemplate");
      ViewName = Value<string>("ViewName");
      ViewFieldNames = Value<string>("ViewFieldNames");
      IsEnabled = Value<bool>("IsEnabled");
    }
  }
}
