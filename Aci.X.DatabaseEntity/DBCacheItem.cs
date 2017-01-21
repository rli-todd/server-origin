using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBCacheItem : MySqlResult
  {
    public byte[] HashKey;
    public string Value;
    public DateTime DateCreated;

    public override void Read()
    {
      HashKey = Value<byte[]>("HashKey");
      Value = Value<string>("Value");
      DateCreated = Value<DateTime>("DateCreated");
    }
  }
}
