using System;
using System.Linq;
using Aci.X.Business.Cache;
using Aci.X.ClientLib.Exceptions;
using Aci.X.Database;
using Aci.X.DatabaseEntity;
using Aci.X.IwsLib;
using Aci.X.IwsLib.Storefront;
using Aci.X.ServerLib;
using Aci.X.ServerLib.Mandrill;
using Solishine.CommonLib;
using Client=Aci.X.ClientLib;
using SF=Aci.X.IwsLib.Storefront;
using NLog;

namespace Aci.X.Business
{
  public class User
  {
    public static Logger _logger = LogManager.GetCurrentClassLogger();

    public static Client.User Render(DBUser dbUser)
    {
      return new Client.User
      {
        SiteID=dbUser.SiteID,
        UserID=dbUser.UserID,
        UserGuid=dbUser.UserGuid,
        FirstName=dbUser.FirstName,
        MiddleName=dbUser.MiddleName,
        LastName=dbUser.LastName,
        Email=dbUser.EmailAddress,
        HasAcceptedUserAgreement=dbUser.HasAcceptedUserAgreement,
        HasValidPaymentMethod=dbUser.HasValidPaymentMethod,
        CardNumberLast4 = dbUser.CardNumberLast4,
        IsBackofficeReader = dbUser.IsBackofficeReader,
        IsBackofficeWriter = dbUser.IsBackofficeWriter,
        DateCreated = dbUser.DateCreated
      };
    }

    private static DBUser Get(byte tSiteID, int intUserID)
    {
      return SiteUserCache.ForSite(tSiteID).Get(intUserID);
    }

    public static ClientLib.User Authenticate(CallContext context, ClientLib.UserCredentials credentials)
    {
      StorefrontClient storefrontClient = new SF.StorefrontClient(context);
      int intIwsUserID = 0;
      string strStorefrontUserToken = storefrontClient.AuthenticateUser(
        strEmailAddress: credentials.EmailAddress,
        strPassword: credentials.Password,
        strRemoteIP: context.UserIP,
        intExternalUserID: out intIwsUserID);

      // Need to get another IWS User Token for use with transaction client
      IwsCustomerClient custCli = new IwsCustomerClient();
      string strIwsUserToken = custCli.Authenticate(
        strEmailAddress: credentials.EmailAddress,
        strPassword: credentials.Password,
        strClientIP: context.UserIP);

      // Now determine whether or not the user has a valid payment method
      bool boolHasValidPaymentMethod = false;
      SF.Wallet wallet = storefrontClient.GetPaymentInformation(strStorefrontUserToken, intIwsUserID);
      if (wallet != null && wallet.cards != null)
      {
        foreach (SF.Card card in wallet.cards)
        {
          if (card.active != 0)
          {
            boolHasValidPaymentMethod = true;
            break;
          }
        }
      }
      using (var db = new AciXDB())
      {
        DBUser dbUser = null;
        var user = SiteUserCacheByEmailAddress.ForSite(context.SiteID).Get(credentials.EmailAddress, boolForceRefresh:true);
        if (user == null)
        {
          /*
           * Handle the case where the IWS user already exists, but the ACI user does not.
           */
          var storefrontUser = storefrontClient.GetUser(intIwsUserID, strStorefrontUserToken);
          dbUser = db.spUserCreate(
            intSiteID: context.SiteID,
            intExternalID: intIwsUserID,
            intVisitID: context.VisitID,
            boolIsBackofficeReader: storefrontUser.admin,
            strEmailAddress: credentials.EmailAddress,
            strFirstName: storefrontUser.name.firstName,
            strMiddleName: storefrontUser.name.middleName,
            strLastName: storefrontUser.name.lastName,
            boolHasAcceptedUserAgreement: storefrontUser.userAgreementAccepted,
            strIwsUserToken: strIwsUserToken,
            strStorefrontUserToken: strStorefrontUserToken);
          _logger.Info("UserID {0} created for preexisting IWS user {1}", dbUser.UserID, dbUser.EmailAddress);
        }
        else
        {
          dbUser = db.spUserUpdate(
            intSiteID: context.SiteID,
            intExternalUserID: intIwsUserID,
            strIwsUserToken: strIwsUserToken,
            strStorefrontUserToken: strStorefrontUserToken,
            intVisitID: context.VisitID,
            boolHasValidPaymentMethod: boolHasValidPaymentMethod,
            boolUpdateDateLastAuthenticated: true);
        }
        SiteUserCache.ForSite(context.SiteID).Set(dbUser);
        context.DBUser = dbUser;
        // save the cart
        var cart = context.DBVisit.Cart;
        context.DBVisit = db.spVisitUpdateIwsUserToken(
          intSiteID: context.SiteID,
          intVisitID: context.VisitID,
          strIwsUserToken: strIwsUserToken,
          strStorefrontUserToken: strStorefrontUserToken,
          intUserID: dbUser.UserID,
          intIwsUserID: intIwsUserID);
        context.DBVisit.Cart = cart;
        Visit.Cache.Set(context.DBVisit);
        return Render(dbUser);
      }
    }

    public static ClientLib.User Get(CallContext context, int? intUserID=null, string strEmailAddress=null)
    {
      DBUser user = null;
      if (intUserID != null)
        user = SiteUserCache.ForSite(context.SiteID).Get(intUserID??0);
      else
        user = SiteUserCacheByEmailAddress.ForSite(context.SiteID).Get(strEmailAddress);
      return Render(user);
    }

    public static ClientLib.User GetCurrent(CallContext context)
    {
      var user = SiteUserCache.ForSite(context.SiteID).Get(context.AuthorizedUserID);
      return Render(user);
    }

    public static ClientLib.User Create(CallContext context, ClientLib.User user)
    {
      SF.StorefrontClient client = new SF.StorefrontClient(context);
      SF.User sfUser = client.NewUser(user, strReferCode: context.DBVisit.ReferCode);
      int intIwsUserID = 0;
      string strStorefrontUserToken = client.AuthenticateUser(
        strEmailAddress: user.Email,
        strPassword: user.Password,
        strRemoteIP: context.UserIP,
        intExternalUserID: out intIwsUserID);

      IwsCustomerClient custCli = new IwsCustomerClient();
      string strIwsUserToken = custCli.Authenticate(
        strEmailAddress: user.Email,
        strPassword: user.Password,
        strClientIP: context.UserIP);


      using (var db = new AciXDB())
      {
        var dbUser = db.spUserCreate(
          intSiteID: context.SiteID,
          intExternalID: sfUser.userId,
          intVisitID: context.VisitID,
          strEmailAddress: sfUser.userName,
          strFirstName: sfUser.name.firstName,
          strMiddleName: sfUser.name.middleName,
          strLastName: sfUser.name.lastName,
          boolHasAcceptedUserAgreement: sfUser.userAgreementAccepted,
          boolIsBackofficeReader: false,
          strStorefrontUserToken: strStorefrontUserToken,
          strIwsUserToken: strIwsUserToken);

        SiteUserCache.ForSite(context.SiteID).Set(dbUser);

        var dbVisit = db.spVisitUpdateIwsUserToken(
          intSiteID: context.SiteID,
          intVisitID: context.VisitID,
          strIwsUserToken: strIwsUserToken,
          strStorefrontUserToken: strStorefrontUserToken,
          intUserID: dbUser.UserID,
          intIwsUserID: intIwsUserID);

        context.DBVisit.IwsUserID = dbVisit.IwsUserID;
        context.DBVisit.IwsUserToken = dbVisit.IwsUserToken;
        context.DBVisit.IwsUserTokenExpiry = dbVisit.IwsUserTokenExpiry;
        context.DBVisit.StorefrontUserToken = dbVisit.StorefrontUserToken;
        context.DBVisit.UserGuid = dbVisit.UserGuid;
        context.DBVisit.UserID = dbVisit.UserID;

        Visit.Cache.Set(context.DBVisit);
        context.DBUser = dbUser;
        //MandrillClient.NewMessage(context, "NewAccount", strVarName: "user", oVarValue: dbUser).Send();
        return Render(dbUser);
      }
    }

    public static ClientLib.User Update(CallContext context, ClientLib.User user)
    {
      if (context.AuthorizedUserID != user.UserID)
      {
        throw new Aci.X.ClientLib.Exceptions.AuthorizationRequiredException();
      }

      SF.StorefrontClient client = new SF.StorefrontClient(context);
      var user2 = SiteUserCache.ForSite(context.SiteID).Get(context.AuthorizedUserID);
      user2.FirstName = user.FirstName;
      user2.MiddleName = user.MiddleName;
      user2.LastName = user.LastName;

      SF.User sfUser = client.UpdateUser(user2);

      using (var db = new AciXDB())
      {
        db.spUserUpdate(
          intVisitID: context.VisitID,
          intSiteID: context.SiteID,
          intUserID: user.UserID,
          strFirstName: user.FirstName,
          strLastName: user.LastName,
          strMiddleName: user.MiddleName);

        var dbUser = context.DBUser = 
          SiteUserCache.ForSite(context.SiteID).Get(context.AuthorizedUserID, boolForceRefresh: true);
        return Render(dbUser);
      }
    }

    private static void EnsurePostalFields(ClientLib.CreditCard card)
    {
      /*
       * Lookup the city and state if not provided
       */
      if (String.IsNullOrEmpty(card.State) || String.IsNullOrEmpty(card.City) && !String.IsNullOrEmpty(card.Zip))
      {
        using (var db = new GeoDB())
        {
          var postal = db.spPostalLookup(card.Zip);
          if (postal == null)
          {
            throw new InvalidPostalCodeException();
          }
          card.State = postal.StateAbbr;
          card.City = postal.City.Trim();
        }
      }
    }

    /*
     * Updates the user information based on a CreditCard.
     * Note: This path is no longer supported because of the requirement to use
     * the Intelius payment proxy.
     */
    private static Client.User UpdateUserFromCard(CallContext context, Client.CreditCard card)
    {
      using (var db = new AciXDB())
      {
        var dbUser = db.spUserUpdate(
          intUserID: context.AuthorizedUserID,
          intSiteID: context.SiteID,
          intVisitID: context.VisitID,
          strFirstName: card.FirstName,
          strLastName: card.LastName,
          strCardCVV: card.CVV,
          tCardHash: SHA1.FromObject(card),
          boolHasValidPaymentMethod: true);

        context.DBUser = dbUser;
        SiteUserCache.ForSite(context.SiteID).Set(dbUser);
        return Render(dbUser);
      }
    }

    /*
     * Updates user information based on the active credit card in the wallet
     * returned by the Intelius payment proxy.
     */
    private static Client.User UpdateUserFromCard(CallContext context, Client.Card card)
    {
      // Separate the parts of the name
      var idxSpace = card.cardholderName.IndexOf(' ');

      using (var db = new AciXDB())
      {
        var dbUser = db.spUserUpdate(
          intUserID: context.AuthorizedUserID,
          intSiteID: context.SiteID,
          intVisitID: context.VisitID,
          strFirstName: card.cardholderName.Substring(0,idxSpace),
          strLastName: card.cardholderName.Substring(idxSpace+1),
          strCardCVV: null,
          tCardHash: null,
          strCardLast4: card.ccNumLastFour,
          strCardExpiry: card.expDate,
          strCardholderName: card.cardholderName,
          strCardAddress: card.address,
          strCardCity: card.city,
          strCardState: card.state,
          strCardCountry: card.country,
          strCardZip: card.postcode,
          dtCardLastModified: DateTime.Parse(card.lastModified),
          boolHasValidPaymentMethod: true);

        context.DBUser = dbUser;
        SiteUserCache.ForSite(context.SiteID).Set(dbUser);
        return Render(dbUser);
      }
    }
    public static ClientLib.User AddCard(CallContext context, ClientLib.CreditCard card)
    {
      EnsurePostalFields(card);
      SF.StorefrontClient client = new SF.StorefrontClient(context);
      SF.Card sfCard = client.AddCard(
        strUserToken: context.DBVisit.StorefrontUserToken,
        intIwsUserID: context.DBVisit.IwsUserID ?? 0,
        card: card);
      return UpdateUserFromCard(context, card);
    }

    public static ClientLib.User UpdateWallet(CallContext context, ClientLib.Wallet wallet)
    {
      /*
       * Find an active credit card.
       * Note: There is not currently any support for multiple active cards.
       */
      var card = (from c in wallet.cards where c.active == "1" select c).FirstOrDefault();
      return UpdateUserFromCard(context, card);
    }

    public static ClientLib.CreditCard GetCard(CallContext context)
    {
      SF.StorefrontClient client = new SF.StorefrontClient(context);
      SF.Card sfCard = client.GetCard(
        strUserToken: context.DBVisit.StorefrontUserToken,
        intIwsUserID: context.DBVisit.IwsUserID ?? 0);
      return CreditCard.Render(sfCard);
    }

    public static ClientLib.Credit[] GetCredits(CallContext context)
    {
      IwsTransactionClient client = new IwsTransactionClient(context);
      var credits = client.GetCredits();
      /*
       * Pre-fetch all the credits and ensure that we have a known order for
       * each of them.  If there's no order for the credit, it means that the
       * order was created in Intelius' system, and we need to create one on our
       * side so that the credits can be used.
       */
      var orderCache = Cache.SiteOrderCacheByExternalID.ForSite(context.SiteID);
      foreach (var c in credits)
      {
        var order = orderCache.Get(c.TxnID, boolForceRefresh: true);
        if (order == null)
        {
          var strOrderXml = client.GetOrder(c.TxnID);
          if (strOrderXml != null)
          {
            int intOrderID = Order.CreateFromIwsOrder(context, strOrderXml);
            order = orderCache.Get(c.TxnID);
          }
        }
      }
      return Credit.Render(context.SiteID, credits);
    }

    public static string GetPaymentJavaScript(CallContext context, string strPaymentDiv)
    {
      SF.PaymentProxyClient client = new SF.PaymentProxyClient(context);
      Nonce nonce = null;
      return client.GetPaymentJavaScript(
        strUserToken: context.DBVisit.StorefrontUserToken,
        intIwsUserID: context.DBVisit.IwsUserID ?? 0,
        strPaymentDiv: strPaymentDiv,
        nonce: out nonce);
    }

    public static ClientLib.PaymentProxyResponse FakePaymentProxyForm(CallContext context, ClientLib.CreditCard card)
    {
      EnsurePostalFields(card);
      var client = new PaymentProxyClient(context);
      return client.FakeSubmitPaymentForm(
        strUserToken: context.DBVisit.StorefrontUserToken,
        intIwsUserID: context.DBVisit.IwsUserID ?? 0,
        card: card);
    }

    public static void UpdatePassword(CallContext context, ClientLib.PasswordUpdate pwUpdate)
    {
      SF.StorefrontClient client = new SF.StorefrontClient(context);
      var strStorefrontUserToken = client.UpdatePassword(context.DBVisit.StorefrontUserToken, pwUpdate);

      /*
       * Save the storefront token
       */
      using (var db = new AciXDB())
      {
        var dbVisit = db.spVisitUpdateIwsUserToken(
          intSiteID: context.SiteID,
          intVisitID: context.VisitID,
          strIwsUserToken: null,
          strStorefrontUserToken: strStorefrontUserToken,
          intUserID: context.DBVisit.UserID,
          intIwsUserID: context.DBVisit.IwsUserID ?? 0);

        context.DBVisit.StorefrontUserToken = dbVisit.StorefrontUserToken;
      }

      // Need to get another IWS User Token for use with transaction client
      IwsCustomerClient custCli = new IwsCustomerClient();
      var strIwsUserToken = custCli.Authenticate(
        strEmailAddress: context.DBUser.EmailAddress,
        strPassword: pwUpdate.NewPassword,
        strClientIP: context.UserIP);

      /*
       * Now save the IWS token
       */
      using (var db = new AciXDB())
      {
        var dbVisit = db.spVisitUpdateIwsUserToken(
          intSiteID: context.SiteID,
          intVisitID: context.VisitID,
          strIwsUserToken: strIwsUserToken,
          strStorefrontUserToken: null,
          intUserID: context.DBVisit.UserID,
          intIwsUserID: context.DBVisit.IwsUserID ?? 0);

        context.DBVisit.IwsUserToken = strIwsUserToken;
        context.DBVisit.IwsUserTokenExpiry = dbVisit.IwsUserTokenExpiry;
      }

      Visit.Cache.Set(context.DBVisit);
    }

    public static void ResetPassword(CallContext context, string strEmailAddress)
    {
      /*
       * First see if we have a user with this email address
       */
      int intUserID = 0;
      using (var db = new AciXDB())
      {
        intUserID = db.spUserSearch(context.SiteID, strEmailAddress);
      }
      if (intUserID != 0)
      {
        /*
         * Set the current context's user to be the desired user, 
         * only so that we can send the email out.   This context will not
         * persist beyond the current API call, so there is no security risk.
         */
        context.DBUser = SiteUserCache.ForSite(context.SiteID).Get(intUserID);
        SF.StorefrontClient client = new SF.StorefrontClient(context);
        var strTemporaryPassword = client.ResetPassword(strEmailAddress);
        MandrillClient.NewMessage(context, "PasswordReset", strVarName: "TemporaryPassword", oVarValue: strTemporaryPassword).Send();
      }
    }

    public static void Logout(CallContext context)
    {
      context.DBUser = null;
      context.DBVisit.UserGuid = Guid.Empty;
      context.DBVisit.UserID = 0;
      Visit.Cache.Set(context.DBVisit);
    }

    public static object GetSubscriptions(CallContext context)
    {
      IwsSubscriptionClient subCli = new IwsSubscriptionClient(context);
      return subCli.GetSubscriptions();
    }

    public static object GetSubscription(CallContext context, int intSubscriptionID)
    {
      IwsSubscriptionClient subCli = new IwsSubscriptionClient(context);
      return subCli.GetSubscription(intSubscriptionID);
    }

    public static object CancelAllSubscriptions(CallContext context)
    {
      IwsSubscriptionClient subCli = new IwsSubscriptionClient(context);
      return subCli.CancelAllSubscriptions();
    }

    public static object CancelSubscription(CallContext context, int intSubscriptionID)
    {
      IwsSubscriptionClient subCli = new IwsSubscriptionClient(context);
      return subCli.CancelSubscription(intSubscriptionID);
    }
  }
}
