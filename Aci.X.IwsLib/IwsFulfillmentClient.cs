using System;
using System.Linq;
using Aci.X.DatabaseEntity;
using Aci.X.ClientLib;
using Aci.X.ServerLib;
using Fulfillment = Aci.X.IwsLib.Commerce.v2_1.Fulfillment;
using NLog;



namespace Aci.X.IwsLib
{
  public class IwsFulfillmentClient : CommerceClientBase
  {
    private CallContext _context;
    private Fulfillment.FulfillmentServicesPortTypeClient _iwsClient;
    private static NLogger _logger = new NLogger(LogManager.GetCurrentClassLogger());

    public IwsFulfillmentClient(CallContext context)
    {
      _context = context;
      _iwsClient = new Fulfillment.FulfillmentServicesPortTypeClient(
      "FulfillmentServicesPort",
      IwsConfig.FulfillmentApiBaseUrl);
    }

    private Fulfillment.ClientAuthType ClientAuth
    {
      get
      {
        return new Fulfillment.ClientAuthType
        {
          ClientID = base.ClientID,
          AuthID = base.AuthID,
          AuthKey = base.AuthKey
        };
      }
    }

    public string DebitOrder(
      Order order, 
      OrderItem orderItem, 
      string strProfileID,
      string strFirstName,
      string strMiddleInitial,
      string strLastName,
      string strState)
    {
      Fulfillment.DebitOrderRequest debitOrderRequest = new Fulfillment.DebitOrderRequest
      {
        ClientAuth = ClientAuth,
        UserContext = MyUserContext,
        OrderID = order.OrderExternalID,
        OrderItemID = orderItem.OrderItemExternalID,
        ComplexSearchCriteria = new Fulfillment.ComplexSearchCriteria
        {
          ProfileID = strProfileID,
          Person = new Fulfillment.Person
          {
            Name = new Fulfillment.Name
            {
              FirstName = strFirstName,
              MiddleName = strMiddleInitial,
              LastName = strLastName
            },
            Address = new Fulfillment.Address
            {
              State = strState
            }
          }
        }
      };
      var response = _iwsClient.DebitOrderAsync(debitOrderRequest).Result;
      Fulfillment.CompletionResponseType completionResponse = response.CompletionResponse;
      if (completionResponse.CompletionCode != "1000")
      {
        _logger.LogEvent(LogLevel.Warn, "DebitOrderCompletionCode={0}, Message={1}, Detail={2}",
          completionResponse.CompletionCode, completionResponse.ResponseMessage, completionResponse.ResponseDetail);
        throw new IwsException(System.Net.HttpStatusCode.BadRequest, completionResponse.ResponseMessage);
      }
      return response.RequestReferenceID;
    }

    private Fulfillment.UserContextType MyUserContext
    {
      get
      {
        return new Fulfillment.UserContextType
        {
          EmailAddress = _context.DBUser.EmailAddress,
          UserToken = _context.DBVisit.IwsUserToken
        };
      }
    }
  }
}
