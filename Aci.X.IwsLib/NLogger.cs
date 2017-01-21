using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Aci.X.IwsLib
{
  public class NLogger
  {
    private Logger _logger;

    public NLogger( Logger logger) 
    {
      _logger = logger;
    }

    public void LogEvent(LogLevel level, string strMessage, params object[] oParams)
    {
      LogEvent(level: level, strMessage: strMessage, ex: null, oParams: oParams);
    }

    public void LogEvent(LogLevel level, Exception ex, string strMessage, params object[] oParams)
    {
      if (ex != null)
      {
        strMessage = String.Format("{0}, Ex:{1}, Stack:{2}", strMessage, ex.Message, ex.StackTrace);
      }
      LogEventInfo logEvent = new LogEventInfo(level, _logger.Name, System.Globalization.CultureInfo.CurrentCulture, strMessage, oParams, ex);
      System.Web.HttpContext context = System.Web.HttpContext.Current;
      if (context != null && context.Request != null)
      {
        string strRemoteAddr = context.Request.ServerVariables["REMOTE_ADDR"];
        string strForwardedFor = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
        logEvent.Properties["ClientIP"] = String.IsNullOrEmpty(strForwardedFor) ? strRemoteAddr : strForwardedFor;
        logEvent.Properties["ServerName"] = System.Net.Dns.GetHostName();
      }
      _logger.Log(logEvent);  
    }

  }
}
