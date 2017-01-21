using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using Aci.X.Business;
using Aci.X.Database;
using Aci.X.ServerLib;

namespace Aci.X.WebAPI
{
  public class MvcAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
  {
    public MvcAuthorizeAttribute(string strRoles)
    {
      Roles = strRoles;
    }
    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
      // This is a hack specifically for the help page.
      // By stuffing the authorization status into the session, we 
      // can enable the developer to pass their authorization token on the
      // querystring to the main help page, and from that point forward,
      // they will no longer need to pass it again, enabling them to click 
      // on links, etc. within the help page.
      string strSessionKey = "authorized_" + Roles ?? "";
      var vSessionVal = httpContext.Session[strSessionKey];
      if (vSessionVal != null && (bool)vSessionVal)
        return true;

      vSessionVal = CallContext.Current.IsAuthorized(Roles);
      httpContext.Session[strSessionKey] = vSessionVal;
      return (bool)vSessionVal;
    }
  }

  public class WebServiceAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
  {
    public static Dictionary<string, bool> _dictClientSecrets = new Dictionary<string, bool>();

    protected override bool IsAuthorized(System.Web.Http.Controllers.HttpActionContext actionContext)
    {
      bool boolIsClientSecretValid = false;
      string strClientSecret = CallContext.Current.ClientSecret;
      if (!String.IsNullOrEmpty(strClientSecret))
      {
        if (_dictClientSecrets.ContainsKey(strClientSecret))
        {
          boolIsClientSecretValid = _dictClientSecrets[strClientSecret];
        }
        else
        {
          using (SqlConnection conn = WebServiceConfig.WebServiceSqlConnection)
          {
            spApiClientIsAuthorized sp = new spApiClientIsAuthorized(conn);
            boolIsClientSecretValid = _dictClientSecrets[strClientSecret] = sp.Execute(strClientSecret);
          }
        }
      }

      if (boolIsClientSecretValid)
      {
        
      }
      return boolIsClientSecretValid &&
        CallContext.Current != null &&
        CallContext.Current.DBVisit != null && 
        !String.IsNullOrEmpty(CallContext.Current.UserIP);
    }

    protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
    {
      actionContext.Response = this.SetResponseMessage(
        actionContext.Request, 
        System.Net.HttpStatusCode.Unauthorized,
        new Exception("Unauthorized: X-Auth-Client-Secret or X-Visit-Guid header or X-User-IP-Address is invalid"));
    }
  }
}