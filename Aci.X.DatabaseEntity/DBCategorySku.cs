using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBCategorySku : MySqlResult
  {
    public int CategoryID;
    public int SkuID;

    public override void Read()
    {
      CategoryID = Value<int>("CategoryID");
      SkuID = Value<int>("SkuID");
    }
  }
}
