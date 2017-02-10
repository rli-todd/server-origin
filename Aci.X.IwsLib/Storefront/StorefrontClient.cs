using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Web;
using System.Linq;
using Aci.X.ClientLib.Exceptions;
using Aci.X.ServerLib;
using Solishine.CommonLib;
using Cli = Aci.X.ClientLib;
using Aci.X.DatabaseEntity;

namespace Aci.X.IwsLib.Storefront
{
  public class StorefrontClient : ExternalApiClient
  {

    public StorefrontClient(CallContext context) : base( "Storefront", context )
		{
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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

    public User NewUser(Cli.User user, string strReferCode)
    {
      string strBody =
        "email_address=" + HttpUtility.UrlEncode(user.Email ?? "") +
        "&password=" + HttpUtility.UrlEncode(user.Password ?? "") +
        "&no_spam_flag=0" +
        "&phone=" + StringHelper.StripNonNumeric(user.Phone ?? "") +
        "&user_agreement=" + user.HasAcceptedUserAgreement.ToString() +
        "&first_name=" + HttpUtility.UrlEncode(user.FirstName ?? "") +
        "&last_name=" + HttpUtility.UrlEncode(user.LastName ?? "") +
        "&address=" + HttpUtility.UrlEncode(user.Address ?? "") +
        "&city=" + HttpUtility.UrlEncode(user.City ?? "") +
        "&state=" + HttpUtility.UrlEncode(user.State ?? "") +
        "&country=" + HttpUtility.UrlEncode(user.Country ?? "") +
        "&post_code=" + HttpUtility.UrlEncode(user.ZipCode ?? "") +
        "&remote_ip=" + HttpUtility.UrlEncode(user.RemoteIP ?? "") +
        "&company=" + HttpUtility.UrlEncode(user.Company) +
        "&refer=" + HttpUtility.UrlEncode(strReferCode ?? "");

      string strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: strBody,
        strResource: "account/createuser");

      UserResponse response = JsonObjectSerializer.Deserialize<UserResponse>(strJson);
      return response.user;
    }

    public User UpdateUser(DBUser dbUser)
    {
      var sfUser = new User
      {
        userId = _context.DBUser.ExternalID,
        name = new Name
        {
          firstName = dbUser.FirstName,
          middleName = dbUser.MiddleName,
          lastName = dbUser.LastName
        },
        active = true,
        userAgreementAccepted = true
      };
      var strJson = JsonObjectSerializer.Serialize(sfUser);
      var strBase64Json = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(strJson));
      string strBody =
        "user_token=" + HttpUtility.UrlEncode(_context.DBVisit.StorefrontUserToken) +
        "&user=" + strBase64Json;

      strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: strBody,
        strResource: "account/updateuserinformation");

      UserResponse response = JsonObjectSerializer.Deserialize<UserResponse>(strJson);
      return response.user;
    }

    public User GetUser(int intExternalUserID, string strIwsUserToken)
    {
      string strBody =
        "user_token=" + HttpUtility.UrlEncode(strIwsUserToken ?? "") +
        "&user_id=" + intExternalUserID.ToString();

      string strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: strBody,
        strResource: "account/getuserinformation");

      UserResponse response = JsonObjectSerializer.Deserialize<UserResponse>(strJson);
      return response.user;
    }

    public string AuthenticateUser(
      string strEmailAddress, 
      string strPassword, 
      string strRemoteIP, 
      out int intExternalUserID)
    {
      intExternalUserID = 0;
      string strBody =
        "email_address=" + HttpUtility.UrlEncode(strEmailAddress ?? "") +
        "&password=" + HttpUtility.UrlEncode(strPassword ?? "") +
        "&remote_ip=" + HttpUtility.UrlEncode(strRemoteIP ?? "");

      string strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: strBody,
        strResource: "account/authenticateuser");

      AuthenticateUserResponse response = JsonObjectSerializer.Deserialize<AuthenticateUserResponse>(strJson);
      if (response.responseCode == 1000 && response.userAuthentication.userId != 0)
      {
        intExternalUserID = response.userAuthentication.userId;
        return response.userAuthentication.userToken;
      }
      throw new BadUsernameOrPasswordException(response.responseCode, response.responseDetail.message);
    }

    public string UpdatePassword(string strUserToken, Cli.PasswordUpdate pwUpdate)
    {
      string strBody =
        "user_token=" + HttpUtility.UrlEncode(strUserToken) +
        "&old_password=" + pwUpdate.OldPassword +
        "&new_password=" + pwUpdate.NewPassword;

      string strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: strBody,
        strResource: "account/updatepassword");
      UpdatePasswordResponse response = JsonObjectSerializer.Deserialize<UpdatePasswordResponse>(strJson);
      if (response.responseCode != 1000 || response.userAuthentication.userId == 0)
      {
        if (response.responseCode==2004)
        {
          if (response.responseDetail.message.Contains("does not match"))
          {
            throw new BadUsernameOrPasswordException();
          }
          else if (response.responseDetail.message.Contains("must not match"))
          {
            throw new CannotReuseOldPasswordException();
          }
        }
        throw new AciException(response.responseDetail.message);
      }
      return response.userAuthentication.userToken;
    }

    public string ResetPassword(string strEmailAddress)
    {
      string strBody = "email_address=" + HttpUtility.UrlEncode(strEmailAddress);
      string strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: strBody,
        strResource: "account/resetpassword");
      ResetPasswordResponse response = JsonObjectSerializer.Deserialize<ResetPasswordResponse>(strJson);
      if (response.responseCode != 1000)
      {
        throw new StorefrontException(response.responseCode, response.responseDetail.message);
      }
      return response.temporaryPassword;
    }

    public Wallet GetPaymentInformation(string strStorefrontUserToken, int intIwsUserID)
    {
      string strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: "user_token="+HttpUtility.UrlEncode(strStorefrontUserToken),
        strResource: "account/getpaymentinformation",
        strQuery: "?user_id=" + intIwsUserID.ToString());

      GetPaymentInformationResponse response = JsonObjectSerializer.Deserialize<GetPaymentInformationResponse>(strJson);
      if (response.responseCode == "2005")
      {
        return GetPaymentInformation(response.resplacementToken, intIwsUserID);
      }

      return response.wallet;
    }

    public Card AddCard(string strUserToken, int intIwsUserID, Cli.CreditCard card)
    {
      string strBody =
        "user_token=" + HttpUtility.UrlEncode(strUserToken) +
        "&user_id=" + intIwsUserID.ToString() +
        "&cc_num=" + HttpUtility.UrlEncode(card.CreditCardNumber) +
        "&exp_date=" + HttpUtility.UrlEncode(card.ExpirationDate) +
        "&cardholder_name=" + HttpUtility.UrlEncode(card.CardHolderName) +
        "&address=" + HttpUtility.UrlEncode(card.Address) +
        "&city=" + HttpUtility.UrlEncode(card.City) +
        "&state=" + HttpUtility.UrlEncode(card.State) +
        "&country=US" +
        "&post_code=" + HttpUtility.UrlEncode(card.Zip);
      string strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody: strBody,
        strResource: "cards/addcard");
      CardsResponse response = JsonObjectSerializer.Deserialize<CardsResponse>(strJson);
      if (response.responseCode != 1000)
      {
        if (response.responseCode == 2004)
          throw new BadCreditCardFormatException();

        throw new StorefrontBadRequestException(response.responseCode, response.responseDetail.message);
      }

      ///*
      // * Debug
      //*/

      //strJson = ExecuteApiRequest(
      //  strMethod: "POST",
      //  strBody:
      //    "user_token=" + HttpUtility.UrlEncode(strUserToken) +
      //    "&user_ids=" + intIwsUserID.ToString() +
      //    "&active_only=true",
      //  strResource: "cards/getusercards"
      //  );
      //response = JsonObjectSerializer.Deserialize<CardsResponse>(strJson);
      return response.cards != null && response.cards.Count > 0 ? response.cards[0] : null;
    }

    public Card GetCard(string strUserToken, int intIwsUserID)
    {
      var strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody:
          "user_token=" + HttpUtility.UrlEncode(strUserToken) +
          "&user_ids=" + intIwsUserID.ToString() +
          "&active_only=true",
        strResource: "cards/getusercards"
        );
      var response = JsonObjectSerializer.Deserialize<CardsResponse>(strJson);
      return response.cards != null && response.cards.Count > 0 ? response.cards[0] : null;
    }

    private List<ValidateProductOffering> GetProductOfferingsFromCart()
    {
      List<ValidateProductOffering> listItems = new List<ValidateProductOffering>();
      var cart = _context.DBVisit.Cart;
      if (cart != null && cart.Items != null)
      {
        foreach (Cli.OrderItem orderItem in cart.Items)
        {
          listItems.Add(new ValidateProductOffering
          {
            productOfferingId = orderItem.OfferToken ?? orderItem.ProductToken
          });
        }
      }
      return listItems;
    }

    private PaymentMethod GetPaymentMethod()
    {
      // Get the card
      var wallet = GetPaymentInformation(_context.DBVisit.StorefrontUserToken, _context.DBVisit.IwsUserID ?? 0);
      var card = (from c in wallet.cards where c.active != 0 select c).FirstOrDefault();

      return new PaymentMethod
        {
          paymentMethodId = card.cardId,
          metaData = "'cvv'=" + _context.DBUser.CardCVV
        };
    }

    private TransactionRequestDetails GetTransactionDetails()
    {
      return new TransactionRequestDetails
        {
          referrer = Convert.ToInt32(IwsConfig.StorefrontRefererID),
          adword = _context.DBVisit.VisitID.ToString()
        };
    }

    private ValidateResponse ValidateOrderFromCart(PaymentMethod paymentMethod, List<ValidateProductOffering> listProducts)
    {
      var validateReq = new ValidateRequest
      {
        productOfferings = listProducts,
        payment = paymentMethod,
        transactionDetails = GetTransactionDetails()
      };


      var strJson = JsonObjectSerializer.Serialize(validateReq);
      var strBase64Json = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(strJson));

      var strRetJson = ExecuteApiRequest(
        strMethod: "POST",

        strBody: "json_data=" + strBase64Json,
        strQuery:
          "?userid=" + _context.DBVisit.IwsUserID.ToString(),
        strResource: "3.0/transaction/validate");
        //strResource: "transaction/validate");
      var response = JsonObjectSerializer.Deserialize<ValidateResponse>(strRetJson);
      if (response.responseCode != 1000)
      {
        throw new IwsException(HttpStatusCode.BadRequest, response.responseDetail.message);
      }
      return response;
    }

    public TransactionResponse Checkout()
    {
      var paymentMethod = GetPaymentMethod();
      var listProducts = GetProductOfferingsFromCart();

      var validationResponse = ValidateOrderFromCart(paymentMethod, listProducts);
      var transactionRequest = new TransactRequest
      {
        productOfferings = listProducts,
        payment = paymentMethod,
        transactionDetails = GetTransactionDetails(),
        token = validationResponse.token
      };

      var strJson = JsonObjectSerializer.Serialize(transactionRequest);
      var strBase64Json = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(strJson));

      var strRetJson = ExecuteApiRequest(
        strMethod: "POST",

        strBody: "json_data=" + strBase64Json,
        strQuery:
          "?userid=" + _context.DBVisit.IwsUserID.ToString(),
        strResource: "3.0/transaction/transact");
        //strResource: "transaction/transact");
      var response = JsonObjectSerializer.Deserialize<TransactionResponse>(strRetJson);
      if (response.responseCode != 1000)
      {
        throw new IwsException(HttpStatusCode.BadRequest, response.responseDetail.message);
      }
      return response;
    }

    public TransactionGetResponse GetTransactions( int[] intTransactionIDs=null)
    {
      var strQuery = "?userid=" + _context.DBVisit.IwsUserID.ToString();
      if (intTransactionIDs != null && intTransactionIDs.Length>0)
      {
        //var list = new List<int>(intTransactionIDs);
        //var strJson = JsonObjectSerializer.Serialize(list);
        //var strBase64Json = Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(strJson));
        strQuery = strQuery + "&transactionIds=" + string.Join(",",intTransactionIDs);
      }
      var strRetJson = ExecuteApiRequest(
        strMethod: "GET",
        strBody: null,
        strQuery: strQuery,
        strResource: "3.0/transaction/get");

      var response = JsonObjectSerializer.Deserialize<TransactionGetResponse>(strRetJson);
      if (response.responseCode != 1000)
      {
        throw new IwsException(HttpStatusCode.BadRequest, response.responseDetail.message);
      }
      return response;
    }

    public void RecordVisit()
    {
      var strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody:
          "referrer_id=" + IwsConfig.StorefrontRefererID +
          "&adword=" + _context.DBVisit.VisitID.ToString(),
        strResource: "persistence/recordvisit"
        );
      var response = JsonObjectSerializer.Deserialize<GenericResponse>(strJson);
    }

    public string ApplyPromoItem(string strProductToken, string strPromoCode)
    {
      var strJson = ExecuteApiRequest(
        strMethod: "POST",
        strBody:
          "promo_code=" + strPromoCode + 
          "&product_offering_id=" +strProductToken,
        strResource: "promotions/applyitem"
        );
      var response = JsonObjectSerializer.Deserialize<ApplyItemPromotionResponse>(strJson);
      return response != null && 
        response.details != null &&
        response.details.status == "VALID"
        ? response.details.productOfferingId
        : null;
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
      _logger.Trace(null, "StorefrontClient.GetApiRequest(strMethod:{0},strBody:{1},strResource:{2},strQuery:{3},strParamsLen:{4}",
        strMethod, strBody, strResource, strQuery, strParams == null ? -1 : strParams.Length);

      string strRequiredParams = 
        "client_id="+IwsConfig.StorefrontClientID +
        "&timestamp="+HttpUtility.UrlEncode(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")).Replace("+","%20");

      strQuery=strQuery==null
        ? "?" + strRequiredParams
        : String.Format(strQuery, strParams) + "&" + strRequiredParams;

      /*
       * This is a bit of a hack to enable us to pass in 3.0 API calls
       */

      string strUrlSuffix = strResource.StartsWith("3.0/")
        ? strResource + strQuery
        : String.Format(
            "{0}/{1}{2}", 
            IwsConfig.AuthProxyApiVersion,
            strResource, 
            strQuery);

      strBody = (strBody == null ? "" : strBody.Replace("+", "%20"));
      string strHashSource = 
        strUrlSuffix + 
        strBody +
        IwsConfig.StorefrontSharedSecret;

      string strRequestUrl = IwsConfig.AuthProxyApiBaseUrl 
        + strUrlSuffix 
        + "&signed_request=" 
        + Solishine.CommonLib.SHA256.HexStringFromString(strHashSource);

      HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(strRequestUrl);
      req.Method = strMethod;
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
