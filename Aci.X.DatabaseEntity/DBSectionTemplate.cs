using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBSectionTemplate : MySqlResult
  {
    public string BlockName;
    public string BlockType;
    public string SectionName;
    public string SectionType;
    public int SectionID;
    public int VariationID;
    public int VariationIndex;
    public string Description;
    public string MultirowPrefix;
    public string MultirowSuffix;
    public string MultirowDelimiter;
    public string HeaderTemplate;
    public string HeaderDefault;
    public string BodyTemplate;
    public string BodyDefault;
    public string FieldNames;

    public DBSectionValues[] ValueRows;

    public override void Read()
    {
      BlockName = Value<string>("BlockName");
      BlockType = Value<string>("BlockType");
      SectionName = Value<string>("SectionName");
      SectionType = Value<string>("SectionType");
      Description = Value<String>("Description");
      MultirowPrefix = Value<string>("MultirowPrefix");
      MultirowSuffix = Value<string>("MultirowSuffix");
      MultirowDelimiter = Value<string>("MultirowDelimiter");
      HeaderTemplate = Value<string>("HeaderTemplate");
      BodyTemplate = Value<string>("BodyTemplate");
      HeaderDefault = Value<string>("HeaderDefault");
      BodyDefault = Value<string>("BodyDefault");
      FieldNames = Value<string>("FieldNames");
      SectionID = Value<int>("SectionID");
      VariationID = Value<int>("VariationID");
      VariationIndex = Value<int>("VariationIndex");
    }

  }
    public class DBSectionTemplateDictionary : Dictionary<int,DBSectionTemplate>
    {
      public DBSectionTemplateDictionary(DBSectionTemplate[] templates) : base( templates.ToDictionary( t=>t.SectionID ) )
      {
      }
    }

}
