using System.Linq;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using NLog;
using Aci.X.IwsLib;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;
using Cli = Aci.X.ClientLib;
using DB = Aci.X.Database;
using IwsCatalog = Aci.X.IwsLib.Commerce.v2_1.Catalog;

namespace Aci.X.WebAPI
{
  [RoutePrefix("catalog")]
  public class CatalogController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Pings the catalog controller
    /// </summary>
    /// <returns></returns>
    [GET("ping"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _GET_catalog_ping()
    {
      return HttpStatusOK();
    }

    /// <summary>
    /// Administrative call.  Refreshes catalog.
    /// </summary>
    /// <returns></returns>
    [GET("refresh"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _GET_catalog_refresh()
    {
      Business.Sku.RefreshCatalog(CallContext);
      return HttpStatusOK();
    }

    [GET("products"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Product[]>))]
    public HttpResponseMessage _GET_catalog_products()
    {
      return HttpStatusOK<Cli.Product[]>(Business.Product.GetAll(CallContext));
    }
  }
}
