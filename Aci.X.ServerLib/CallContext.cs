using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Net.Http;
using Aci.X.DatabaseEntity;
using NLog;
using Solishine.CommonLib;
using System.Text.RegularExpressions;

namespace Aci.X.ServerLib
{
  [DataContract(Namespace = "")]
  public class CallContext
  {
    /*static*/
    private static Logger _logger = LogManager.GetCurrentClassLogger();
    private static CacheClient Cache = new CacheClient();
    const string HEADER_AUTH_CLIENT_SECRET = "X-Auth-Client-Secret";
    const string HEADER_VISIT_GUID = "X-Visit-Guid";
    const string HEADER_USER_IP_ADDRESS = "X-User-IP-Address";
    const string HEADER_HTTP_X_FORWARDED_FOR = "HTTP_X_FORWARDED_FOR";
    const string HEADER_X_FORWARDED_FOR = "X_FORWARDED_FOR";
    const string HEADER_REMOTE_ADDR = "REMOTE_ADDR";

    public delegate void ApiLogEventHandler(object sender, ApiLogEventArgs ea);
    public /*static*/ event ApiLogEventHandler ApiLogged;

    #region Serialized members
    [DataMember(EmitDefaultValue = false)] public byte SiteID;
    [DataMember(EmitDefaultValue = false)] public bool IsGuest = false;
    [DataMember(EmitDefaultValue = false)] public bool IsAdministrator = false;
    [DataMember(EmitDefaultValue = false)] public string[] Roles = new string[0];
    [DataMember(EmitDefaultValue = false)] public DateTime StartTime = DateTime.UtcNow;
    [DataMember(EmitDefaultValue = false)] public string UserAgent;
    [DataMember(EmitDefaultValue = false)] public string ClientIP;
    [DataMember(EmitDefaultValue = false)] public string UserIP;
    [DataMember(EmitDefaultValue = false)] public int UtcOffsetMins;
    [DataMember(EmitDefaultValue = false)] public string VisitGuid;
    [DataMember(EmitDefaultValue = false)] public int VisitID;
    [DataMember(EmitDefaultValue = false)] public int AuthorizedUserID;
    [DataMember(EmitDefaultValue = false)] public DBVisit DBVisit;
    [DataMember(EmitDefaultValue = false)] public DBUser DBUser;
    [DataMember(EmitDefaultValue = false)] public DBSite DBSite;
 
    [DataMember(EmitDefaultValue = false)] public string ClientSecret;
    [DataMember(EmitDefaultValue = false)] public string Controller;
    [DataMember(EmitDefaultValue = false)] public string Template;
    [DataMember(EmitDefaultValue = false)] public Exception Exception;


    /// <summary>
    /// This member is used for reducing the number of logged 
    /// paths associated with a particular API call.  For example,
    /// if a spider crawls a particular city, say Los Angeles, there
    /// will be millions of distinct paths associated with that activity.
    /// If we set the logged request path for the following API:
    /// states/{state_fips:int}/cities/{city_fips:int}/lastname/{lastname}/firstname/{firstname_starts_with}
    /// to, say:
    /// states/{state_fips}/cities/{city_fips}
    /// We can still track the relative activity volume between cities with 
    /// relatively less overhead.  We lose only the fine-grained resolution 
    /// for individual nodes within the city.
    /// </summary>
    [DataMember(EmitDefaultValue = false)]
    public string LoggedRequestPath;

    #endregion // Serialized Members

    public System.Reflection.MethodInfo ActionMethod;

    public static CallContext Current
    {
      get
      {
        CallContext retVal = null;
        HttpContext context = HttpContext.Current;
        if (context != null)
        {
          retVal = HttpContext.Current.Items["CallContextType"] as CallContext;
          if (retVal == null)
          {
            HttpContext.Current.Items["CallContextType"] = retVal = new CallContext(HttpContext.Current.Request);
          }
        }
        else
        {
          retVal = new CallContext();
        }
        return retVal;
      }
    }

    public CallContext(HttpRequest request)
    {
      string strVisitGuid = request.Headers[HEADER_VISIT_GUID];
      if (String.IsNullOrEmpty(strVisitGuid))
      {
        // Not specified in the authorization header, so look for it on the query string
        strVisitGuid = request.QueryString["visit"];
      }
      VisitGuid = strVisitGuid;
      UserAgent = request.UserAgent;
      UserIP = request.Headers[HEADER_USER_IP_ADDRESS];
      if (WebServiceConfig.UseServerPublicIpAddressForUserIp)
      {
        const string CACHE_KEY = "ServerPublicIpAddress";
        string strPublicIpAddress = Cache.Get<string>(CACHE_KEY);
        if (String.IsNullOrEmpty(strPublicIpAddress))
        {
          var webCli = new System.Net.WebClient();
          webCli.Headers["User-Agent"] = "Mozilla/5.0 (Linux; Android 4.4.2; es-us; SAMSUNG GT-I9195L Build/KOT49H) AppleWebKit/537.36 (KHTML, like Gecko) Version/1.5 Chrome/28.0.1500.94 Mobile Safari/537.36";
          var html = webCli.DownloadString("http://whatismyipaddress.com/");
          Regex regex = new Regex("//whatismyipaddress.com/ip/([0-9.]*)", RegexOptions.Singleline);
          Match m = regex.Match(html);
          if (m.Success)
          {
            strPublicIpAddress = m.Groups[1].Value;
          }
          Cache.Put(CACHE_KEY, strPublicIpAddress);
        }
        UserIP = strPublicIpAddress;
      }
      if (String.IsNullOrEmpty(UserIP))
      {
        UserIP = request.QueryString["user_ip"];
      }
      ClientIP = (request.ServerVariables[HEADER_HTTP_X_FORWARDED_FOR] ?? String.Empty).Split(new char[] { ',' })[0].Trim();
      if (String.IsNullOrEmpty(ClientIP))
      {
        ClientIP = (request.ServerVariables[HEADER_X_FORWARDED_FOR] ?? String.Empty).Split(new char[] { ',' })[0].Trim();
        if (String.IsNullOrEmpty(ClientIP))
        {
          ClientIP = request.ServerVariables[HEADER_REMOTE_ADDR];
        }
      }
      ClientSecret = request.Headers[HEADER_AUTH_CLIENT_SECRET];
      if (String.IsNullOrEmpty(ClientSecret))
      {
        ClientSecret = request.QueryString["client_secret"];
      }
    }

    public CallContext()
    {
      // TODO: initialize all the "null" values for this "null" context
    }

    public void Log(HttpRequestMessage request, HttpResponseMessage response, byte[] tRequestContent, byte[] tResponseContent)
    {
      string strResponseJson = null;
      int intResponseSize = 0;
      string strRequestBody = "";

      var requestType = request.Content.Headers.ContentType;
      if (requestType != null)
      {
        if (requestType.MediaType == "application/json")
        {
          strRequestBody = System.Text.UTF8Encoding.UTF8.GetString(tRequestContent);
        }
        else
        {
          strRequestBody = requestType.ToString();
        }
      }

      strResponseJson = "";
      intResponseSize = tRequestContent.Length;

      Uri uri = request.RequestUri;
      string strPathAndQuery = uri.PathAndQuery;
      string[] strSegments = uri.Segments;



      if (ApiLogged != null)
      {
        TimeSpan ts = DateTime.UtcNow.Subtract(StartTime);
        ApiLogged(typeof(CallContext), new ApiLogEventArgs
        {
          SiteID = SiteID,
          VisitID = VisitID,
          UserID = AuthorizedUserID != 0 ? AuthorizedUserID : AuthorizedUserID,
          ClientIP = IPAddressHelper.ToInt(ClientIP),
          UserIP = IPAddressHelper.ToInt(UserIP),
          RequestMethod = request.Method.ToString(),
          PathAndQuery = strPathAndQuery,
          Controller = Controller,
          RequestTemplate = Template,
          UserAgent = UserAgent,
          RequestBody = System.Text.UTF8Encoding.UTF8.GetString(tRequestContent),
          DurationTicks = (long)ts.Ticks,
          HttpStatusCode = (short)response.StatusCode,
          ErrorType = this.Exception == null ? null : this.Exception.GetType().Name,
          ErrorMessage = this.Exception == null ? null : this.Exception.Message,
          ResponseJson = strResponseJson,
          ResponseSize = intResponseSize
        });
      }
    }

    public void LogApiEntry()
    {
      StartTime = DateTime.UtcNow;
    }

    public bool IsAuthorized(string strRoles)
    {
      bool boolIsAuthorized = AuthorizedUserID != 0;
      if (boolIsAuthorized && !String.IsNullOrEmpty(strRoles))
      {
        boolIsAuthorized = false;
        string[] strAllowedRoles = strRoles.Split(',');
        foreach (string strAllowedRole in strAllowedRoles)
        {
          if (Roles.Contains(strAllowedRole))
          {
            boolIsAuthorized = true;
            break;
          }
        }
      }
      //    if (strAllowedRole.Equals("BackofficeWriter", StringComparison.CurrentCultureIgnoreCase)
      //      && DBUser != null
      //      && DBUser.IsBackofficeWriter)
      //    {
      //      boolIsAuthorized = true;
      //      break;
      //    }
      //  }
      //}
      return boolIsAuthorized;
    }

  }
}