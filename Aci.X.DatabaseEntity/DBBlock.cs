using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBBlock: MySqlResult
  {
    public int BlockID;
    public string BlockName;
    public string BlockType;
    public bool IsEnabled;

    public override void Read()
    {
      BlockID = Value<int>("ID");
      BlockName = Value<string>("BlockName");
      BlockType = Value<string>("BlockType");
      IsEnabled = Value<bool>("IsEnabled");
    }
  }
}
