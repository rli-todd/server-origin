using System.Collections.Generic;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBCategory : MySqlResult
  {
    public int CategoryID;
    public int ExternalID;
    public string CategoryName;
    public string CategoryCode;

    // Added by spCategoryGet
    public DBSku[] Skus;

    public override void Read()
    {
      CategoryID = Value<int>("ID");
      ExternalID = Value<int>("ExternalID");
      CategoryName = Value<string>("CategoryName");
      CategoryCode = Value<string>("CategoryCode");
    }
  }
}
