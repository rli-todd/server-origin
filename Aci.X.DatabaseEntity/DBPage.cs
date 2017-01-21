using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBPage: MySqlResult
  {
    public int PageID;
    public string PageCode;
    public string Description;

    public override void Read()
    {
      PageID = Value<int>("ID");
      PageCode= Value<string>("PageCode");
      Description = Value<string>("Description");
    }
  }
}
