using System.Net.Http;
using System.Threading.Tasks;
using Aci.X.ServerLib;
using Aci.X.Business;
using Aci.X.Business.Cache;

namespace Aci.X.WebAPI
{
  public class ApiLogMessageHandler : DelegatingHandler
  {
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
    {
      var context = CallContext.Current;
      if (context.VisitGuid != null)
      {
        context.DBVisit = Visit.Get(context.VisitGuid);
        if (context.DBVisit != null)
        {
          context.VisitID = context.DBVisit.VisitID;
          context.SiteID = (byte)context.DBVisit.SiteID;
          context.DBSite = Site.Get(context.SiteID);
          context.AuthorizedUserID = context.DBVisit.UserID;
          if (context.SiteID != 0 && context.AuthorizedUserID != 0)
          {
            context.DBUser = SiteUserCache.ForSite(context.SiteID).Get(context.AuthorizedUserID);
          }
        }
      }
      
      byte[] tRequestContent = await request.Content.ReadAsByteArrayAsync();
      byte[] tResponseContent = new byte[0];
      var response = await base.SendAsync(request, cancellationToken);
      if (response.Content != null && (WebServiceConfig.LogApiResponseSize || WebServiceConfig.LogApiResponse))
      {
        tResponseContent = await response.Content.ReadAsByteArrayAsync();
      }
      await Task.Run(() => context.Log(request, response, tRequestContent, tResponseContent));
      return response;
    }
  }
}