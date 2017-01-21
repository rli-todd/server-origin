using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using NLog;
using AttributeRouting;
using Aci.X.ServerLib;
using Cli=Aci.X.ClientLib;

namespace Aci.X.WebAPI
{
  public class ControllerBase : System.Web.Http.ApiController
  {
    private static ConcurrentDictionary<int, DateTime> _dictUserLastApiCall = new ConcurrentDictionary<int, DateTime>();
    static Logger _logger = LogManager.GetCurrentClassLogger();

    protected CallContext CallContext
    {
      get
      {
        // This gets stuffed into the HttpContext in ApiLogMessageHandler
        //return HttpContext.Current == null ? null : (CallContextType)HttpContext.Current.Items["CallContextType"];
        return CallContext.Current;
      }
    }

    protected Uri BaseUrl;

    protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
    {
      base.Initialize(controllerContext);
      RoutePrefixAttribute attr = (RoutePrefixAttribute)GetType().GetCustomAttributes(typeof(RoutePrefixAttribute), true).FirstOrDefault();

      if (attr != null)
      {
        string strUrl = Request.RequestUri.ToString();
        BaseUrl = new Uri(strUrl.Substring(0, strUrl.IndexOf(attr.Url) + attr.Url.Length + 1));
      }
      if (CallContext != null)
      {
        CallContext.Controller = controllerContext.ControllerDescriptor.ControllerName;
        CallContext.Template = controllerContext.RouteData.Route.RouteTemplate.ToString();
        CallContext.ActionMethod = (System.Reflection.MethodInfo)controllerContext.RouteData.Route.DataTokens["actionMethod"];
        CallContext.ApiLogged += new CallContext.ApiLogEventHandler(Business.ApiLogger.CallContext_ApiLogged);
      }
    }

    static ControllerBase()
    {

    }


    protected HttpResponseMessage GetStream(string strFormat, params object[] oParams)
    {
      return GetStream(String.Format(strFormat, oParams));
    }

    protected HttpResponseMessage GetStream(StringBuilder sb)
    {
      return GetStream(sb.ToString());
    }

    protected HttpResponseMessage GetStream(string str)
    {
      return new HttpResponseMessage
      {
        Content = new StreamContent(new MemoryStream(Encoding.UTF8.GetBytes(str)))
      };
    }

    protected HttpResponseMessage HttpStatusCodeStream(HttpStatusCode statusCode, Exception ex = null)
    {
      Response.StatusCode = (int)statusCode;
      Response.ContentType = "text/html; charset=UTF-8";
      return GetStream(@"<html>{0}{1}</html>", statusCode, (ex == null ? "" : ":" + ex.Message));
    }

    protected static string ApiRootUrl
    {
      get
      {
        Match m = Regex.Match(HttpContext.Current.Request.Url.ToString(), @"https?://[^/]*/v[0-9]*/");
        return m.Value;
      }
    }

    protected Cli.User GetAuthenticatedUser()
    {
      Cli.User retVal = null;
      //using (DbConnection conn = WebServiceConfig.WebServiceSqlConnection)
      //{
      //  DB.spUserGet sp = new spUserGet(conn);
      //  sp.ExecuteGetByUserID(CallContext.AuthorizedUserID, CallContext.AuthorizedUserID);
      //  if (sp.Users.Length > 0)
      //    retVal = Server.ServerUser.FromDB(CallContext, sp.Users[0], true);
      //}
      return retVal;
    }

    protected HttpResponse Response
    {
      get 
      {
        HttpContext context = System.Web.HttpContext.Current;
        return context == null ? null : context.Response;
      }
    }

    #region templated responses

    protected HttpResponseMessage ReturnResponse(HttpStatusCode statusCode)
    {
      if (Response != null)
        Response.StatusCode = (int)statusCode;
      return ReturnResponse(statusCode, new Cli.WebServiceResponse());
    }

    protected HttpResponseMessage ReturnResponse<T>(HttpStatusCode statusCode, T response = default(T))
    {
      if (Response != null)
        Response.StatusCode = (int)statusCode;
      Cli.WebServiceResponse<T> retVal = null;
      retVal = new Cli.WebServiceResponse<T>(payload: response);
      UpdateTypes(response);
      
      return Request.CreateResponse<Cli.WebServiceResponse<T>>(statusCode, retVal);
    }

    private void UpdateTypes(object oResponse)
    {
      if (oResponse != null)
      {
        Type type = oResponse.GetType();

        if (oResponse is Cli.WebServiceEntity)
        {
          ((Cli.WebServiceEntity)oResponse).__type = type.Name;
        }

        foreach (System.Reflection.FieldInfo fieldInfo in type.GetFields())
        {
          if (fieldInfo.FieldType.BaseType.Name == "WebServiceEntity")
          {
            UpdateTypes(fieldInfo.GetValue(oResponse));
          }
        }
        foreach (System.Reflection.PropertyInfo propInfo in type.GetProperties())
        {
          if (propInfo.PropertyType.BaseType != null && propInfo.PropertyType.BaseType.Name == "WebServiceEntity")
          {
            UpdateTypes(propInfo.GetValue(oResponse, null));
          }
        }
      }
    }

    protected HttpResponseMessage HttpStatusCreated<T>(T response, string strFormat, params object[] oParams)
    {
      HttpResponseMessage resp = ReturnResponse(HttpStatusCode.Created, response);
      resp.Headers.Location = GetServiceLink(strFormat, oParams);
      return resp;
    }
    protected HttpResponseMessage HttpStatusAccepted<T>(T response)
    {
      return ReturnResponse<T>(HttpStatusCode.Accepted, response);
    }

    protected HttpResponseMessage HttpStatusAccepted<T>(T response, string strFormat, params object[] oParams)
    {
      HttpResponseMessage resp = ReturnResponse(HttpStatusCode.Accepted, response);
      resp.Headers.Location = GetServiceLink(strFormat, oParams);
      return resp;
    }

    protected HttpResponseMessage HttpStatusOK()
    {
      return ReturnResponse(HttpStatusCode.OK);
    }

    protected HttpResponseMessage HttpStatusOK<T>()
    {
      return ReturnResponse<T>(HttpStatusCode.OK);
    }

    protected HttpResponseMessage HttpStatusOK<T>(T response)
    {
      return ReturnResponse<T>(HttpStatusCode.OK, response);
    }
    protected HttpResponseMessage HttpStatusNotFound<T>()
    {
      return ReturnResponse<T>(statusCode: HttpStatusCode.NotFound);
    }

    protected HttpResponseMessage HttpStatusForbidden<T>()
    {
      return ReturnResponse<T>(statusCode: HttpStatusCode.Forbidden);
    }

    protected HttpResponseMessage HttpStatusUnauthorized<T>()
    {
      return ReturnResponse<T>(statusCode: HttpStatusCode.Unauthorized);
    }

    protected HttpResponseMessage HttpStatusConflict<T>()
    {
      return ReturnResponse<T>(statusCode: HttpStatusCode.Conflict);
    }

    protected HttpResponseMessage HttpError<T>(T retVal)
    {
      return ReturnResponse<T>(response: retVal, statusCode: HttpStatusCode.Forbidden);
    }
    #endregion // templated responses


    //*************************************************

    #region untemplated responses
    protected Uri GetServiceLink(string strFormat, params object[] oParams)
    {
      return new Uri(BaseUrl, String.Format(strFormat, oParams));
    }

    private HttpResponseMessage HttpStatus(HttpStatusCode statusCode)
    {
      return ReturnResponse(statusCode);
    }
    protected HttpResponseMessage HttpStatusCreated(string strFormat, params object[] oParams)
    {
      Response.StatusCode = (int)HttpStatusCode.Created;
      string strLocation = GetServiceLink(strFormat, oParams).ToString();
      Response.RedirectLocation = strLocation;
      return ReturnResponse(HttpStatusCode.Created);
    }
    #endregion // untemplated responses

    #region Logging
    public void Log(LogLevel level, string strMessage, params object[] oParams)
    {
      _logger.Log(level, strMessage, oParams);
    }

    public void LogException(LogLevel level, string strMessage, Exception ex)
    {
      _logger.LogException(level, strMessage, ex);
    }


    #endregion // Logging

  }
}