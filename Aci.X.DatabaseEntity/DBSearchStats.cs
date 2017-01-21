using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBSearchStats: MySqlResult
  {
    public string Period;
    public int LastNameSearches;
    public int LastNameRank;
    public int FirstNameSearches;
    public int FirstNameRank;
    public int FullNameSearches;
    public int FullNameRank;
    public int StateSearches;
    public int StateRank;

    public override void Read()
    {
      Period = Value<string>("period");
      LastNameSearches = Value<int?>("last_name_searches")??0;
      LastNameRank = Value<int?>("last_name_rank")??0;
      FirstNameSearches = Value<int?>("first_name_searches")??0;
      FirstNameRank = Value<int?>("first_name_rank")??0;
      FullNameSearches = Value<int?>("full_name_searches")??0;
      FullNameRank = Value<int?>("full_name_rank")??0;
      StateSearches = Value<int?>("state_searches")??0;
      StateRank = Value<int?>("state_rank")??0;
    }
  }
}
