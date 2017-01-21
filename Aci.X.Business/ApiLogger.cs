using System;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Net;
using Aci.X.Database;
using Aci.X.ServerLib;

namespace Aci.X.Business
{
  public class ApiLogger
  {
    private static ConcurrentDictionary<string, short> _dictApiLogTemplate = new ConcurrentDictionary<string, short>();
    private static ConcurrentDictionary<string, int> _dictUserAgent = new ConcurrentDictionary<string, int>();
    private static ConcurrentDictionary<string, int> _dictApiLogPath = new ConcurrentDictionary<string, int>();
    private static ConcurrentDictionary<string, short> _dictApiServer = new ConcurrentDictionary<string, short>();

    public static void CallContext_ApiLogged(object sender, ApiLogEventArgs logEventArgs)
    {
      //if (logEventArgs.HttpStatusCode<400 && logEventArgs.UserID == 0 && !Cache.GeneralPurposeCache.Singleton.LogUnauthenticatedCalls)
      //  return;
      /*
        * The following should be temporary until we determine how we are getting null values
        */
      if (logEventArgs.UserAgent == null)
        logEventArgs.UserAgent = "NULL";
      if (logEventArgs.PathAndQuery == null)
        logEventArgs.PathAndQuery = "NULL";
      if (logEventArgs.RequestTemplate == null)
        logEventArgs.RequestTemplate = "NULL";
      if (logEventArgs.ServerHostname == null)
        logEventArgs.ServerHostname = "NULL";

      if (_dictUserAgent.TryGetValue(logEventArgs.UserAgent, out logEventArgs.UserAgentID))
        logEventArgs.UserAgent = null;
      if (_dictApiLogPath.TryGetValue(logEventArgs.PathAndQuery, out logEventArgs.ApiLogPathID))
        logEventArgs.PathAndQuery = null;
      if (_dictApiLogTemplate.TryGetValue(logEventArgs.RequestTemplate, out logEventArgs.ApiLogTemplateID))
        logEventArgs.RequestTemplate = null;
      if (_dictApiServer.TryGetValue(logEventArgs.ServerHostname, out logEventArgs.ApiServerID))
        logEventArgs.ServerHostname = null;

      try
      {
        using (var db = new AciXDB())
        {
          db.spApiLog(
            intSiteID: logEventArgs.SiteID,
            intVisitID: logEventArgs.VisitID,
            intUserID: logEventArgs.UserID,
            intClientIP: logEventArgs.ClientIP,
            intUserIP: logEventArgs.UserIP,
            strRequestMethod: logEventArgs.RequestMethod,
            strRequestBody: logEventArgs.RequestBody,
            strResponseJson: logEventArgs.ResponseJson,
            shHttpStatusCode: logEventArgs.HttpStatusCode,
            intDurationMsecs: (int) logEventArgs.DurationTicks/10000,
            strServerHostname: Dns.GetHostName(),
            strPathAndQuery: logEventArgs.PathAndQuery,
            strRequestTemplate: logEventArgs.RequestTemplate,
            strUserAgent: logEventArgs.UserAgent,
            strErrorType: logEventArgs.ErrorType,
            strErrorMessage: logEventArgs.ErrorMessage,
            shServerID: ref logEventArgs.ApiServerID,
            intApiLogPathID: ref logEventArgs.ApiLogPathID,
            shApiLogTemplateID: ref logEventArgs.ApiLogTemplateID,
            intUserAgentID: ref logEventArgs.UserAgentID);

          if (logEventArgs.UserAgent != null)
            _dictUserAgent[logEventArgs.UserAgent] = logEventArgs.UserAgentID;
          if (logEventArgs.PathAndQuery != null)
            _dictApiLogPath[logEventArgs.PathAndQuery] = logEventArgs.ApiLogPathID;
          if (logEventArgs.RequestTemplate != null)
            _dictApiLogTemplate[logEventArgs.RequestTemplate] = logEventArgs.ApiLogTemplateID;
          if (logEventArgs.ServerHostname != null)
            _dictApiServer[logEventArgs.ServerHostname] = logEventArgs.ApiServerID;
        }
      }
      catch (Exception ex)
      {
        const string ACIX = "ACI_X";
        if (!EventLog.SourceExists(ACIX))
        {
          EventLog.CreateEventSource(ACIX, ACIX);
        }
        EventLog.WriteEntry(ACIX, String.Format("{0} at {1}", ex.Message, ex.StackTrace), EventLogEntryType.Error);
      }
    }
  }
}
