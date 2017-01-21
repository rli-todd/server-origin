using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBReportType : MySqlResult
  {
    public string TypeCode;
    public string Title;
    public string ProfileAttributes;

    public override void Read()
    {
      TypeCode= Value<string>("TypeCode");
      Title = Value<string>("Title");
      ProfileAttributes = Value<string>("ProfileAttributes");
    }
  }
}
