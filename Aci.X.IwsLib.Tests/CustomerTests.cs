using System;
using System.Threading.Tasks;
using Customer = Aci.X.IwsLib.Commerce.v2_1.Customer;
using Token = Aci.X.IwsLib.Commerce.v2_1.Token;
using Transaction = Aci.X.IwsLib.Commerce.v2_1.Transaction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Aci.X.IwsLib.Tests
{
  [TestClass]
  public class CustomerTests
  {
    private static Customer.ClientAuthType _customerClientAuth = new Customer.ClientAuthType
    {
      // NOTE: The following credentials were provided by Katie Patterson
      //ClientID = "100148",
      //AuthID = "7808164",
      //AuthKey = "18aa8ecaa"
      // The following credentials were in Justin's sample code
      ClientID = "100010",
      AuthID = "7693969",
      AuthKey = "Ra#d@#87"
    };
    private static Token.ClientAuthType _tokenClientAuth = new Token.ClientAuthType
    {
      ClientID = "100148",
      AuthID = "7808164",
      AuthKey = "18aa8ecaa"
    };

    private string _strCreditCardNumber = "4111111111111111";

    [TestMethod]
    public void Customer_TokenizeCreditCard()
    {
      string strToken = GetCreditCardToken(_strCreditCardNumber);
      Assert.IsNotNull(strToken);
      Token.TokenServicesPortTypeClient client = new Token.TokenServicesPortTypeClient();
    }

    [TestMethod]
    public async Task Customer_CreateUserAsync()
    {
      Assert.Inconclusive();
      return;
      // This API is deprecated
      //

      Customer.User user = PrepareTestUser(_strCreditCardNumber);
      string strUserToken = await CreateTestUserAsync(user);
      Assert.IsTrue(!String.IsNullOrEmpty(strUserToken));
    }

    [TestMethod]
    public async Task Customer_AuthenticateUserAsync()
    {
      Assert.Inconclusive();
      return;
      // This API is deprecated
      //
      Customer.AccountServicesPortTypeClient client = new Customer.AccountServicesPortTypeClient();
      Customer.User user = PrepareTestUser(_strCreditCardNumber);
      string strUserToken = await CreateTestUserAsync(user);

      // First try with wrong password
      Customer.AuthenticateUserRequest request = new Customer.AuthenticateUserRequest
      {
        ClientAuth = _customerClientAuth,
        LoginRequest = new Customer.LoginRequest
        {
          UserName = user.Login.UserName,
          Password = user.Login.Password + "xyz",
          RemoteIp = "127.0.0.1"
        }
      };
      Customer.AuthenticateUserResponse response = await client.AuthenticateUserAsync(request);
      Assert.AreEqual("1003", response.CompletionResponse.CompletionCode); // Bad password

      // Now try with correct password
      request = new Customer.AuthenticateUserRequest
      {
        ClientAuth = _customerClientAuth,
        LoginRequest = new Customer.LoginRequest
        {
          UserName = user.Login.UserName,
          Password = user.Login.Password,
          RemoteIp = "127.0.0.1"
        }
      };
      response = await client.AuthenticateUserAsync(request);
      Assert.AreEqual("1000", response.CompletionResponse.CompletionCode); 
    }

    [TestMethod]
    public async Task Customer_IsUserExistAsync()
    {
      throw new AssertInconclusiveException();
    }

    [TestMethod]
    public async Task Customer_ResetPasswordAsync()
    {
      throw new AssertInconclusiveException();
    }

    [TestMethod]
    public async Task Customer_UpdateCommunicationAsync()
    {
      throw new AssertInconclusiveException();
    }

    [TestMethod]
    public async Task Customer_UpdatePasswordAsync()
    {
      throw new AssertInconclusiveException();
    }

    [TestMethod]
    public async Task Customer_UpdatePaymentAsync()
    {
      throw new AssertInconclusiveException();
    }

    [TestMethod]
    public async Task Customer_UpdateUserAsync()
    {
      throw new AssertInconclusiveException();
    }

    private async Task<string> CreateTestUserAsync(Customer.User user)
    {
      Customer.AccountServicesPortTypeClient client = new Customer.AccountServicesPortTypeClient();
      Customer.CreateUserRequest request = new Customer.CreateUserRequest
      {
        Channel = new Customer.ChannelType
        {
          ChannelID = 1,
          AffiliateReferenceID = "",
          ChannelAdWord = ""
        },
        ClientAuth = _customerClientAuth,
        RequestContext = new Customer.RequestContext
        {
          AccountList = "",
          EndUserIpAddress = "127.0.0.1"
        },
        User = user
      };
      Customer.CreateUserResponse response = await client.CreateUserAsync(request);
      Assert.AreEqual("1000", response.CompletionResponse.CompletionCode);
      return response.UserToken;
    }

    private Customer.User PrepareTestUser(string strCreditCardNumber)
    {
      string strCreditCardToken = GetCreditCardToken(strCreditCardNumber);
      string strFirstName = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 12);
      string strLastName = "UnitTester";
      string strEmailAddress = strFirstName + "." + strLastName + "@appliedcreatalytics.com";
      return new Customer.User
      {
        FirstName = strFirstName,
        LastName = strLastName,
        Login = new Customer.Login
        {
          UserName = strEmailAddress,
          Password = "Password123!"
        },
        UserPreferences = new Customer.Preference[0],
        UserAgreements = new Customer.Agreement[0],
        Channels = new Customer.Channels
        {
          Emails = new Customer.Email[] 
          {
            new Customer.Email
            {
              Address = strEmailAddress
            }
          },
          Addresses = new Customer.Address[]
          {
            new Customer.Address
            {
              Street1 = "500 108th Ave NE",
              City = "Bellevue",
              State = "WA",
              Zip = "98004",
              Country = "US"
            }
          },
          Phones = new Customer.Phone[]
          {
            new Customer.Phone
            {
              Number = "18005555555"
            }
          },
          Urls = new Customer.WebUrl[] { }
        },
        Wallet = new Customer.Wallet
        {
          ITunes = new Customer.ITunes(),
          CreditCards = new Customer.CreditCard[] 
          {
            new Customer.CreditCard
            {
              Number = "",
              BinNumber = "",
              LastFour = "",
              CardToken = strCreditCardToken,
              Owner = strFirstName + " " + strLastName,
              Type = "1",
              ExpirationMonth = "04",
              ExpirationYear = "15",
              BillingAddress = new Customer.Address
              {
                Street1 = "500 108th Ave NE",
                City = "Bellevue",
                State = "WA",
                Zip = "98004",
                Country = "US"
              }
            }
          }
        }
      };
    }

    private string GetCreditCardToken(string strCreditCardNumber)
    {
      Token.TokenServicesPortTypeClient client = new Token.TokenServicesPortTypeClient();
      string strToken = null;
      client.ProtectCreditCard(_tokenClientAuth, strCreditCardNumber, out strToken);
      return strToken;
    }
  }
}