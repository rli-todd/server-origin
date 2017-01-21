using System;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBProfile : MySqlResult
  {
    public string ProfileID;
    public string ProfileAttributes;
    public byte[] CompressedJson;
    public DateTime DateCached;

    public override void Read()
    {
      ProfileID = Value<string>("ProfileID");
      ProfileAttributes = Value<string>("ProfileAttributes");
      CompressedJson = Value<byte[]>("CompressedJson");
      DateCached = Value<DateTime>("DateCached");
    }
  }
}
