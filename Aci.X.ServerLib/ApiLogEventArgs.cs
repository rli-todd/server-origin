using System;

namespace Aci.X.ServerLib
{
  public class ApiLogEventArgs : EventArgs
  {
    public int SiteID;
    public int VisitID;
    public int UserID;
    public int ClientIP;
    public int UserIP;
    public string Controller;
    public string RequestMethod;
    public int ApiLogPathID;
    public string PathAndQuery;
    public short ApiLogTemplateID;
    public string RequestTemplate;
    public int UserAgentID;
    public string UserAgent;
    public string RequestBody;
    public string ResponseJson;
    public int ResponseSize;
    public long DurationTicks;
    public short HttpStatusCode;
    public string ErrorType;
    public string ErrorMessage;
    public short ApiServerID;
    public string ServerHostname;
  }
}
