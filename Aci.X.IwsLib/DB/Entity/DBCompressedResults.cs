using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.IwsLib.DB
{
  public class DBCompressedResults : MySqlResult
  {
    public int SearchResultsID;
    public byte[] CompressedResults;

    public override void Read()
    {
      SearchResultsID= Value<int>("ID");
      CompressedResults = Value<byte[]>("CompressedResultsJson");
    }
  }
}
