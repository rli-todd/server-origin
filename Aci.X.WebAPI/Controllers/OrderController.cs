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
  [RoutePrefix("order")]
  public class OrderController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Pings the order controller
    /// </summary>
    /// <returns></returns>
    [GET("ping"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _GET_order_ping()
    {
      return HttpStatusOK();
    }

    /// <summary>
    /// Searches for orders in the user's order history with optional filters
    /// </summary>
    /// <param name="query_id"></param>
    /// <param name="profile_id"></param>
    /// <returns></returns>
    [GET("search?{firstname?}&{lastname?}&{state?}&{profile_id?}&{user_id:int?"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Order[]>))]
    public HttpResponseMessage _GET_order_search(
      string firstname=null,
      string lastname=null,
      string state=null,
      string profile_id = null,
      int? user_id=null)
    {
      return HttpStatusOK<Cli.Order[]>(
        Business.Order.Search(
          context: CallContext,
          strFirstName: firstname,
          strLastName: lastname,
          strState: state,
          strProfileID: profile_id,
          intUserID: user_id));
    }

    /// <summary>
    /// Returns the specified order.
    /// </summary>
    /// <param name="order_id"></param>
    /// <returns></returns>
    [GET("{order_id:int}?{is_external_id:bool}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Order>))]
    public HttpResponseMessage _GET_order_X(int order_id, bool is_external_id=false)
    {
      Cli.Order order = null;
      if (is_external_id)
      {
        order = Business.Order.GetByExternalID(CallContext, order_id);
      }
      else
      {
        var intAccessibleOrderIds = Business.Order.ValidateOrderAccess(CallContext, new int[] { order_id });
        if (intAccessibleOrderIds.Length > 0)
        {
          var orders = Business.Order.Get(
              context: CallContext,
              keys: intAccessibleOrderIds);
          if (orders.Length > 0)
            order = orders[0];
        }
      }
      if (order == null)
        throw new OrderNotFoundException();
      return HttpStatusOK<Cli.Order>(order);
    }

  }
}
