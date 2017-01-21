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
  [RoutePrefix("cart")]
  public class cartController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Pings the cart controller
    /// </summary>
    /// <returns></returns>
    [GET("ping"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _GET_cart_ping()
    {
      return HttpStatusOK();
    }

    /// <summary>
    /// Updates the cart with the cart specified in the request payload
    /// </summary>
    /// <param name="cart"></param>
    /// <returns></returns>
    [POST("update"), HttpPost]
    public HttpResponseMessage _POST_cart_update([FromBody] Cli.Cart cart)
    {
      Business.Cart.Update(CallContext, cart);
      return HttpStatusOK();
    }

    /// <summary>
    /// Gets the shopping cart for the current user's visit.
    /// </summary>
    /// <returns></returns>
    [GET("get"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Cart>))]
    public HttpResponseMessage _GET_cart()
    {
      return HttpStatusOK<Cli.Cart>(Business.Cart.Get(CallContext));
    }

    /// <summary>
    /// Adds the specified SKU to the cart.
    /// </summary>
    /// <param name="product_sku_id"></param>
    /// <returns></returns>
    [GET("add/{product_sku_id:int}?{firstname?}&{lastname?}&{state?}&{profile_id?}&{promo}"),HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Cart>))]
    public HttpResponseMessage _GET_add_X(int product_sku_id, string firstname=null, string lastname=null, string state=null, string profile_id=null, string promo=null)
    {
      var cart = Business.Cart.AddSku(
        context: CallContext, 
        intSkuID: product_sku_id, 
        strProfileID: profile_id, 
        strFirstName: firstname,
        strLastName: lastname,
        strState: state,
        strPromoCode: promo);
      return HttpStatusOK<Cli.Cart>(cart);
    }

    /// <summary>
    /// Removes the specified SKU from the cart.http://localhost:62503/select/linda-kubek-in-united-states/twyhqlqmbbce
    /// </summary>
    /// <param name="product_sku_id"></param>
    /// <returns></returns>
    [GET("remove/{product_sku_id:int}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Cart>))]
    public HttpResponseMessage _GET_remove_X(int product_sku_id)
    {
      var cart = Business.Cart.RemoveSku(CallContext, product_sku_id);
      return HttpStatusOK<Cli.Cart>(cart);
    }

    /// <summary>
    /// Checks out the user with the contents of the cart.
    /// User must have a valid credit card on file.
    /// </summary>
    /// <returns></returns>
    [GET("checkout"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Order>))]
    public HttpResponseMessage _GET_checkout()
    {
      if (CallContext.AuthorizedUserID==0)
      {
        throw new AuthorizationRequiredException();
      }
      if (!CallContext.DBUser.HasValidPaymentMethod)
      {
        throw new NoPaymentMethodException();
      }
      ClientLib.Order order = Business.Order.Checkout(CallContext);
      Business.Cart.Update(CallContext, null);
      return HttpStatusOK<Cli.Order>(order);
    }

    /// <summary>
    /// Clears the shopping cart for the current user.
    /// </summary>
    /// <returns></returns>
    [GET("clear"), HttpGet]
    public HttpResponseMessage _GET_clear()
    {
      Business.Cart.Update(CallContext, null);
      return HttpStatusOK();
    }
  }
}
