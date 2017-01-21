using System;
using System.Text;
using System.Net;
using System.Web;
using Aci.X.ClientLib.Exceptions;
using Aci.X.ServerLib;
using Solishine.CommonLib;
using Cli = Aci.X.ClientLib;

namespace Aci.X.IwsLib.Storefront
{
  public class PaymentProxyClient : ExternalApiClient
  {

    public PaymentProxyClient(CallContext context)
      : base("PaymentProxy", context)
    {
    }

    #region Public methods
    public bool Ping()
    {
      string strJson = ExecuteApiRequest(
        strMethod: "GET",
        strBody: null,
        strResource: "ping");
      return strJson.Contains("Success");
    }

    private Nonce GetNonce()
    {
      var strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody:
          "clientid=" + IwsConfig.StorefrontClientID +
          "&enduserip=" + _context.UserIP,
        strResource: "getnonce");

      var resp = JsonObjectSerializer.Deserialize<NonceResponce>(strJson);
      resp.nonce.signednonce = SHA256.HexStringFromString(resp.nonce.nonce + IwsConfig.StorefrontSharedSecret);
      return resp.nonce;
    }

    public string GetPaymentJavaScript(string strUserToken, int intIwsUserID, string strPaymentDiv, out Nonce nonce)
    {
      nonce = GetNonce();
      var strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: 
          "clientid=" + IwsConfig.StorefrontClientID +
          "&nonce=" + nonce.nonce +
          "&signednonce=" + nonce.signednonce + 
          "&divfield=" + strPaymentDiv + 
          "&userid=" + _context.DBVisit.IwsUserID + 
          "&usertoken=" + _context.DBVisit.StorefrontUserToken,
          strResource: "getpaymentjs");

      /*
       * HACK
       */
      string strInternalBaseUrl = "http://integ-jweb11.tuk2.intelius.com:8080/paymentproxy-0.0.2/";
      strJson = strJson.Replace(strInternalBaseUrl, IwsConfig.PaymentProxyApiBaseUrl);
      return strJson;
    }

    public ClientLib.PaymentProxyResponse FakeSubmitPaymentForm(string strUserToken, int intIwsUserID, Cli.CreditCard card)
    {
      Nonce nonce = null;
      var JS = GetPaymentJavaScript(strUserToken, intIwsUserID, "PaymentDIV", out nonce);
      var timeFormat = "yyyy-MM-dd HH:mm:ss";
      var submitTimeFormat = "{0:ddd} {0:MMM} {0:dd} {0:yyyy} {0:HH}:{0:mm}:{0:ss} GMT";
      var submitTimeZoneFormat = "{0:zzz} (Pacific Daylight Time)";
      var timestamp = DateTime.Now.ToString(timeFormat);
      System.Threading.Thread.Sleep(1000);
      var submitTimestamp =
        String.Format(submitTimeFormat, DateTime.Now) +
        String.Format(submitTimeZoneFormat, DateTime.Now).Replace(":", "");
      var strBody =           
          "ccname=" + HttpUtility.UrlEncode(card.CardHolderName) +
          "&ccnum=" + HttpUtility.UrlEncode(card.CreditCardNumber) +
          "&cccvv=" + card.CVV +
          "&ccexpmonth=" + card.ExpirationDate.Substring(0,2) +
          "&ccexpyear=20" + card.ExpirationDate.Substring(2,2) +
          "&country=us" + 
          "&address=" + HttpUtility.UrlEncode(card.Address) +
          "&city=" + HttpUtility.UrlEncode(card.City) +
          "&state=" + HttpUtility.UrlEncode(card.State) +
          "&zip=" + HttpUtility.UrlEncode(card.Zip) +
          //"&ccsubmit=Submit" + 
          "&user_id=" + _context.DBVisit.IwsUserID +
          "&user_token=" + _context.DBVisit.StorefrontUserToken + 
          "&client_id=" + IwsConfig.StorefrontClientID +
          "&timestamp=" + HttpUtility.UrlEncode(timestamp) +
          "&submittimestamp=" + HttpUtility.UrlEncode(submitTimestamp) +
          "&nonce=" + nonce.nonce + 
          "&signednonce=" + nonce.signednonce;

      var response = ExecuteApiRequest(
        strMethod: "POST",
        strBody: strBody,
        strResource: "acceptformresponse");

      return JsonObjectSerializer.Deserialize<ClientLib.PaymentProxyResponse>(response);
    }

    #endregion // Public methods

    #region Private & Protected methods
    protected override HttpWebRequest GetApiRequest(
      string strMethod,
      string strBody,
      string strResource,
      string strQuery = null,
      string strAuthorization = null,
      params string[] strParams)
    {
      _logger.Trace(null, "PaymentProxyClient.GetApiRequest(strMethod:{0},strBody:{1},strResource:{2},strQuery:{3},strParamsLen:{4}",
        strMethod, strBody, strResource, strQuery, strParams == null ? -1 : strParams.Length);

      string strRequestUrl = IwsConfig.PaymentProxyApiBaseUrl +
        IwsConfig.PaymentProxyApiVersion +
        "/" +
        strResource;

      HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strRequestUrl);
      req.Method = strMethod;
      if (strResource != "acceptformresponse")
        req.Headers["X-Forwarded-For"] = _context.UserIP;
      if (strMethod == "POST" && strBody != null)
      {
        byte[] tReqBytes = Encoding.UTF8.GetBytes(strBody);
        req.ContentLength = tReqBytes.Length;
        req.ContentType = "application/x-www-form-urlencoded";
        System.IO.Stream reqStream = req.GetRequestStream();
        reqStream.Write(tReqBytes, 0, tReqBytes.Length);
        reqStream.Close();
      }
      return req;
    }

    #endregion // Private & Protected methods

  }
}
