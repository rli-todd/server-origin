using System.Collections.Generic;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBProduct : MySqlResult
  {
    public int ProductID;
    public string ProductName;
    public bool IsActive;
    public string ProductCode;
    public bool RequireQueryID;
    public bool RequireState;
    public bool RequireProfileID;
    public string ProductType; 
    public int ProductExternalID;
    public string ReportTypeCode;

    public DBSku[] Skus; // Added by spProductGet

    public override void Read()
    {
      ProductID = Value<int>("ID");
      ProductName = Value<string>("ProductName");
      IsActive = Value<bool>("IsActive");
      ProductCode = Value<string>("ProductCode");
      RequireQueryID = Value<bool>("RequireQueryID");
      RequireState = Value<bool>("RequireState");
      RequireProfileID = Value<bool>("RequireProfileID");
      ProductType = Value<string>("ProductType");
      ProductExternalID = Value<int>("ProductExternalID");
      ReportTypeCode = Value<string>("ReportTypeCode");
    }
  }
}
