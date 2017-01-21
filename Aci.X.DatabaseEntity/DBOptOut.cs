using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBOptOut : MySqlResult
  {
    public string FirstName;
    public string LastName;
    public string State;
    public DateTime OptOutDate;
    public DateTime RefreshDate;
    public bool NeedsRefresh;

    public override void Read()
    {
      FirstName = Value<string>("FirstName");
      LastName = Value<string>("LastName");
      State = Value<string>("State");
      OptOutDate = Value<DateTime>("OptOutDate");
      RefreshDate = Value<DateTime?>("RefreshDate") ?? new DateTime(0);
      NeedsRefresh = Value<bool>("NeedsRefresh");
    }
  }
}
