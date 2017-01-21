using System;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using NLog;
using Aci.X.ClientLib;
using Aci.X.ClientLib.Exceptions;
using Cli = Aci.X.ClientLib;
using DB = Aci.X.Database;

namespace Aci.X.WebAPI
{
  [RoutePrefix("visit")]
  public class VisitController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Pings the visit controller
    /// </summary>
    /// <returns></returns>
    [GET("ping"), HttpGet]
    [AllowAnonymous]
    [ReturnValue(typeof(WebServiceResponse<Cli.VisitToken>))]
    public HttpResponseMessage _GET_users_ping()
    {
      var fakeVisitToken = Business.Visit.Ping(CallContext);
      return HttpStatusOK<Cli.VisitToken>(fakeVisitToken);
    }    /// <summary>
    
    /// Generates an error
    /// </summary>
    /// <returns></returns>
    [GET("error"), HttpGet]
    [AllowAnonymous]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _GET_users_error()
    {
      throw new Exception("Fake error");
    }    /// <summary>
    /// Generates and returns a new visit token for the given user
    /// </summary>
    /// <param name="visitParams"></param>
    /// <returns></returns>
    [POST("new"), HttpPost]
    [AllowAnonymous]
    [ReturnValue(typeof(WebServiceResponse<Cli.VisitToken>))]
    public HttpResponseMessage _POST_visit_new([FromBody] Cli.VisitParams visitParams)
    {
      if (visitParams.Complete)
      {
        if (String.IsNullOrEmpty(CallContext.UserIP))
        {
          throw new VisitHeaderInvalidException();
        }
        var visitToken = Business.Visit.CreateVisit(CallContext, visitParams);
        return HttpStatusCreated<Cli.VisitToken>(visitToken, visitToken.Token.ToString());
      }
      else
      {
        return HttpStatusUnauthorized<Cli.VisitToken>();
      }
    }

 

  }
}
