using System.Web.Http;

namespace Aci.X.WebAPI
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      config.Properties["MaxReceivedMessageSize"] = int.MaxValue;
      config.Filters.Add(new WebServiceAuthorizeAttribute());
      config.Filters.Add(new WebServiceExceptionFilterAttribute());
      config.MessageHandlers.Add(new ApiLogMessageHandler());
      config.Formatters.Clear();
      config.Formatters.Add(new DataContractJsonMediaFormatter());

      var generalCache = Business.Cache.GeneralPurposeCache.Singleton;
      generalCache.LogUnauthenticatedCalls = generalCache.LogUnauthenticatedCalls;
    }
  }
}
