using System;
using System.Text;
using System.Net;
using System.Runtime.Remoting.Messaging;
using NLog;

namespace Aci.X.ServerLib
{
  public class ExternalApiClient
  {
    protected string _strApiName = "?";
    protected int _intMaxRetries = 4;
    protected CallContext _context;
    protected static Logger _logger = LogManager.GetCurrentClassLogger();

    public ExternalApiClient(string strApiName, CallContext context)
    {
      _strApiName = strApiName;
      _context = context;
    }

    protected virtual void LogApiCall(string strAuthHeader, string strResource, string strRequestUrl, string strRequestBody, string strResponse, int intDurationMsecs, HttpStatusCode statusCode)
    {
      if (WebServiceConfig.LogApiCalls)
      {
      }
    }

    protected virtual HttpWebRequest GetApiRequest(
      string strMethod,
      string strBody,
      string strResource,
      string strQuery = null,
      string strAuthorization = null,
      params string[] strParams)
    {
      return (HttpWebRequest)HttpWebRequest.Create(String.Format(strResource, strParams));
    }

    protected virtual string ExecuteApiRequest(
      string strMethod,
      string strBody,
      string strResource,
      string strQuery = null,
      string strAuthorization = null,
      params string[] strParams)
    {
      string strRequestUrl = string.Empty;
      string strResponse = String.Empty;
      DateTime dtStart = DateTime.Now;
      try
      {
        HttpWebRequest req = GetApiRequest(
          strMethod: strMethod,
          strBody: strBody,
          strResource: strResource,
          strQuery: strQuery,
          strAuthorization: strAuthorization,
          strParams: strParams);
        return GetApiResponse(req, strResource, strBody);
      }
      catch (Exception ex)
      {
        _logger.Log(LogLevel.Error, "ERROR " + strMethod + " " + strRequestUrl, ex);
        throw;
      }
    }

    protected string GetApiResponse(HttpWebRequest req, string strUrlFormat, string strBody, int intRetryCount = 0)
    {
      string strResponse = "";
      string strAuthHeader = req.Headers["Authentication"];
      HttpStatusCode statusCode = HttpStatusCode.OK;
      DateTime dtStart = DateTime.Now;
      try
      {
        using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
        {
          statusCode = resp.StatusCode;
          System.IO.Stream receiveStream = resp.GetResponseStream();
          Encoding encode = Encoding.UTF8;
          System.IO.StreamReader readStream = new System.IO.StreamReader(receiveStream, encode);
          strResponse = readStream.ReadToEnd();
          resp.Close();
          readStream.Close();
        }
      }
      catch (WebException wex)
      {
        HttpWebResponse resp = (HttpWebResponse)wex.Response;
        if (resp != null)
        {
          statusCode = resp.StatusCode;
          System.IO.Stream stream = resp.GetResponseStream();
          if (stream != null)
          {
            strResponse = new System.IO.StreamReader(stream).ReadToEnd();
          }
        }

        LogApiCall(
        strAuthHeader: strAuthHeader,
          strResource: strUrlFormat,
          strRequestUrl: req.RequestUri.ToString(),
          strRequestBody: strBody,
          strResponse: strResponse,
          intDurationMsecs: (int)DateTime.Now.Subtract(dtStart).TotalMilliseconds,
          statusCode: statusCode);
        if (intRetryCount >= _intMaxRetries)
        {
          _logger.Log(LogLevel.Error, "ABORTING " + req.Method + " " + req.RequestUri, wex);
          throw;
        }
        else
        {
          _logger.Log(LogLevel.Warn, "RETRYING" + req.Method + " " + req.RequestUri, wex);
          // "exponential backoff"
          int intSleepMsecs = (int)Math.Pow(2, intRetryCount) + new Random().Next(1000);
          System.Threading.Thread.Sleep(intSleepMsecs);
          return GetApiResponse(req, strUrlFormat, strBody, intRetryCount + 1);
        }
      }
      LogApiCall(
        strResource: strUrlFormat,
        strAuthHeader: strAuthHeader,
        strRequestUrl: req.RequestUri.ToString(),
        strRequestBody: strBody,
        strResponse: strResponse,
        intDurationMsecs: (int)DateTime.Now.Subtract(dtStart).TotalMilliseconds,
        statusCode: statusCode);
      return strResponse;
    }
  }
}
