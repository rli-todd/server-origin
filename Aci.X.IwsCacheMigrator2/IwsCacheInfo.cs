using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace IwsCacheMigrator2
{
  public class IwsCacheInfo : MySqlResult
  {
    public int QueryID;
    public DateTime? DateCached;
    public int? FileSize;
    public byte[] CompressedResults;

    public override void Read()
    {
      QueryID = Value<int>("QueryID");
      DateCached = Value<DateTime?>("DateCached");
      FileSize = Value<int?>("FileSize");
      CompressedResults= Value<byte[]>("CompressedResults");
    }
  }
}
