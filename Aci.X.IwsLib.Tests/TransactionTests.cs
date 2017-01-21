using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TRAN=Aci.X.IwsLib.Commerce.v2_1.Transaction;
using CUST = Aci.X.IwsLib.Commerce.v2_1.Customer;
using Storefront = Aci.X.IwsLib.Storefront;
using Cli = Aci.X.ClientLib;
using Aci.X.ClientLib;
using Aci.X.WebAPI.Tests;
using Aci.X.WebAPI.Tests.Controllers;

namespace Aci.X.IwsLib.Tests
{
  [TestClass]
  public class TransactionTests
  {
    //private const string _strEmailAddress = "p-trobertson@intelius.com";
    //private const string _strPassword = "publicrecords500";
    //private const string _strPassword = "pr.com.2014";
    private const string _strEmailAddress = "iws_test@solishine.com";
    private const string _strPassword = "Password1.";


    private static CUST.ClientAuthType _customerClientAuth= new CUST.ClientAuthType
    {
      ClientID = "100010",
      AuthID = "7693969",
      AuthKey = "Ra#d@#87"
    };
    private static TRAN.ClientAuthType _transactionClientAuth = new TRAN.ClientAuthType
    {
      ClientID = "100010",
      AuthID = "7693969",
      AuthKey = "Ra#d@#87"
    };

    [TestMethod]
    public async Task Transaction_ValidateOrderAsync()
    {
      CUST.AccountServicesPortTypeClient customerClient = new CUST.AccountServicesPortTypeClient();
      TRAN.TransactionServicesPortTypeClient transactionClient = new TRAN.TransactionServicesPortTypeClient();

      CUST.AuthenticateUserResponse authenticateResponse = await customerClient.AuthenticateUserAsync(
        GetAuthenticateRequest(strEmailAddress: "iws_test_20140619102836@solishine.com", strPassword: "Password1."));

      TRAN.ValidateOrderResponse validateResponse = await transactionClient.ValidateOrderAsync(
        GetValidateRequest(authenticateResponse.User.UserToken));
      Assert.AreEqual("1000", validateResponse.CompletionResponse.CompletionCode);
    }

    [TestMethod]
    public void Transaction_30_Validate()
    {
      const string PRODUCT_OFFERING_ID = "ldlfkjdflkdjfd";
      var intPaymentMethodId = 0;
      var request = new Storefront.ValidateRequest
      {
        productOfferings = new List<Storefront.ValidateProductOffering>(
          new Storefront.ValidateProductOffering[] { 
            new Storefront.ValidateProductOffering
            {
              productOfferingId = PRODUCT_OFFERING_ID
            }
          }),
        payment = new Storefront.PaymentMethod 
        {
          paymentMethodId = intPaymentMethodId
        }
      };

    }

    [TestMethod]
    public void Transaction_30_Transact()
    {

    }

    [TestMethod]
    public async Task Transaction_CreateOrderAsync()
    {
      WebServiceClient client = new WebServiceClient(UserControllerTests.CLIENT_SECRET, "127.0.0.1");
      VisitControllerTests.CreateTestVisit(client);
      User user = UserControllerTests.CreateTestUser(client, "Password1.");
      //const string CC = "371144371144376";
      const string CC = "4444424444444440";
      /*
        "4111111111111111", // Valid Luhn
        "4444424444444440", // Valid Luhn
        "36111111111111", // Valid Luhn
        "371449635398431", // Valid Luhn
        "343434343434343", // Valid Luhn
        "371144371144376", // Valid Luhn
        "341134113411347", // Valid Luhn
        "36110361103612", // Valid Luhn
        "6011000995500000", // Valid Luhn
        "36438999960016", // Valid Luhn
        "36438936438936"// Valid Luhn
        //"4444444444444440",
        //"4444414444444440",
        //"4055011111111110",
        //"4112344112344110",
        //"4110144110144110",
        //"4114360123456780",
        //"4061724061724060",
        //"5500005555555550",
        //"5555555555555550",
        //"5555515555555550",
        //"5528621111111110",
        //"5581582222222220",
        //"5474633333333330",
        //"5111005111051120",
        //"5112345112345110",
        //"5115915115915110",
        //"5116601234567890",
        //"6011016011016010",
        //"6559906559906550",
        //"6500000000000000",
        //"3566002020140000",
        //"3566003566003560"
      */
      CreditCard cc = new CreditCard
      {
        CreditCardNumber = CC,
        ExpirationDate = "0715",
        CardHolderName = "Joe Test",
        Address = "123 Any street",
        City = "Los Angeles",
        State = "CA",
        Zip = "90049"
      };
      // should fail because of bad state
      user = client.Post<User>("user/card", cc);
      client.AssertHttpOK();
      Assert.IsTrue(user.HasValidPaymentMethod);

      CUST.AccountServicesPortTypeClient customerClient = new CUST.AccountServicesPortTypeClient(
        "AccountServicesPort",
        "https://iiwscommerce.intelius.com/commerce/2.1/customer.php");

      TRAN.TransactionServicesPortTypeClient transactionClient = new TRAN.TransactionServicesPortTypeClient(
        "TransactionServicesPort",
        "https://iiwscommerce.intelius.com/commerce/2.1/transaction.php");

      CUST.AuthenticateUserResponse authenticateResponse = await customerClient.AuthenticateUserAsync(
        GetAuthenticateRequest(strEmailAddress: user.Email, strPassword: "Password1."));
      Assert.AreEqual("1000", authenticateResponse.CompletionResponse.CompletionCode);

      TRAN.ValidateOrderResponse validateResponse = await transactionClient.ValidateOrderAsync(
        GetValidateRequest(
          authenticateResponse.User.UserToken));
      Assert.AreEqual("1000", validateResponse.CompletionResponse.CompletionCode);

      TRAN.CreateOrderResponse createResponse = await transactionClient.CreateOrderAsync(
        GetCreateOrderRequest(
          authenticateResponse.User.UserToken, 
          validateResponse.OrderHash));
      Assert.AreEqual("1000", createResponse.CompletionResponse.CompletionCode);
    }

    [TestMethod]
    public async Task Transaction_GetOrderDetailsAsync()
    {
      Assert.Inconclusive();
      CUST.AccountServicesPortTypeClient customerClient = new CUST.AccountServicesPortTypeClient();
      TRAN.TransactionServicesPortTypeClient transactionClient = new TRAN.TransactionServicesPortTypeClient();

      CUST.AuthenticateUserResponse authenticateResponse = await customerClient.AuthenticateUserAsync(GetAuthenticateRequest());
      Assert.AreEqual("1000", authenticateResponse.CompletionResponse.CompletionCode);

      TRAN.ValidateOrderResponse validateResponse = await transactionClient.ValidateOrderAsync(GetValidateRequest(authenticateResponse.User.UserToken));
      Assert.AreEqual("1000", validateResponse.CompletionResponse.CompletionCode);

      TRAN.CreateOrderResponse createOrderResponse = await transactionClient.CreateOrderAsync(GetCreateOrderRequest(authenticateResponse.User.UserToken, validateResponse.OrderHash));
      Assert.AreEqual("1000", createOrderResponse.CompletionResponse.CompletionCode);

      TRAN.GetOrderDetailRequest orderDetailRequest = new TRAN.GetOrderDetailRequest
      {
        ClientAuth = _transactionClientAuth,
        OrderID = createOrderResponse.Order.OrderID,
        UserContext = GetUserContext(authenticateResponse.User.UserToken)
      };
      TRAN.GetOrderDetailResponse orderDetailResponse = await transactionClient.GetOrderDetailAsync(orderDetailRequest);
      Assert.AreEqual("1000", orderDetailResponse.CompletionResponse.CompletionCode);
    }


    private TRAN.ValidateOrderRequest GetValidateRequest(string strUserToken)
    {
      return new TRAN.ValidateOrderRequest
      {
        RequestContext = GetRequestContext(),
        UserContext = GetUserContext(strUserToken),
        Channel = GetChannel(),
        ClientAuth = _transactionClientAuth,
        CVV = "111",
        PurchaseContext = GetPurchaseContext(),
        PurchaseItemList = new TRAN.PurchaseItemType[] {
          new TRAN.PurchaseItemType
          {
            ProductID = 450, // Instant People Search Report
            ProductIDSpecified = true,
            //Discount = new TRAN.DiscountType { Price = 0.95m, Text = "Identity Protect" },
            ReferenceID = "08CK12K54VN" // brian lee smith
          },
          new TRAN.PurchaseItemType
          {
            ProductID = 420, // Identity Product
            ProductIDSpecified = true
          }
        }
      };
    }

    [TestMethod]
    public async Task Transaction_GetCreditsAsync()
    {
      Assert.Inconclusive();
      CUST.AccountServicesPortTypeClient customerClient = new CUST.AccountServicesPortTypeClient();
      TRAN.TransactionServicesPortTypeClient transactionClient = new TRAN.TransactionServicesPortTypeClient();

      CUST.AuthenticateUserResponse authenticateResponse = await customerClient.AuthenticateUserAsync(GetAuthenticateRequest());
      Assert.AreEqual("1000", authenticateResponse.CompletionResponse.CompletionCode);

      TRAN.GetCreditsRequest getCreditsRequest = new TRAN.GetCreditsRequest
      {
        ClientAuth = _transactionClientAuth,
        UserID = authenticateResponse.User.UserID
      };
      TRAN.GetCreditsResponse getCreditsResponse = await transactionClient.GetCreditsAsync(getCreditsRequest);
      Assert.AreEqual("1000", getCreditsResponse.CompletionResponse.CompletionCode);

      Assert.IsTrue(getCreditsResponse.Credits.Length > 0);

    }


    [TestMethod]
    public async Task Transaction_GetPurchaseHistoryAsync()
    {
      Assert.Inconclusive();
      CUST.AccountServicesPortTypeClient customerClient = new CUST.AccountServicesPortTypeClient();
      TRAN.TransactionServicesPortTypeClient transactionClient = new TRAN.TransactionServicesPortTypeClient();

      CUST.AuthenticateUserResponse authenticateResponse = await customerClient.AuthenticateUserAsync(GetAuthenticateRequest());
      Assert.AreEqual("1000", authenticateResponse.CompletionResponse.CompletionCode);

      TRAN.GetPurchaseHistoryRequest purchaseHistoryRequest = new TRAN.GetPurchaseHistoryRequest
      {
        ClientAuth = _transactionClientAuth,
        DateFrom = DateTime.Today,
        DateTo = DateTime.Today.AddDays(1),
        PageSize = 50,
        StartIndex = 0,
        UserContext = GetUserContext(authenticateResponse.User.UserToken)
      };
      TRAN.GetPurchaseHistoryResponse purchaseHistoryResponse = await transactionClient.GetPurchaseHistoryAsync(purchaseHistoryRequest);
      Assert.AreEqual("1000", purchaseHistoryResponse.CompletionResponse.CompletionCode);
      Assert.IsTrue(purchaseHistoryResponse.OrderHistory.Length > 0);
    }

    private TRAN.PurchaseContextType GetPurchaseContext()
    {
      return new TRAN.PurchaseContextType
      {
        ClientTime = DateTime.Now,
        ClientTimeSpecified = true,
        Language = "en",
        OrderLocation = "",
        RemoteIP = "127.0.0.1",
        TimeZone = -7,
        TimeZoneSpecified = true,
        UserAgent = ""
      };
    }

    private CUST.LoginRequest GetLoginRequest( string strEmailAddress=null, string strPassword=null)
    {
      return new CUST.LoginRequest
      {
        UserName = strEmailAddress??_strEmailAddress,
        Password = strPassword??_strPassword,
        RemoteIp = "127.0.0.1"
      };
    }

    private TRAN.UserContextType GetUserContext( string strUserToken )
    {
      return new TRAN.UserContextType
      {
        UserToken = strUserToken,
        EmailAddress = _strEmailAddress
      };
    }

    private TRAN.ChannelType GetChannel()
    {
      return new TRAN.ChannelType
      {
        AffiliateReferenceID = "",
        ChannelAdWord = "",
        ChannelID = 1
      };
    }

    private TRAN.RequestContext GetRequestContext()
    {
      return new TRAN.RequestContext
      {
        AccountList = "",
        EndUserIpAddress = "127.0.0.1"
      };
    }

    private CUST.AuthenticateUserRequest GetAuthenticateRequest(string strEmailAddress=null, string strPassword=null)
    {
      return new CUST.AuthenticateUserRequest
      {
        ClientAuth = _customerClientAuth,
        LoginRequest = GetLoginRequest(strEmailAddress: strEmailAddress, strPassword: strPassword),
      };
    }

    private TRAN.CreateOrderRequest GetCreateOrderRequest(string strUserToken, string strOrderHash)
    {
      return new TRAN.CreateOrderRequest
      {
        Channel = GetChannel(),
        ClientAuth = _transactionClientAuth,
        RequestContext = GetRequestContext(),
        RequestReference = new TRAN.RequestReferenceType
        {
          ChannelID = ""
        },
        UserContext = GetUserContext(strUserToken),
        OrderHash = strOrderHash
      };
    }
  }
}
