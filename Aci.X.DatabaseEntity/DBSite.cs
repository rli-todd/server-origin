using Solishine.CommonLib;

namespace Aci.X.DatabaseEntity
{
  public class DBSite: MySqlResult
  {
    public byte SiteID;
    public string SiteName;
    public string SeoReferCode;
    public string SemReferCode;
    public string BaseUrl;

    public override void Read()
    {
      SiteID = Value<byte>("SiteID");
      SiteName = Value<string>("SiteName");
      SeoReferCode = Value<string>("SeoReferCode");
      SemReferCode = Value<string>("SemReferCode");
      BaseUrl = Value<string>("BaseUrl");
    }
  }
}
