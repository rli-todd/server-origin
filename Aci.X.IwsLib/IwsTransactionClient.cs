using System;
using System.Collections.Generic;
using NLog;
using TRAN = Aci.X.IwsLib.Commerce.v2_1.Transaction;
using ACIDB = Aci.X.Database;
using Aci.X.ClientLib;
using Aci.X.DatabaseEntity;
using Aci.X.ServerLib;
using Solishine.CommonLib;

namespace Aci.X.IwsLib
{
  public class IwsTransactionClient : CommerceClientBase
  {
    private TRAN.TransactionServicesPortTypeClient _iwsClient;
    private CallContext _context;
    private static NLogger _logger = new NLogger(LogManager.GetCurrentClassLogger());

    public IwsTransactionClient(CallContext context )
    {
      _context = context;
      _iwsClient = new TRAN.TransactionServicesPortTypeClient(
        "TransactionServicesPort",
        IwsConfig.TransactionApiBaseUrl);
    }

    private TRAN.ClientAuthType ClientAuth
    {
      get 
      {
        return new TRAN.ClientAuthType
        {
          ClientID = base.ClientID,
          AuthID = base.AuthID,
          AuthKey = base.AuthKey
        };
      }
    }

    public string Checkout(DBProduct[] products)
    {
      TRAN.PurchaseItemType[] purchaseItems = GetPurchaseItems(products);
      if (purchaseItems.Length == 0)
        throw new IwsException(System.Net.HttpStatusCode.BadRequest, "EmptyCart");

      TRAN.ValidateOrderRequest validateRequest = ValidateRequest(products);
      validateRequest.CVV = _context.DBUser.CardCVV;
      TRAN.ValidateOrderResponse validateResponse = _iwsClient.ValidateOrderAsync(validateRequest).Result;
      TRAN.CompletionResponseType completionResponse = validateResponse.CompletionResponse;
      if (completionResponse.CompletionCode != "1000")
      {
        _logger.LogEvent(LogLevel.Warn, "ValidateOrder CompletionCode={0}, Message={1}, Detail={2}",
          completionResponse.CompletionCode, completionResponse.ResponseMessage, completionResponse.ResponseDetail);
        throw new IwsException(System.Net.HttpStatusCode.BadRequest, completionResponse.ResponseMessage);
      }
      string strOrderHash = validateResponse.OrderHash;

      TRAN.CreateOrderResponse createResponse = _iwsClient.CreateOrderAsync(GetCreateOrderRequest(strOrderHash)).Result;
      completionResponse = createResponse.CompletionResponse;

      
      if (completionResponse.CompletionCode != "1000")
      {
        _logger.LogEvent(LogLevel.Warn, "CreateOrder CompletionCode={0}, Message={1}, Detail={2}",
          completionResponse.CompletionCode, completionResponse.ResponseMessage, completionResponse.ResponseDetail);
        throw new IwsException(System.Net.HttpStatusCode.BadRequest, completionResponse.ResponseDetail);
      }

      string strOrderXml = XmlHelper.Serialize(createResponse.Order, System.Text.UnicodeEncoding.Unicode);
      return strOrderXml;
    }

    public TRAN.Credit[] GetCredits()
    {
      var request = new TRAN.GetCreditsRequest
      {
        UserID = _context.DBVisit.IwsUserID??0,
        ClientAuth = ClientAuth
      };
      TRAN.GetCreditsResponse response = _iwsClient.GetCreditsAsync(request).Result;
      if (response.CompletionResponse.CompletionCode != "1000")
      {
        _logger.LogEvent(LogLevel.Warn, "GetCredits CompletionCode={0}, Message={1}, Detail={2}",
          response.CompletionResponse.CompletionCode, 
          response.CompletionResponse.ResponseMessage, 
          response.CompletionResponse.ResponseDetail);
        throw new IwsException(System.Net.HttpStatusCode.BadRequest, response.CompletionResponse.ResponseDetail);
      }
      return response.Credits;
    }

    public string GetOrder(int intIwsOrderID)
    {
      TRAN.GetOrderDetailRequest req = new TRAN.GetOrderDetailRequest
      {
        ClientAuth = ClientAuth,
        UserContext = MyUserContext,
        OrderID = intIwsOrderID
      };
      TRAN.GetOrderDetailResponse resp = _iwsClient.GetOrderDetailAsync(req).Result;
      if (resp.CompletionResponse.CompletionCode != "1000")
      {
        _logger.LogEvent(LogLevel.Error, "GetOrderDetail CompletionCode={0}, Message={1}, Detail={2}",
          resp.CompletionResponse.CompletionCode, resp.CompletionResponse.ResponseMessage, resp.CompletionResponse.ResponseDetail);
        return null;
      }
      /*
       * Putting the VisitID into the Adword is how we associate the order
       * with the user when we create it in our DB.
       */
      resp.Order.Channel.ChannelAdWord = _context.DBVisit.VisitID.ToString();
      return XmlHelper.Serialize(resp.Order, System.Text.UnicodeEncoding.Unicode);
    }

    private TRAN.ValidateOrderRequest ValidateRequest(DBProduct[] products)
    {
      return new TRAN.ValidateOrderRequest
      {
        RequestContext = MyRequestContext,
        UserContext = MyUserContext,
        Channel = MyChannel,
        ClientAuth = ClientAuth,
        CVV = _context.DBUser.CardCVV,
        PurchaseContext = MyPurchaseContext,
        PurchaseItemList = GetPurchaseItems(products)
      };
    }

    private TRAN.CreateOrderRequest GetCreateOrderRequest(string strOrderHash)
    {
      return new TRAN.CreateOrderRequest
      {
        Channel = MyChannel,
        ClientAuth = ClientAuth,
        OrderHash = strOrderHash,
        RequestContext = MyRequestContext,
        UserContext = MyUserContext,
        RequestReference = MyRequestReference
      };
    }

    private TRAN.RequestReferenceType MyRequestReference
    {
      get
      {
        return new TRAN.RequestReferenceType
        {
          ChannelID = ""
        };
      }
    }

    private TRAN.PurchaseItemType[] GetPurchaseItems(DBProduct[] products)
    {
      List<TRAN.PurchaseItemType> listItems = new List<TRAN.PurchaseItemType>();
      Cart cart = _context.DBVisit.Cart;
      if (cart != null && cart.Items != null)
      {
        foreach (OrderItem orderItem in cart.Items)
        {
          listItems.Add(new TRAN.PurchaseItemType
          {
            ProductID = orderItem.ProductExternalID,
            ProductIDSpecified = true,
            ReferenceID = orderItem.ProfileID,
            ShortTitle = "ACI_PRODUCT_" + orderItem.ProductID.ToString()
          });
        }
      }
      return listItems.ToArray();
    }

    private TRAN.RequestContext MyRequestContext
    {
      get 
      {
        return new TRAN.RequestContext
        {
          AccountList = "",
          EndUserIpAddress = _context.DBVisit.ClientIP
        };
      }
    }

    private TRAN.PurchaseContextType MyPurchaseContext
    {
      get
      {
        return new TRAN.PurchaseContextType
        {
          ClientTime = DateTime.UtcNow.AddMinutes(_context.UtcOffsetMins),
          ClientTimeSpecified = true,
          Language = _context.DBVisit.AcceptLanguage,
          /*
              “1” - if the order is placed through the normal search driven flow. (Search person, select report type, checkout.)
              “4” - if the order is placed within the report. (Upgrading a report to include more data)
              “7” - if the order is placed “top-down” meaning, no search was performed and the user is buying a “product” or “subscription” stand-alone
          */
          OrderLocation = "1",
          /*
              Needs to be set to “4”, but only matters if OrderLoaction=7 (see above)
           */
          RecurrenceChannel = "4",
          RemoteIP = _context.DBVisit.ClientIP,
          TimeZone = _context.DBVisit.UtcOffsetMins/60,
          TimeZoneSpecified = true,
          UserAgent = _context.UserAgent
        };
      }
    }

    private TRAN.UserContextType MyUserContext
    {
      get 
      {
        return new TRAN.UserContextType
        {
          EmailAddress = _context.DBUser.EmailAddress,
          UserToken = _context.DBVisit.IwsUserToken
        };
      }
    }

    private string ConvertStorefrontUserToken(string strToken)
    {
      byte[] bytes = StringHelper.HexStringToByteArray(strToken.Replace("-",""));
      return Convert.ToBase64String(bytes);
    }

    private TRAN.ChannelType MyChannel
    {
      get
      {
        return new TRAN.ChannelType
        {
          AffiliateReferenceID = "",
          ChannelAdWord = _context.DBVisit.VisitID.ToString(),
          ChannelID = Int32.Parse(_context.DBVisit.ReferCode??"14927")
        };
      }
    }
  }
}
