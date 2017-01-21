using System;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBQuery : MySqlResult
  {
    public int QueryID;
    public string FirstName;
    public string MiddleInitial;
    public string LastName;
    public string State;

    public override void Read()
    {
      QueryID = Value<int>("ID");
      FirstName = Value<String>("FirstName");
      LastName = Value<String>("LastName");
      MiddleInitial = Value<String>("MiddleInitial");
      State = Value<string>("State");
    }
  }
}
