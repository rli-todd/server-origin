using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CUST = Aci.X.IwsLib.Commerce.v2_1.Customer;
using NLog;
using ACIDB = Aci.X.Database;



namespace Aci.X.IwsLib
{
  public class IwsCustomerClient : CommerceClientBase
  {
    private CUST.AccountServicesPortTypeClient _iwsClient;
    private static NLogger _logger = new NLogger(LogManager.GetCurrentClassLogger());

    public IwsCustomerClient()
    {
      _iwsClient = new CUST.AccountServicesPortTypeClient(
      "AccountServicesPort",
      IwsConfig.CustomerApiBaseUrl);
    }

    private CUST.ClientAuthType ClientAuth
    {
      get
      {
        return new CUST.ClientAuthType
        {
          ClientID = base.ClientID,
          AuthID = base.AuthID,
          AuthKey = base.AuthKey
        };
      }
    }

    public string Authenticate( string strEmailAddress, string strPassword, string strClientIP)
    {
      CUST.AuthenticateUserRequest request = new CUST.AuthenticateUserRequest
      {
        ClientAuth = ClientAuth,
        LoginRequest = new CUST.LoginRequest
        {
          UserName = strEmailAddress,
          Password = strPassword,
          RemoteIp = strClientIP
        }
      };

      CUST.AuthenticateUserResponse response = _iwsClient.AuthenticateUserAsync(request).Result;
      if (response.CompletionResponse.CompletionCode != "1000")
      {
        _logger.LogEvent(LogLevel.Warn, "AuthenticateUser CompletionCode={0}, Message={1}, Detail={2}",
          response.CompletionResponse.CompletionCode, 
          response.CompletionResponse.ResponseMessage, 
          response.CompletionResponse.ResponseDetail);
        throw new IwsException(System.Net.HttpStatusCode.BadRequest, response.CompletionResponse.ResponseMessage);
      }
      return response.User.UserToken;
    }

  }
}
