using System;
using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBReport : MySqlResult
  {
    public int SiteID;
    public int ReportID;
    public int UserID;
    public int OrderID;
    public int OrderExternalID;
    public int QueryID;
    public string ProfileID;
    public string ReportTypeCode;
    public string Title;
    public string FirstName;
    public string MiddleInitial;
    public string LastName;
    public string State;
    public byte[] CompressedJson;
    public byte[] CompressedHtml;
    public DateTime ReportDate;

    public override void Read()
    {
      SiteID = Value<byte>("SiteID");
      ReportID = Value<int>("ID");
      UserID = Value<int>("UserID");
      OrderID = Value<int>("OrderID");
      OrderExternalID = Value<int>("OrderExternalID");
      QueryID = Value<int>("QueryID");
      ProfileID = Value<string>("ProfileID");
      ReportTypeCode = Value<string>("ReportTypeCode");
      Title = Value<string>("Title");
      FirstName = Value<string>("FirstName");
      MiddleInitial = Value<string>("MiddleInitial");
      LastName = Value<string>("LastName");
      State = Value<string>("State");
      CompressedJson = Value<byte[]>("CompressedJson");
      CompressedHtml = Value<byte[]>("CompressedHtml");
      ReportDate = Value<DateTime>("ReportDate");
    }
  }
}

