using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class DBIwsCachedPreview : MySqlResult
  {
    public int QueryID;
    public byte[] CompressedResults;

    public override void Read()
    {
      QueryID= Value<int>("ID");
      CompressedResults = Value<byte[]>("CompressedResults");
    }
  }
}
