using System;
using System.Collections.Generic;
using NLog;
using SUB= Aci.X.IwsLib.Commerce.v2_1.Subscription;
using ACIDB = Aci.X.Database;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;
using Solishine.CommonLib;

namespace Aci.X.IwsLib
{
  public class IwsSubscriptionClient : CommerceClientBase
  {
    private SUB.SubscriptionServicesPortTypeClient _iwsClient;
    private CallContext _context;
    private static NLogger _logger = new NLogger(LogManager.GetCurrentClassLogger());

    public IwsSubscriptionClient(CallContext context)
    {
      _context = context;
      _iwsClient = new SUB.SubscriptionServicesPortTypeClient(
        "SubscriptionServicesPort",
        IwsConfig.SubscriptionApiBaseUrl);
    }

    public SUB.SubcriptionType GetSubscription(int intSubscriptionID)
    {
      /*
       * If the user is not logged in, there are no subscriptions..
       */
      if (_context.DBUser == null || _context.DBUser.UserID == 0)
      {
        return null;
      }

      var response = _iwsClient.GetSubscriptionDetailAsync(
        new SUB.GetSubscriptionDetailRequest()
        {
          UserContext = MyUserContext,
          ClientAuth = MyClientAuth,
          SubscriptionID = intSubscriptionID
        }).Result;

      ValidateResponse(response,_logger, "GetSubscriptions({0})", intSubscriptionID);
      return response.Subscription;
    }

    public SUB.SubcriptionType CancelSubscription(int intSubscriptionID)
    {
      /*
       * If the user is not logged in, there are no subscriptions..
       */
      if (_context.DBUser == null || _context.DBUser.UserID == 0)
      {
        return null;
      }

      var response = _iwsClient.CancelSubscriptionAsync(
        new SUB.CancelSubscriptionRequest()
        {
          UserContext = MyUserContext,
          ClientAuth = MyClientAuth,
          SubscriptionID = intSubscriptionID,
          ReasonCode = SUB.EnumerationOfReasonCode.UserInitiated,
          CancelledByUserID = (_context.DBVisit.IwsUserID??0).ToString()
        }).Result;
      ValidateResponse(response, _logger, "CancelSubscription({0})", intSubscriptionID);
      return response.Subscription;
    }

    public SUB.SubcriptionType[] CancelAllSubscriptions()
    {
      /*
       * If the user is not logged in, there are no subscriptions..
       */
      if (_context.DBUser == null || _context.DBUser.UserID == 0)
      {
        return new SUB.SubcriptionType[0];
      }

      var subscriptions = GetSubscriptions();
      for (int idx = 0; idx < subscriptions.Length; ++idx)
      {
        var sub = subscriptions[idx];
        if (sub.Status == "Active")
        {
          subscriptions[idx] = CancelSubscription(sub.SubscriptionID);
        }
      }
      return subscriptions;
    }

    public SUB.SubcriptionType[] GetSubscriptions()
    {
      /*
       * If the user is not logged in, there are no subscriptions..
       */
      if (_context.DBUser == null || _context.DBUser.UserID == 0)
      {
        return new SUB.SubcriptionType[0];
      }

      var response = _iwsClient.GetUserSubscriptionsAsync(
        new SUB.GetUserSubscriptionsRequest()
        {
          UserContext = MyUserContext,
          ClientAuth = MyClientAuth
        }).Result;

      ValidateResponse(response, _logger, "GetUserSubscriptions");

      var retVal = new SUB.SubcriptionType[response.Subscriptions.Length];
      for (int idx = 0; idx < response.Subscriptions.Length; idx++)
      {
        retVal[idx] = GetSubscription(response.Subscriptions[idx].SubscriptionID);
      }
      return response.Subscriptions;
    }

    private SUB.UserContextType MyUserContext
    {
      get
      {
        return new SUB.UserContextType
        {
          EmailAddress = _context.DBUser==null||_context.DBUser.UserID==0?null:_context.DBUser.EmailAddress,
          UserToken = _context.DBVisit.IwsUserToken
        };
      }
    }


    private SUB.ClientAuthType MyClientAuth
    {
      get
      {
        return new SUB.ClientAuthType
        {
          ClientID = base.ClientID,
          AuthID = base.AuthID,
          AuthKey = base.AuthKey
        };
      }
    }

  }
}
