using System.Net.Http;
using System.Web.Http;
using AttributeRouting;
using AttributeRouting.Web.Http;
using NLog;
using Aci.X.ClientLib;
using Aci.X.ClientLib.Exceptions;
using SF=Aci.X.IwsLib.Storefront;
using Cli = Aci.X.ClientLib;
using DB = Aci.X.Database;

namespace Aci.X.WebAPI
{
  /// <summary>
  /// User Controller
  /// </summary>
  [RoutePrefix("user")]
  public class UserController : ControllerBase
  {
    static Logger _logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Pings the user authentication service
    /// </summary>
    /// <returns></returns>
    [GET("ping"), HttpGet]
    [AllowAnonymous]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _GET_users_ping()
    {
      SF.StorefrontClient client = new SF.StorefrontClient(CallContext);
      if (client.Ping())
        return HttpStatusOK();
      else
        throw new PingFailedException();
    }

    /// <summary>
    /// Authenticates user with given credentials and returns an updated VisitToken
    /// </summary>
    /// <param name="credentials"></param>
    /// <returns></returns>
    [POST("authenticate"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse<Cli.User>))]
    public HttpResponseMessage _POST_user_authenticate([FromBody] Cli.UserCredentials credentials)
    {
      if (credentials.Complete)
      {
        var user = Business.User.Authenticate(CallContext, credentials);
        return HttpStatusOK<Cli.User>(user);
      }
      else
      {
        return HttpStatusUnauthorized<Cli.User>();
      }
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <returns></returns>
    [POST("new"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse<Cli.User>))]
    public HttpResponseMessage _POST_user_new([FromBody] Cli.User user)
    {
      user = Business.User.Create(CallContext, user);
      return HttpStatusCreated<Cli.User>(user, user.UserID.ToString());
      
    }

    /// <summary>
    /// Updates a user
    /// </summary>
    /// <returns></returns>
    [POST("update"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse<Cli.User>))]
    public HttpResponseMessage _POST_user_update([FromBody] Cli.User user)
    {
        user = Business.User.Update(CallContext, user);
        return HttpStatusOK<Cli.User>(user);
    }

      
      /// <summary>
    /// Returns active user
    /// </summary>
    /// <returns></returns>
    [GET("current"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.User>))]
    public HttpResponseMessage _GET_user_current()
    {
      return HttpStatusOK<Cli.User>(Business.User.GetCurrent(CallContext));
    }

    /// <summary>
    /// Returns specified user if user is a BackofficeReader
    /// </summary>
    /// <returns></returns>
    [POST("search"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse<Cli.User>))]
    public HttpResponseMessage _POST_user_search([FromBody] string emailAddress)
    {
      if (!CallContext.DBUser.IsBackofficeReader)
        throw new AuthorizationRequiredException();
      return HttpStatusOK<Cli.User>(Business.User.Get(CallContext, strEmailAddress: emailAddress));
    }


    /// <summary>
    /// Returns specified user if user is a BackofficeReader
    /// </summary>
    /// <returns></returns>
    [GET("{user_id:int}"), HttpGet]
    //[Authorize(Roles="BackofficeReader,BackofficeWriter")]
    [ReturnValue(typeof(WebServiceResponse<Cli.User>))]
    public HttpResponseMessage _GET_user_X(int user_id)
    {
      if (!CallContext.DBUser.IsBackofficeReader)
        throw new AuthorizationRequiredException();
      return HttpStatusOK<Cli.User>(Business.User.Get(CallContext, user_id));
    }



    /// <summary>
    /// Updates the user's payment method
    /// </summary>
    /// <returns></returns>
    [POST("card"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse<Cli.User>))]
    public HttpResponseMessage _POST_user_card([FromBody] Cli.CreditCard card)
    {
      var user = Business.User.AddCard(CallContext, card);
      return HttpStatusOK<Cli.User>(user);
    }

    /// <summary>
    /// Updates the user's payment method
    /// </summary>
    /// <returns></returns>
    [POST("wallet"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse<Cli.User>))]
    public HttpResponseMessage _POST_user_wallet([FromBody] Cli.Wallet wallet)
    {
      var user = Business.User.UpdateWallet(CallContext, wallet);
      return HttpStatusOK<Cli.User>(user);
    }

    /// <summary>
    /// Gets the user's payment method
    /// </summary>
    /// <returns></returns>
    [GET("card"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.CreditCard>))]
    public HttpResponseMessage _GET_user_card()
    {
      return HttpStatusOK<Cli.CreditCard>(Business.User.GetCard(CallContext));
    }

    /// <summary>
    /// Gets the user's credits
    /// </summary>
    /// <returns></returns>
    [GET("credits"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<Cli.Credit[]>))]
    public HttpResponseMessage _GET_user_credits()
    {
      return HttpStatusOK<Cli.Credit[]>(Business.User.GetCredits(CallContext));
    }

    /// <summary>
    /// Gets the client-side JavaScript for rendering and submitting the payment form
    /// </summary>
    /// <returns></returns>
    [GET("payment-javascript/{payment_div}"), HttpGet]
    [ReturnValue(typeof(WebServiceResponse<string>))]
    public HttpResponseMessage _GET_payment_javascript(string payment_div)
    {
      return HttpStatusOK<string>(Business.User.GetPaymentJavaScript(CallContext,payment_div));
    }

    /// <summary>
    /// Updates the user's payment method
    /// </summary>
    /// <returns></returns>
    [POST("payment-proxy"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse<PaymentProxyResponse>))]
    public HttpResponseMessage _POST_user_payment_proxy([FromBody] Cli.CreditCard card)
    {
      var response = Business.User.FakePaymentProxyForm(CallContext, card);
      return HttpStatusOK<PaymentProxyResponse>(response);
    }





    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <returns></returns>
    [GET("logout"), HttpGet]
    public HttpResponseMessage _GET_user_logout()
    {
      Business.User.Logout(CallContext);
      return HttpStatusOK();

    }

    /// <summary>
    /// Updates the user's password.
    /// </summary>
    /// <returns></returns>
    [POST("password"), HttpPost]
    [ReturnValue(typeof(WebServiceResponse))]
    public HttpResponseMessage _POST_user_password([FromBody] Cli.PasswordUpdate pwUpdate)
    {
      Business.User.UpdatePassword(CallContext, pwUpdate);
      return HttpStatusOK();
    }

    /// <summary>
    /// Resets the user's password.
    /// </summary>
    /// <returns></returns>
    [POST("password/reset"), HttpPost]
    [AllowAnonymous]
    public HttpResponseMessage _GET_user_password_reset([FromBody] string strEmailAddress)
    {
      Business.User.ResetPassword(CallContext, strEmailAddress);
      return HttpStatusOK();
    }



    /// <summary>
    /// Returns user subscriptions
    /// </summary>
    /// <returns></returns>
    [GET("subscriptions"), HttpGet]
    public HttpResponseMessage _GET_user_subscriptions()
    {
      var subscriptions = Business.User.GetSubscriptions(CallContext);
      return HttpStatusOK();

    }

    /// <summary>
    /// Returns a single user subscription
    /// </summary>
    /// <returns></returns>
    [GET("subscriptions/{subscription_id:int}"), HttpGet]
    public HttpResponseMessage _GET_user_subscription( int subscription_id)
    {
      var subscription = Business.User.GetSubscription(CallContext, subscription_id);
      return HttpStatusOK();
    }

    /// <summary>
    /// cancels a single user subscription
    /// </summary>
    /// <returns></returns>
    [GET("subscriptions/cancel/{subscription_id:int}"), HttpGet]
    public HttpResponseMessage _GET_user_subscription_cancel(int subscription_id)
    {
      var subscriptions = Business.User.CancelSubscription(CallContext, subscription_id);
      return HttpStatusOK();
    }

    /// <summary>
    /// cancels a single user subscription
    /// </summary>
    /// <returns></returns>
    [GET("subscriptions/cancel-all"), HttpGet]
    public HttpResponseMessage _GET_user_subscription_cancel_all()
    {
      var subscriptions = Business.User.CancelAllSubscriptions(CallContext);
      return HttpStatusOK();
    }

  }
}
